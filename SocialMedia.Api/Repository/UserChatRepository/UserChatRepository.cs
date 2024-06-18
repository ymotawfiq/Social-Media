

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.UserChatRepository
{
    public class UserChatRepository : IUserChatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserChatRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<UserChat> AddAsync(UserChat t)
        {
            await _dbContext.UserChats.AddAsync(t);
            await SaveChangesAsync();
            return new UserChat
            {
                Id = t.Id,
                User1Id = t.User1Id,
                User2Id = t.User2Id
            };
        }

        public async Task<UserChat> DeleteByIdAsync(string id)
        {
            var chat = await GetByIdAsync(id);
            _dbContext.UserChats.Remove(chat);
            await SaveChangesAsync();
            return chat;
        }

        public async Task<IEnumerable<UserChat>> GetAllAsync()
        {
            return await _dbContext.UserChats.Select(e => new UserChat
            {
                Id = e.Id,
                User1Id = e.User1Id,
                User2Id = e.User2Id
            }).ToListAsync();
        }

        public async Task<UserChat> GetByIdAsync(string id)
        {
            return (await _dbContext.UserChats.Select(e => new UserChat
            {
                User2Id = e.User2Id,
                Id = e.Id,
                User1Id = e.User1Id
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<UserChat> GetByUser1AndUser2Async(string user1Id, string user2Id)
        { 
            var case1 = 
            (await _dbContext.UserChats.Select(e => new UserChat
            {
                User2Id = e.User2Id,
                Id = e.Id,
                User1Id = e.User1Id
            }).Where(e => e.User1Id == user1Id).Where(e=>e.User2Id == user2Id).FirstOrDefaultAsync())!;
            var case2 =
            (await _dbContext.UserChats.Select(e => new UserChat
            {
                User2Id = e.User2Id,
                Id = e.Id,
                User1Id = e.User1Id
            }).Where(e => e.User1Id == user2Id).Where(e => e.User2Id == user1Id).FirstOrDefaultAsync())!;
            return case1 == null ? case2! : case1;
        }

        public async Task<IEnumerable<UserChat>> GetUserChatsAsync(string userId)
        {
            return from c in await GetAllAsync()
                   where c.User1Id == userId || c.User2Id == userId
                   select (new UserChat
                   {
                       User2Id = c.User2Id,
                       User1Id = c.User1Id,
                       Id = c.Id
                   });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserChat> UpdateAsync(UserChat t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}

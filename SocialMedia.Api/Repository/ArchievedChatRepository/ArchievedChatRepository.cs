

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.ArchievedChatRepository
{
    public class ArchievedChatRepository : IArchievedChatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ArchievedChatRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<ArchievedChat> AddAsync(ArchievedChat t)
        {
            await _dbContext.ArchievedChats.AddAsync(t);
            await SaveChangesAsync();
            return new ArchievedChat
            {
                ChatId = t.ChatId,
                Id = t.Id,
                UserId = t.UserId
            };
        }

        public async Task<ArchievedChat> DeleteByIdAsync(string id)
        {
            var chat = await GetByIdAsync(id);
            _dbContext.ArchievedChats.Remove(chat);
            await SaveChangesAsync();
            return chat;
        }

        public async Task<IEnumerable<ArchievedChat>> GetAllAsync()
        {
            return await _dbContext.ArchievedChats.Select(e=>new ArchievedChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                UserId = e.UserId
            }).ToListAsync();
        }

        public async Task<ArchievedChat> GetByChatAndUserIdAsync(string chatId, string userId)
        {
            return (await _dbContext.ArchievedChats.Select(e => new ArchievedChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                UserId = e.UserId
            }).Where(e=>e.ChatId == chatId).Where(e=>e.UserId == userId).FirstOrDefaultAsync())!;
        }

        public async Task<ArchievedChat> GetByIdAsync(string id)
        {
            return (await _dbContext.ArchievedChats.Select(e => new ArchievedChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                UserId = e.UserId
            }).Where(e=>e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<ArchievedChat>> GetUserArchievedChatsAsync(string userId)
        {
            return from c in await GetAllAsync()
                   where c.UserId == userId
                   select (new ArchievedChat
                   {
                       UserId = c.UserId,
                       ChatId = c.ChatId,
                       Id = c.Id
                   });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ArchievedChat> UpdateAsync(ArchievedChat t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}

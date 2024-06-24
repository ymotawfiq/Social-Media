using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;

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
            await _dbContext.AddAsync(t);
            await SaveChangesAsync();
            return new ArchievedChat
            {
                ChatId = t.ChatId,
                Id = t.Id,
                UserId = t.UserId,
            };
        }

        public async Task<ArchievedChat> DeleteByIdAsync(string id)
        {
            var archievedChat = await GetByIdAsync(id);
            _dbContext.ArchievedChat.Remove(archievedChat);
            await SaveChangesAsync();
            return new ArchievedChat
            {
                ChatId = archievedChat.ChatId,
                Id = archievedChat.Id,
                UserId = archievedChat.UserId,
            };
        }

        public async Task<IEnumerable<ArchievedChat>> GetAllAsync()
        {
            return await _dbContext.ArchievedChat.Select(e => new ArchievedChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                UserId = e.UserId,
            }).ToListAsync();
        }

        public async Task<IEnumerable<ArchievedChat>> GetAllByUserIdAsync(string userId)
        {
            return
                from a in await GetAllAsync()
                where a.UserId == userId
                select (new ArchievedChat
                {
                    UserId = a.UserId,
                    ChatId = a.ChatId,
                    Id = a.Id,
                });
        }

        public async Task<ArchievedChat> GetByIdAsync(string id)
        {
            return (await _dbContext.ArchievedChat.Select(e => new ArchievedChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                UserId = e.UserId,
            }).Where(e=>e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<ArchievedChat> GetByUserAndChatIdAsync(string userId, string chatId)
        {
            return (await _dbContext.ArchievedChat.Select(e => new ArchievedChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                UserId = e.UserId,
            }).Where(e => e.ChatId == chatId).Where(e=>e.UserId == userId).FirstOrDefaultAsync())!;
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

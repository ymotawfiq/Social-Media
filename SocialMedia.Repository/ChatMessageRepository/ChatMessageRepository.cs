

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.ChatMessageRepository
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatMessageRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<ChatMessage> AddAsync(ChatMessage t)
        {
            await _dbContext.ChatMessages.AddAsync(t);
            await SaveChangesAsync();
            return new ChatMessage
            {
                ChatId = t.ChatId,
                Id = t.Id,
                Message = t.Message,
                Photo = t.Photo,
                SenderId = t.SenderId,
                SentAt = t.SentAt
            };
        }

        public async Task<ChatMessage> DeleteByIdAsync(string id)
        {
            var message = await GetByIdAsync(id);
            _dbContext.ChatMessages.Remove(message);
            await SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<ChatMessage>> GetAllAsync()
        {
            return await _dbContext.ChatMessages.Select(e => new ChatMessage
            {
                ChatId = e.ChatId,
                Id = e.Id,
                Message = e.Message,
                Photo = e.Photo,
                SenderId = e.SenderId,
                SentAt = e.SentAt
            }).ToListAsync();
        }

        public async Task<ChatMessage> GetByIdAsync(string id)
        {
            return (await _dbContext.ChatMessages.Select(e => new ChatMessage
            {
                ChatId = e.ChatId,
                Id = e.Id,
                Message = e.Message,
                Photo = e.Photo,
                SenderId = e.SenderId,
                SentAt = e.SentAt
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<ChatMessage>> GetUserSentMessagesAsync(SiteUser user, string chatId)
        {
            return from m in await GetAllAsync()
                   where m.SenderId == user.Id && m.ChatId == chatId 
                   orderby m.SentAt
                   select (new ChatMessage
                   {
                       Id = m.Id,
                       SenderId = m.SenderId,
                       ChatId = m.ChatId,
                       Message = m.Message,
                       Photo = m.Photo,
                       SentAt = m.SentAt
                   });
        }

        public async Task<bool> IsChatEmptyAsync(SiteUser user, string chatId)
        {
            return (await GetUserSentMessagesAsync(user, chatId)).ToList().Count == 0;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChatMessage> UpdateAsync(ChatMessage t)
        {
            var message = await GetByIdAsync(t.Id);
            if (t.Message != null)
            {
                message.Message = t.Message;
            }
            else
            {
                message.Photo = t.Photo;
            }
            await SaveChangesAsync();
            return new ChatMessage
            {
                Message = message.Message,
                ChatId = message.ChatId,
                Id = message.Id,
                SenderId = message.SenderId,
                SentAt = message.SentAt
            };
        }
    }
}

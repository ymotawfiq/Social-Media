using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.ChatMessageRepository
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
            await _dbContext.AddAsync(t);
            await SaveChangesAsync();
            return new ChatMessage
            {
                ChatId = t.ChatId,
                Id = t.Id,
                Message = t.Message,
                Photo = t.Photo,
                SenderId = t.SenderId,
                SentAt = t.SentAt,
                UpdatedAt = t.UpdatedAt
            };
        }

        public async Task<ChatMessage> DeleteByIdAsync(string id)
        {
            var message = await GetByIdAsync(id);
            _dbContext.Remove(message);
            await SaveChangesAsync();
            return new ChatMessage
            {
                ChatId = message.ChatId,
                Id = message.Id,
                Message = message.Message,
                Photo = message.Photo,
                SenderId = message.SenderId,
                SentAt = message.SentAt,
                UpdatedAt = message.UpdatedAt
            };
        }

        public async Task<IEnumerable<ChatMessage>> GetAllAsync()
        {
            return await _dbContext.ChatMessage.Select(e => new ChatMessage
            {
                ChatId = e.ChatId,
                Id = e.Id,
                Message = e.Message,
                Photo = e.Photo,
                SenderId = e.SenderId,
                SentAt = e.SentAt,
                UpdatedAt = e.UpdatedAt
            }).ToListAsync();
        }

        public async Task<ChatMessage> GetByChatIdAndMessageAsync(string chatId, string messageId)
        {
            return (await _dbContext.ChatMessage.Select(e => new ChatMessage
            {
                ChatId = e.ChatId,
                Id = e.Id,
                Message = e.Message,
                Photo = e.Photo,
                SenderId = e.SenderId,
                SentAt = e.SentAt,
                UpdatedAt = e.UpdatedAt
            }).Where(e => e.ChatId == chatId).Where(e=>e.Id == messageId).FirstOrDefaultAsync())!;
        }

        public async Task<ChatMessage> GetByIdAsync(string id)
        {
            return (await _dbContext.ChatMessage.Select(e => new ChatMessage
            {
                ChatId = e.ChatId,
                Id = e.Id,
                Message = e.Message,
                Photo = e.Photo,
                SenderId = e.SenderId,
                SentAt = e.SentAt,
                UpdatedAt = e.UpdatedAt
            }).Where(e=>e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByChatIdAsync(string chatId)
        {
            return
                from m in await GetAllAsync()
                where m.ChatId == chatId
                select (new ChatMessage
                {
                    ChatId = m.ChatId,
                    Id = m.Id,
                    Message = m.Message,
                    Photo = m.Photo,
                    SenderId = m.SenderId,
                    SentAt = m.SentAt,
                    UpdatedAt = m.UpdatedAt
                });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChatMessage> UpdateAsync(ChatMessage t)
        {
            var message = await GetByIdAsync(t.Id);
            message.Message = t.Message;
            await SaveChangesAsync();
            return new ChatMessage
            {
                ChatId = t.ChatId,
                Id = t.Id,
                Message = t.Message,
                Photo = t.Photo,
                SenderId = t.SenderId,
                SentAt = t.SentAt,
                UpdatedAt = t.UpdatedAt
            };
        }
    }
}

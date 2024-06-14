

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.SarehneRepository
{
    public class SarehneRepository : ISarehneRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SarehneRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<SarehneMessage> DeleteMessageAsync(string sarehneMessageId)
        {
            try
            {
                var message = await GetMessageAsync(sarehneMessageId);
                _dbContext.SarehneMessages.Remove(message);
                await SaveChangesAsync();
                return message;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SarehneMessage> GetMessageAsync(string sarehneMessageId)
        {
            try
            {
                return (await _dbContext.SarehneMessages.Select(e=>new SarehneMessage
                {
                    Id = e.Id,
                    Message = e.Message,
                    MessagePolicyId = e.MessagePolicyId,
                    ReceiverId = e.ReceiverId,
                    SenderName = e.SenderName,
                    SentAt = e.SentAt
                }).Where(e => e.Id == sarehneMessageId)
                     .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId)
        {
            return from m in await _dbContext.SarehneMessages.ToListAsync()
                   where m.ReceiverId == userId
                   select (new SarehneMessage
                   {
                       Id = m.Id,
                       Message = m.Message,
                       MessagePolicyId = m.MessagePolicyId,
                       ReceiverId = m.ReceiverId,
                       SenderName = m.SenderName,
                       SentAt = m.SentAt
                   });
        }

        public async Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId, string policyId)
        {
            return from m in await _dbContext.SarehneMessages.ToListAsync()
                   where m.ReceiverId == userId && m.MessagePolicyId == policyId
                   select (new SarehneMessage
                   {
                       Id = m.Id,
                       Message = m.Message,
                       MessagePolicyId = m.MessagePolicyId,
                       ReceiverId = m.ReceiverId,
                       SenderName = m.SenderName,
                       SentAt = m.SentAt
                   });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SarehneMessage> SendMessageAsync(SarehneMessage sarehneMessage)
        {
            await _dbContext.SarehneMessages.AddAsync(sarehneMessage);
            await SaveChangesAsync();
            return new SarehneMessage
            {
                Id = sarehneMessage.Id,
                Message = sarehneMessage.Message,
                MessagePolicyId = sarehneMessage.MessagePolicyId,
                ReceiverId = sarehneMessage.ReceiverId,
                SenderName = sarehneMessage.SenderName,
                SentAt = sarehneMessage.SentAt
            };
        }

        public async Task<SarehneMessage> UpdateMessagePolicyAsync(SarehneMessage sarehneMessage)
        {
            var message = await GetMessageAsync(sarehneMessage.Id);
            message.MessagePolicyId = sarehneMessage.MessagePolicyId;
            await SaveChangesAsync();
            return message;
        }
    }
}

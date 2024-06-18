

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.SarehneRepository
{
    public class SarehneRepository : ISarehneRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SarehneRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<SarehneMessage> AddAsync(SarehneMessage t)
        {
            await _dbContext.SarehneMessages.AddAsync(t);
            await SaveChangesAsync();
            return new SarehneMessage
            {
                Id = t.Id,
                Message = t.Message,
                MessagePolicyId = t.MessagePolicyId,
                ReceiverId = t.ReceiverId,
                SenderName = t.SenderName,
                SentAt = t.SentAt
            };
        }

        public async Task<SarehneMessage> DeleteByIdAsync(string id)
        {
            try
            {
                var message = await GetByIdAsync(id);
                _dbContext.SarehneMessages.Remove(message);
                await SaveChangesAsync();
                return message;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SarehneMessage>> GetAllAsync()
        {
            return await _dbContext.SarehneMessages.Select(e=>new SarehneMessage
            {
                Id = e.Id,
                Message = e.Message,
                MessagePolicyId = e.MessagePolicyId,
                ReceiverId = e.ReceiverId,
                SenderName = e.SenderName,
                SentAt = e.SentAt
            }).ToListAsync();
        }

        public async Task<SarehneMessage> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.SarehneMessages.Select(e => new SarehneMessage
                {
                    Id = e.Id,
                    Message = e.Message,
                    MessagePolicyId = e.MessagePolicyId,
                    ReceiverId = e.ReceiverId,
                    SenderName = e.SenderName,
                    SentAt = e.SentAt
                }).Where(e => e.Id == id)
                     .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId)
        {
            return from m in await GetAllAsync()
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


        public async Task<SarehneMessage> UpdateAsync(SarehneMessage t)
        {
            var message = await GetByIdAsync(t.Id);
            message.MessagePolicyId = t.MessagePolicyId;
            await SaveChangesAsync();
            return message;
        }

    }
}

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.MessageReactRepository
{
    public class MessageReactRepository : IMessageReactRepository
    {

        private readonly ApplicationDbContext _dbContext;
        public MessageReactRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<MessageReact> AddAsync(MessageReact t)
        {
            await _dbContext.AddAsync(t);
            await SaveChangesAsync();
            return new MessageReact
            {
                Id = t.Id,
                MessageId = t.MessageId,
                ReactId = t.ReactId,
                ReactedUserId = t.ReactedUserId,
                SentAt = t.SentAt,
                UpdatedAt = t.UpdatedAt
            };
        }

        public async Task<MessageReact> DeleteByIdAsync(string id)
        {
            var messageReact = await GetByIdAsync(id);
            _dbContext.MessageReact.Remove(messageReact);
            await SaveChangesAsync();
            return new MessageReact
            {
                Id = messageReact.Id,
                MessageId = messageReact.MessageId,
                ReactId = messageReact.ReactId,
                ReactedUserId = messageReact.ReactedUserId,
                SentAt = messageReact.SentAt,
                UpdatedAt = messageReact.UpdatedAt
            };
        }

        public async Task<IEnumerable<MessageReact>> GetAllAsync()
        {
            return await _dbContext.MessageReact.Select(e => new MessageReact
            {
                Id = e.Id,
                MessageId = e.MessageId,
                ReactedUserId = e.ReactedUserId,
                ReactId = e.ReactId,
                SentAt = e.SentAt,
                UpdatedAt = e.UpdatedAt
            }).ToListAsync();
        }

        public async Task<MessageReact> GetByIdAsync(string id)
        {
            return (await _dbContext.MessageReact.Select(e => new MessageReact
            {
                Id = e.Id,
                MessageId = e.MessageId,
                ReactedUserId = e.ReactedUserId,
                ReactId = e.ReactId,
                SentAt = e.SentAt,
                UpdatedAt = e.UpdatedAt
            }).Where(e=>e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<MessageReact> GetMessageReactByMessageAndUserIdAsync(string messageId, string userId)
        {
            return (await _dbContext.MessageReact.Select(e => new MessageReact
            {
                Id = e.Id,
                MessageId = e.MessageId,
                ReactedUserId = e.ReactedUserId,
                ReactId = e.ReactId,
                SentAt = e.SentAt,
                UpdatedAt = e.UpdatedAt
            }).Where(e => e.MessageId == messageId).Where(e=>e.ReactedUserId==userId).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<MessageReact>> GetMessageReactsByMessageIdAsync(string messageId)
        {
            return
                from r in await GetAllAsync()
                where r.MessageId == messageId
                select (new MessageReact
                {
                    MessageId = r.MessageId,
                    Id = r.Id,
                    ReactedUserId = r.ReactedUserId,
                    ReactId = r.ReactId,
                    SentAt = r.SentAt,
                    UpdatedAt = r.UpdatedAt
                });
        }

        public async Task SaveChangesAsync()
        {
            await SaveChangesAsync();
        }

        public async Task<MessageReact> UpdateAsync(MessageReact t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}

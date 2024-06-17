

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.MessageReactRepository
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
            await _dbContext.MessageReacts.AddAsync(t);
            await SaveChangesAsync();
            return new MessageReact
            {
                Id = t.Id,
                MessageId  = t.MessageId,
                ReactedUserId = t.ReactedUserId,
                ReactId = t.ReactId
            };
        }

        public async Task<MessageReact> DeleteByIdAsync(string id)
        {
            var messageReact = await GetByIdAsync(id);
            _dbContext.MessageReacts.Remove(messageReact);
            await SaveChangesAsync();
            return messageReact;
        }

        public async Task<IEnumerable<MessageReact>> GetAllAsync()
        {
            return await _dbContext.MessageReacts.Select(e => new MessageReact
            {
                Id = e.Id,
                MessageId = e.MessageId,
                ReactedUserId = e.ReactedUserId,
                ReactId = e.ReactId
            }).ToListAsync();
        }

        public async Task<MessageReact> GetByIdAsync(string id)
        {
            return (await _dbContext.MessageReacts.Select(e => new MessageReact
            {
                Id = e.Id,
                MessageId = e.MessageId,
                ReactedUserId = e.ReactedUserId,
                ReactId = e.ReactId
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<MessageReact>> GetMessageReactsAsync(string messageId)
        {
            return (from mr in await GetAllAsync()
                    where mr.MessageId == messageId
                    select (new MessageReact
                    {
                        MessageId = mr.MessageId,
                        Id = mr.Id,
                        ReactedUserId = mr.ReactedUserId,
                        ReactId = mr.ReactId
                    }));
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<MessageReact> UpdateAsync(MessageReact t)
        {
            var messageReact = await GetByIdAsync(t.Id);
            messageReact.ReactId = t.ReactId;
            await SaveChangesAsync();
            return new MessageReact
            {
                Id = messageReact.Id,
                MessageId = messageReact.MessageId,
                ReactedUserId = messageReact.ReactedUserId,
                ReactId = messageReact.ReactId
            };
        }
    }
}

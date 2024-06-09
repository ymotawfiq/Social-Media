

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
                return (await _dbContext.SarehneMessages.Where(e => e.Id == sarehneMessageId)
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
                   select m;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SarehneMessage> SendMessageAsync(SarehneMessage sarehneMessage)
        {
            await _dbContext.SarehneMessages.AddAsync(sarehneMessage);
            await SaveChangesAsync();
            return sarehneMessage;
        }
    }
}

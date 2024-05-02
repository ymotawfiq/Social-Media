

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.ReactRepository
{
    public class ReactRepository : IReactRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ReactRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<React> AddReactAsync(React react)
        {
            try
            {
                await _dbContext.Reacts.AddAsync(react);
                await SaveChangesAsync();
                return react;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<React> DeleteReactByIdAsync(Guid reactId)
        {
            try
            {
                var react = await GetReactByIdAsync(reactId);
                _dbContext.Reacts.Remove(react);
                await SaveChangesAsync();
                return react;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<React>> GetAllReactsAsync()
        {
            try
            {
                return await _dbContext.Reacts.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<React> GetReactByIdAsync(Guid reactId)
        {
            try
            {
                return await _dbContext.Reacts.Where(e => e.Id == reactId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<React> UpdateReactAsync(React react)
        {
            try
            {
                var react1 = await GetReactByIdAsync(react.Id);
                react1.ReactValue = react.ReactValue;
                await SaveChangesAsync();
                return react1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

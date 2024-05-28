

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.SpecialPostsReactsRepository
{
    
    public class SpecialPostsReactsRepository : ISpecialPostsReactsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SpecialPostsReactsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<SpecialPostReacts> AddSpecialPostReactsAsync(SpecialPostReacts specialPostReacts)
        {
            try
            {
                await _dbContext.SpecialPostReacts.AddAsync(specialPostReacts);
                await SaveChangesAsync();
                return specialPostReacts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialPostReacts> DeleteSpecialPostReactsByIdAsync(string Id)
        {
            try
            {
                var postReact = await GetSpecialPostReactsByIdAsync(Id);
                _dbContext.SpecialPostReacts.Remove(postReact);
                await SaveChangesAsync();
                return postReact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialPostReacts> DeleteSpecialPostReactsByReactIdAsync(string reactId)
        {
            try
            {
                var postReact = await GetSpecialPostReactsByReactIdAsync(reactId);
                _dbContext.SpecialPostReacts.Remove(postReact);
                await SaveChangesAsync();
                return postReact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SpecialPostReacts>> GetSpecialPostReactsAsync()
        {
            try
            {
                return await _dbContext.SpecialPostReacts.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialPostReacts> GetSpecialPostReactsByIdAsync(string Id)
        {
            try
            {
                return (await _dbContext.SpecialPostReacts.Where(e => e.Id == Id)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialPostReacts> GetSpecialPostReactsByReactIdAsync(string reactId)
        {
            try
            {
                return (await _dbContext.SpecialPostReacts.Where(e => e.ReactId == reactId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SpecialPostReacts> UpdateSpecialPostReactsAsync(SpecialPostReacts specialPostReacts)
        {
            try
            {
                var postReact = await GetSpecialPostReactsByIdAsync(specialPostReacts.Id);
                postReact.ReactId = specialPostReacts.ReactId;
                await SaveChangesAsync();
                return postReact;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

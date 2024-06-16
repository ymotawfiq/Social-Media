

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

        public async Task<React> AddAsync(React t)
        {
            try
            {
                t.ReactValue = t.ReactValue.ToUpper();
                await _dbContext.Reacts.AddAsync(t);
                await SaveChangesAsync();
                return new React
                {
                    Id = t.Id,
                    ReactValue = t.ReactValue
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<React> DeleteByIdAsync(string id)
        {
            try
            {
                var react = await GetByIdAsync(id);
                _dbContext.Reacts.Remove(react);
                await SaveChangesAsync();
                return react;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<React>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Reacts.Select(e => new React
                {
                    ReactValue = e.ReactValue,
                    Id = e.Id
                }).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<React> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.Reacts.Select(e => new React
                {
                    ReactValue = e.ReactValue,
                    Id = e.Id
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<React> GetReactByNameAsync(string reactName)
        {
            try
            {
                reactName = reactName.ToUpper();
                return (await _dbContext.Reacts.Select(e => new React
                {
                    ReactValue = e.ReactValue,
                    Id = e.Id
                }).Where(e => e.ReactValue == reactName).FirstOrDefaultAsync())!;
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

        public async Task<React> UpdateAsync(React t)
        {
            try
            {
                t.ReactValue = t.ReactValue.ToUpper();
                var react1 = await GetByIdAsync(t.Id);
                react1.ReactValue = t.ReactValue;
                await SaveChangesAsync();
                return new React
                {
                    Id = react1.Id,
                    ReactValue = react1.ReactValue
                };
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
                react.ReactValue = react.ReactValue.ToUpper();
                var react1 = await GetByIdAsync(react.Id);
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



using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.SpecialCommentReactsRepository
{
    public class SpecialCommentReactsRepository : ISpecialCommentReactsRepository
    {

        private readonly ApplicationDbContext _dbContext;
        public SpecialCommentReactsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<SpecialCommentReacts> AddSpecialCommentReactsAsync(
            SpecialCommentReacts specialCommentReacts)
        {
            try
            {
                await _dbContext.SpecialCommentReacts.AddAsync(specialCommentReacts);
                await SaveChangesAsync();
                return specialCommentReacts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialCommentReacts> DeleteSpecialCommentReactsByIdAsync(string Id)
        {
            try
            {
                var CommentReact = await GetSpecialCommentReactsByIdAsync(Id);
                _dbContext.SpecialCommentReacts.Remove(CommentReact);
                await SaveChangesAsync();
                return CommentReact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialCommentReacts> DeleteSpecialCommentReactsByReactIdAsync(string reactId)
        {
            try
            {
                var CommentReact = await GetSpecialCommentReactsByReactIdAsync(reactId);
                _dbContext.SpecialCommentReacts.Remove(CommentReact);
                await SaveChangesAsync();
                return CommentReact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SpecialCommentReacts>> GetSpecialCommentReactsAsync()
        {
            try
            {
                return await _dbContext.SpecialCommentReacts.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialCommentReacts> GetSpecialCommentReactsByIdAsync(string Id)
        {
            try
            {
                return (await _dbContext.SpecialCommentReacts.Where(e => e.Id == Id)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialCommentReacts> GetSpecialCommentReactsByReactIdAsync(string reactId)
        {
            try
            {
                return (await _dbContext.SpecialCommentReacts.Where(e => e.ReactId == reactId)
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

        public async Task<SpecialCommentReacts> UpdateSpecialCommentReactsAsync(
            SpecialCommentReacts specialCommentReacts)
        {
            try
            {
                var CommentReact = await GetSpecialCommentReactsByIdAsync(specialCommentReacts.Id);
                CommentReact.ReactId = specialCommentReacts.ReactId;
                await SaveChangesAsync();
                return CommentReact;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

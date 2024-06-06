

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PagesFollowersRepository
{
    public class PagesFollowersRepository : IPagesFollowersRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PagesFollowersRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<PageFollower> FollowPageAsync(PageFollower pageFollower)
        {
            try
            {
                await _dbContext.PageFollowers.AddAsync(pageFollower);
                await SaveChangesAsync();
                return pageFollower;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PageFollower> GetPageFollowerByIdAsync(string pageFollowerId)
        {
            try
            {
                return (await _dbContext.PageFollowers.Where(e=>e.Id==pageFollowerId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PageFollower> GetPageFollowerByPageIdAndFollowerIdAsync(
            string pageId, string followerId)
        {
            try
            {
                return (await _dbContext.PageFollowers.Where(e => e.PageId == pageId)
                    .Where(e=>e.FollowerId==followerId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PageFollower>> GetPageFollowersAsync(string pageId)
        {
            return from p in await _dbContext.PageFollowers.ToListAsync()
                   where p.PageId == pageId
                   select p;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PageFollower> UnfollowPageByPageFollowerIdAsync(string pageFollowerId)
        {
            try
            {
                var pageFollower = await GetPageFollowerByIdAsync(pageFollowerId);
                _dbContext.PageFollowers.Remove(pageFollower);
                await SaveChangesAsync();
                return pageFollower;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PageFollower> UnfollowPageByPageIdAsync(string pageId, string followerId)
        {
            try
            {
                var pageFollower = await GetPageFollowerByPageIdAndFollowerIdAsync(
                    pageId, followerId);
                _dbContext.PageFollowers.Remove(pageFollower);
                await SaveChangesAsync();
                return pageFollower;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}



using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.PagesFollowersRepository
{
    public class PagesFollowersRepository : IPagesFollowersRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PagesFollowersRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<PageFollower> AddAsync(PageFollower t)
        {
            try
            {
                await _dbContext.PageFollowers.AddAsync(t);
                await SaveChangesAsync();
                return new PageFollower
                {
                    PageId = t.PageId,
                    Id = t.Id,
                    FollowerId = t.FollowerId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PageFollower> DeleteByIdAsync(string id)
        {
            try
            {
                var pageFollower = await GetByIdAsync(id);
                _dbContext.PageFollowers.Remove(pageFollower);
                await SaveChangesAsync();
                return pageFollower;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PageFollower>> GetAllAsync()
        {
            return await _dbContext.PageFollowers.Select(e=>new PageFollower
            {
                Id = e.Id,
                PageId = e.PageId,
                FollowerId = e.FollowerId
            }).ToListAsync();
        }

        public async Task<PageFollower> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.PageFollowers.Select(e => new PageFollower
                {
                    FollowerId = e.FollowerId,
                    Id = e.Id,
                    PageId = e.PageId
                }).Where(e => e.Id == id)
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
                return (await _dbContext.PageFollowers.Select(e => new PageFollower
                {
                    FollowerId = e.FollowerId,
                    Id = e.Id,
                    PageId = e.PageId
                }).Where(e => e.PageId == pageId).Where(e=>e.FollowerId==followerId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PageFollower>> GetPageFollowersAsync(string pageId)
        {
            return from p in await GetAllAsync()
                   where p.PageId == pageId
                   select (new PageFollower
                   {
                       PageId = p.PageId,
                       Id = p.Id,
                       FollowerId = p.FollowerId
                   });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
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

        public async Task<PageFollower> UpdateAsync(PageFollower t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}

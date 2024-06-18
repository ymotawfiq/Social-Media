
using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.FollowerRepository
{
    public class FollowerRepository : IFollowerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FollowerRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Follower> AddAsync(Follower t)
        {
            try
            {
                await _dbContext.Followers.AddAsync(t);
                await SaveChangesAsync();
                return new Follower
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    FollowerId = t.FollowerId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Follower> DeleteByIdAsync(string id)
        {
            var follow = await GetByIdAsync(id);
            _dbContext.Followers.Remove(follow);
            await SaveChangesAsync();
            return new Follower
            {
                FollowerId = follow.FollowerId,
                Id = follow.Id,
                UserId = follow.UserId
            };
        }



        public async Task<IEnumerable<Follower>> GetAllAsync()
        {
            return await _dbContext.Followers.Select(e=>new Follower
            {
                FollowerId = e.FollowerId,
                Id = e.Id,
                UserId = e.UserId
            }).ToListAsync();
        }

        public async Task<IEnumerable<Follower>> GetAllAsync(string userId)
        {
            return
                from f in await GetAllAsync()
                where f.UserId == userId
                select (new Follower
                {
                    FollowerId = f.FollowerId,
                    UserId = f.UserId,
                    Id = f.Id
                });
        }

        public async Task<Follower> GetByIdAsync(string id)
        {
            return (await _dbContext.Followers.Select(e => new Follower
            {
                UserId = e.UserId,
                FollowerId = e.FollowerId,
                Id = e.Id
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<Follower> GetByUserIdAndFollowerIdAsync(string userId, string followerId)
        {
            return (await _dbContext.Followers.Select(e => new Follower
            {
                Id = e.Id,
                FollowerId = e.FollowerId,
                UserId = e.UserId
            }).Where(e => e.FollowerId == followerId).Where(e=>e.UserId==userId).FirstOrDefaultAsync())!;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Follower> UpdateAsync(string userId, string followerId)
        {
            var followingInfo = await GetByUserIdAndFollowerIdAsync(userId, followerId);
            _dbContext.Followers.Remove(followingInfo);
            await SaveChangesAsync();
            followingInfo!.User = null;
            return followingInfo;
        }

        public async Task<Follower> UpdateAsync(Follower t)
        {
            var follow = await GetByIdAsync(t.Id);
            _dbContext.Followers.Remove(follow);
            await SaveChangesAsync();
            return new Follower
            {
                Id = follow.Id,
                FollowerId = follow.FollowerId,
                UserId = follow.UserId
            };
        }
    }
}

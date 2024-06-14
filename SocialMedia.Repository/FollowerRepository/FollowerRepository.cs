
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.FollowerRepository
{
    public class FollowerRepository : IFollowerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FollowerRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<Follower> FollowAsync(Follower follower)
        {
            try
            {
                await _dbContext.Followers.AddAsync(follower);
                await SaveChangesAsync();
                return new Follower
                {
                    Id = follower.Id,
                    UserId = follower.UserId,
                    FollowerId = follower.FollowerId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Follower>> GetAllFollowers(string userId)
        {
            return
                from f in await _dbContext.Followers.ToListAsync()
                where f.UserId == userId
                select (new Follower
                {
                    FollowerId = f.FollowerId,
                    UserId = f.UserId,
                    Id = f.Id
                });
        }

        public async Task<Follower> GetFollowingByUserIdAndFollowerIdAsync(string userId, string followerId)
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

        public async Task<Follower> UnfollowAsync(string userId, string followerId)
        {
            var followingInfo = await GetFollowingByUserIdAndFollowerIdAsync(userId, followerId);
            _dbContext.Followers.Remove(followingInfo);
            await SaveChangesAsync();
            followingInfo!.User = null;
            return followingInfo;
        }

    }
}

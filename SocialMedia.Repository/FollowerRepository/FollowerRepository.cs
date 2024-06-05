
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.FollowerRepository
{
    public class FollowerRepository : IFollowerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<SiteUser> _userManager;
        public FollowerRepository(ApplicationDbContext _dbContext, UserManager<SiteUser> _userManager)
        {
            this._dbContext = _dbContext;
            this._userManager = _userManager;
        }
        public async Task<Follower> FollowAsync(Follower follower)
        {
            try
            {
                await _dbContext.Followers.AddAsync(follower);
                await SaveChangesAsync();
                follower.User = null;
                return follower;
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
            return (await _dbContext.Followers.Where(e => e.UserId == userId)
                .Where(e => e.FollowerId == followerId).FirstOrDefaultAsync())!;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Follower> UnfollowAsync(string userId, string followerId)
        {
            var followingInfo = await _dbContext.Followers.Where(e => e.UserId == userId)
                .Where(e => e.FollowerId == followerId).FirstOrDefaultAsync();
            _dbContext.Followers.Remove(followingInfo!);
            await SaveChangesAsync();
            followingInfo!.User = null;
            return followingInfo;
        }

    }
}

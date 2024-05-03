
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.FollowerRepository
{
    public interface IFollowerRepository
    {
        Task<Follower> FollowAsync(Follower follower);
        Task<Follower> UnfollowAsync(string userId, string followerId);
        Task<Follower> GetFollowingByUserIdAndFollowerIdAsync(string userId, string followerId);
        Task<IEnumerable<Follower>> GetAllFollowers(string userId);
        Task SaveChangesAsync();
    }
}

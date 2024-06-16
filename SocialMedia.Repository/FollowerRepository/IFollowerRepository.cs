
using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.FollowerRepository
{
    public interface IFollowerRepository : ICrud<Follower>
    {
        Task<Follower> UpdateAsync(string userId, string followerId);
        Task<Follower> GetByUserIdAndFollowerIdAsync(string userId, string followerId);
        Task<IEnumerable<Follower>> GetAllAsync(string userId);
    }
}

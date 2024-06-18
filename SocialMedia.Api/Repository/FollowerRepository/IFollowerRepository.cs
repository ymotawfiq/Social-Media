
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.FollowerRepository
{
    public interface IFollowerRepository : ICrud<Follower>
    {
        Task<Follower> UpdateAsync(string userId, string followerId);
        Task<Follower> GetByUserIdAndFollowerIdAsync(string userId, string followerId);
        Task<IEnumerable<Follower>> GetAllAsync(string userId);
    }
}

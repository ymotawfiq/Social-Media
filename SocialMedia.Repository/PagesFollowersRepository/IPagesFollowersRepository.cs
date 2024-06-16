

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.PagesFollowersRepository
{
    public interface IPagesFollowersRepository : ICrud<PageFollower>
    {
        Task<PageFollower> UnfollowPageByPageIdAsync(string pageId, string followerId);
        Task<IEnumerable<PageFollower>> GetPageFollowersAsync(string pageId);
        Task<PageFollower> GetPageFollowerByPageIdAndFollowerIdAsync(string pageId, string followerId);
    }
}

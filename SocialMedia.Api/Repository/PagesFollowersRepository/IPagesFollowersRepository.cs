

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.PagesFollowersRepository
{
    public interface IPagesFollowersRepository : ICrud<PageFollower>
    {
        Task<PageFollower> UnfollowPageByPageIdAsync(string pageId, string followerId);
        Task<IEnumerable<PageFollower>> GetPageFollowersAsync(string pageId);
        Task<PageFollower> GetPageFollowerByPageIdAndFollowerIdAsync(string pageId, string followerId);
    }
}

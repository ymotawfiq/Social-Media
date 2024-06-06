

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PagesFollowersRepository
{
    public interface IPagesFollowersRepository
    {
        Task<PageFollower> FollowPageAsync(PageFollower pageFollower);
        Task<PageFollower> UnfollowPageByPageIdAsync(string pageId, string followerId);
        Task<PageFollower> UnfollowPageByPageFollowerIdAsync(string pageFollowerId);
        Task<IEnumerable<PageFollower>> GetPageFollowersAsync(string pageId);
        Task<PageFollower> GetPageFollowerByIdAsync(string pageFollowerId);
        Task<PageFollower> GetPageFollowerByPageIdAndFollowerIdAsync(string pageId, string followerId);
        Task SaveChangesAsync();
    }
}

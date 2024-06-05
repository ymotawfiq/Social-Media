

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.PageRepository
{
    public interface IPageRepository
    {
        Task<Page> AddPageAsync(Page page, SiteUser user);
        Task<Page> UpdatePageAsync(Page page);
        Task<Page> GetPageByIdAsync(string pageId);
        Task<UserPage> GetUserPageByPageIdAsync(string pageId);
        Task<Page> DeletePageByIdAsync(string pageId);
        Task<IEnumerable<UserPage>> GetPagesByUserIdAsync(string userId);
        Task SaveChangesAsync();
    }
}

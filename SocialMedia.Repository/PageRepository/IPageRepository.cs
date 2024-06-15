

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.PageRepository
{
    public interface IPageRepository
    {
        Task<Page> AddPageAsync(Page page);
        Task<Page> UpdatePageAsync(Page page);
        Task<Page> GetPageByIdAsync(string pageId);
        Task<Page> DeletePageByIdAsync(string pageId);
        Task<IEnumerable<Page>> GetPagesByUserIdAsync(string userId);
        Task SaveChangesAsync();
    }
}

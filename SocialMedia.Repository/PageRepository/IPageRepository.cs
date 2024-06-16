

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.PageRepository
{
    public interface IPageRepository : ICrud<Page>
    {
        Task<IEnumerable<Page>> GetPagesByUserIdAsync(string userId);
    }
}



using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.PageRepository
{
    public interface IPageRepository : ICrud<Page>
    {
        Task<IEnumerable<Page>> GetPagesByUserIdAsync(string userId);
    }
}

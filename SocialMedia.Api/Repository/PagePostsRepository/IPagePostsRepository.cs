

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.PagePostsRepository
{
    public interface IPagePostsRepository : ICrud<PagePost>
    {
        Task<PagePost> GetPagePostByPostIdAsync(string postId);
    }
}

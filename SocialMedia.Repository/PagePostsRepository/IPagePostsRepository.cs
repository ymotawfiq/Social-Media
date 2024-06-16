

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.PagePostsRepository
{
    public interface IPagePostsRepository : ICrud<PagePost>
    {
        Task<PagePost> GetPagePostByPostIdAsync(string postId);
    }
}

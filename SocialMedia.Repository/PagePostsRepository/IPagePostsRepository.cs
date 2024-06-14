

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.PagePostsRepository
{
    public interface IPagePostsRepository
    {
        Task<PagePost> AddPagePostAsync(PagePost pagePosts);
        Task<PagePost> GetPagePostByIdAsync(string pagePostId);
        Task<PagePost> DeletePagePostByIdAsync(string pagePostId);
        Task SaveChangesAsync();
    }
}



using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.PagePostsRepository
{
    public interface IPagePostsRepository
    {
        Task<PagePosts> AddPagePostAsync(PagePosts pagePosts);
        Task<PagePosts> GetPagePostByIdAsync(string pagePostId);
        Task<PagePosts> DeletePagePostByIdAsync(string pagePostId);
        Task SaveChangesAsync();
    }
}

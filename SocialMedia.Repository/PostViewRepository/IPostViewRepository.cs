
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostViewRepository
{
    public interface IPostViewRepository
    {
        Task<PostView> AddPostViewAsync(PostView postView);
        Task<PostView> UpdatePostViewAsync(PostView postView);
        Task<IEnumerable<PostView>> GetPostViews(string postId);
        Task<PostView> GetPostViewByPostIdAsync(string postId);
        Task SaveChangesAsync();
    }
}

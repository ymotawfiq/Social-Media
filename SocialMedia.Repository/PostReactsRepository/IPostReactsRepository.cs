
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostReactsRepository
{
    public interface IPostReactsRepository
    {

        Task<PostReacts> AddPostReactAsync(PostReacts postReacts);
        Task<PostReacts> UpdatePostReactAsync(PostReacts postReacts);
        Task<PostReacts> GetPostReactByIdAsync(string Id);
        Task<PostReacts> GetPostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<PostReacts> DeletePostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<PostReacts> DeletePostReactByIdAsync(string Id);
        Task<IEnumerable<PostReacts>> GetPostReactsByPostIdAsync(string postId);
        Task SaveChangesAsync();
    }
}



using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostCommentsRepository
{
    public interface IPostCommentsRepository
    {
        Task<PostComment> AddPostCommentAsync(PostComment postComments);
        Task<PostComment> UpdatePostCommentAsync(PostComment postComments);
        Task<PostComment> DeletePostCommentByIdAsync(string postCommentId);
        Task<PostComment> GetPostCommentByIdAsync(string postCommentId);
        Task<PostComment> GetPostCommentByPostIdAndUserIdAsync(string postId, string userId);
        Task<PostComment> DeletePostCommentByPostIdAndUserIdAsync(string postId, string userId);
        Task<PostComment> DeletePostCommentImageAsync(string postId, string userId);
        Task<PostComment> DeletePostCommentImageAsync(string postCommentId);
        Task<IEnumerable<PostComment>> GetPostCommentsByPostIdAsync(string postId);
        Task<IEnumerable<PostComment>> GetPostCommentsByPostIdAndUserIdAsync(string postId, string userId);
        Task SaveChangesAsync();
    }
}

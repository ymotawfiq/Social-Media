

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostCommentsRepository
{
    public interface IPostCommentsRepository
    {
        Task<PostComments> AddPostCommentAsync(PostComments postComments);
        Task<PostComments> UpdatePostCommentAsync(PostComments postComments);
        Task<PostComments> DeletePostCommentByIdAsync(string postCommentId);
        Task<PostComments> GetPostCommentByIdAsync(string postCommentId);
        Task<PostComments> GetPostCommentByPostIdAndUserIdAsync(string postId, string userId);
        Task<PostComments> DeletePostCommentByPostIdAndUserIdAsync(string postId, string userId);
        Task<PostComments> DeletePostCommentImageAsync(string postId, string userId);
        Task<PostComments> DeletePostCommentImageAsync(string postCommentId);
        Task<IEnumerable<PostComments>> GetPostCommentsByPostIdAsync(string postId);
        Task SaveChangesAsync();
    }
}

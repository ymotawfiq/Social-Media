

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.PostCommentsRepository
{
    public interface IPostCommentsRepository : ICrud<PostComment>
    {
        Task<PostComment> GetPostCommentByPostIdAndUserIdAsync(string postId, string userId);
        Task<PostComment> DeletePostCommentByPostIdAndUserIdAsync(string postId, string userId);
        Task<PostComment> DeletePostCommentImageAsync(string postId, string userId);
        Task<PostComment> DeletePostCommentImageAsync(string postCommentId);
        Task<IEnumerable<PostComment>> GetPostCommentsByPostIdAsync(string postId);
        Task<IEnumerable<PostComment>> GetPostCommentsByPostIdAndUserIdAsync(string postId, string userId);
    }
}

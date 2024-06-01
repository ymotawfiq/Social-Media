

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.PostCommentService
{
    public interface IPostCommentService
    {
        Task<ApiResponse<PostComment>> AddPostCommentAsync(AddPostCommentDto addPostCommentDto,
            SiteUser user);
        Task<ApiResponse<PostComment>> UpdatePostCommentAsync(UpdatePostCommentDto updatePostCommentDto,
            SiteUser user);
        Task<ApiResponse<PostComment>> DeletePostCommentByIdAsync(string postCommentId, SiteUser user);
        Task<ApiResponse<PostComment>> GetPostCommentByIdAsync(string postCommentId, SiteUser user);
        Task<ApiResponse<PostComment>> GetPostCommentByIdAsync(string postCommentId);
        Task<ApiResponse<PostComment>> GetPostCommentByPostIdAndUserIdAsync(string postId, SiteUser user);
        Task<ApiResponse<PostComment>> DeletePostCommentByPostIdAndUserIdAsync(string postId, SiteUser user);
        Task<ApiResponse<PostComment>> DeletePostCommentImageAsync(string postId, SiteUser user);
        Task<ApiResponse<PostComment>> DeletePostCommentImageAsync(string postCommentId);
        Task<ApiResponse<IEnumerable<PostComment>>> GetPostCommentsByPostIdAsync(string postId);
        Task<ApiResponse<IEnumerable<PostComment>>> GetPostCommentsByPostIdAndUserIdAsync(
            string postId, string userId);
        Task<ApiResponse<IEnumerable<PostComment>>> GetPostCommentsByPostIdAsync(string postId, 
            SiteUser user);
    }
}

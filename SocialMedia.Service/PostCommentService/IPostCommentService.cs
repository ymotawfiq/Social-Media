

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.PostCommentService
{
    public interface IPostCommentService
    {
        Task<ApiResponse<PostComments>> AddPostCommentAsync(AddPostCommentDto addPostCommentDto,
            SiteUser user);
        Task<ApiResponse<PostComments>> UpdatePostCommentAsync(UpdatePostCommentDto updatePostCommentDto,
            SiteUser user);
        Task<ApiResponse<PostComments>> DeletePostCommentByIdAsync(string postCommentId, SiteUser user);
        Task<ApiResponse<PostComments>> GetPostCommentByIdAsync(string postCommentId, SiteUser user);
        Task<ApiResponse<PostComments>> GetPostCommentByIdAsync(string postCommentId);
        Task<ApiResponse<PostComments>> GetPostCommentByPostIdAndUserIdAsync(string postId, SiteUser user);
        Task<ApiResponse<PostComments>> DeletePostCommentByPostIdAndUserIdAsync(string postId, SiteUser user);
        Task<ApiResponse<PostComments>> DeletePostCommentImageAsync(string postId, SiteUser user);
        Task<ApiResponse<PostComments>> DeletePostCommentImageAsync(string postCommentId);
        Task<ApiResponse<IEnumerable<PostComments>>> GetPostCommentsByPostIdAsync(string postId);
        Task<ApiResponse<IEnumerable<PostComments>>> GetPostCommentsByPostIdAsync(string postId, 
            SiteUser user);
    }
}

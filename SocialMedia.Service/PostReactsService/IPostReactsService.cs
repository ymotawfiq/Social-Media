

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.PostReactsService
{
    public interface IPostReactsService
    {
        Task<ApiResponse<PostReacts>> AddPostReactAsync(AddPostReactDto addPostReactDto, SiteUser user);
        Task<ApiResponse<PostReacts>> UpdatePostReactAsync(UpdatePostReactDto updatePostReactDto,
            SiteUser user);
        Task<ApiResponse<PostReacts>> GetPostReactByIdAsync(SiteUser user, string Id);
        Task<ApiResponse<PostReacts>> GetPostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<ApiResponse<PostReacts>> DeletePostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<ApiResponse<PostReacts>> DeletePostReactByIdAsync(string Id);
        Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByPostIdAsync(string postId, SiteUser user);
        Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByPostIdAsync(string postId);
    }
}

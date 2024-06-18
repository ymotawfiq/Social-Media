

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.PostReactsService
{
    public interface IPostReactsService
    {
        Task<ApiResponse<PostReacts>> AddPostReactAsync(AddPostReactDto addPostReactDto, SiteUser user);
        Task<ApiResponse<PostReacts>> UpdatePostReactAsync(UpdatePostReactDto updatePostReactDto,
            SiteUser user);
        Task<ApiResponse<PostReacts>> GetPostReactByIdAsync(SiteUser user, string Id);
        Task<ApiResponse<PostReacts>> GetPostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<ApiResponse<PostReacts>> DeletePostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<ApiResponse<PostReacts>> DeletePostReactByIdAsync(string Id, SiteUser user);
        Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByPostIdAsync(string postId, SiteUser user);
        Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByPostIdAsync(string postId);
        Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByUserIdAsync(string userId);
    }
}



using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.PostService
{
    public interface IPostService
    {
        Task<ApiResponse<PostResponseObject>> AddPostAsync(SiteUser user, AddPostDto createPostDto);
        Task<ApiResponse<PostResponseObject>> GetPostByIdAsync(SiteUser user, string postId);
        Task<ApiResponse<bool>> DeletePostAsync(SiteUser user, string postId);
        Task<ApiResponse<PostResponseObject>> UpdatePostAsync(SiteUser user, UpdatePostDto updatePostDto);
        Task<ApiResponse<bool>> UpdateUserPostsPolicyToLockedProfileAsync(SiteUser user);
        Task<ApiResponse<bool>> UpdateUserPostsPolicyToUnLockedProfileAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PostResponseObject>>> GetUserPostsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PostResponseObject>>> GetUserPostsAsync(SiteUser user, SiteUser routeUser);
        Task<ApiResponse<IEnumerable<PostResponseObject>>> GetUserPostsByPolicyAsync(SiteUser user, Policy policy);
        Task<ApiResponse<IEnumerable<PostResponseObject>>> GetPostsForFriendsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PostResponseObject>>>CheckFriendShipAndGetPostsAsync(
            SiteUser currentUser, SiteUser RouteUser);
        Task<ApiResponse<IEnumerable<PostResponseObject>>> GetPostsForFriendsOfFriendsAsync(SiteUser user);
        Task<ApiResponse<bool>> UpdatePostPolicyAsync(SiteUser user, UpdatePostPolicyDto updatePostPolicyDto);
        Task<ApiResponse<bool>> UpdatePostReactPolicyAsync(SiteUser user,
            UpdatePostReactPolicyDto updatePostReactPolicy);
        Task<ApiResponse<bool>> UpdatePostCommentPolicyAsync(SiteUser user,
            UpdatePostCommentPolicyDto updatePostCommentPolicyDto);


    }
}

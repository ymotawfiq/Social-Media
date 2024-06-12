

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.PostService
{
    public interface IPostService
    {
        Task<ApiResponse<PostDto>> AddPostAsync(SiteUser user, AddPostDto createPostDto);
        Task<ApiResponse<PostDto>> GetPostByIdAsync(SiteUser user, string postId);
        Task<ApiResponse<bool>> DeletePostAsync(SiteUser user, string postId);
        Task<ApiResponse<PostDto>> UpdatePostAsync(SiteUser user, UpdatePostDto updatePostDto);
        Task<ApiResponse<bool>> UpdateUserPostsPolicyToLockedProfileAsync(SiteUser user);
        Task<ApiResponse<bool>> UpdateUserPostsPolicyToUnLockedProfileAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsAsync(SiteUser user, SiteUser routeUser);
        Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsByPolicyAsync(SiteUser user, PostsPolicy policy);
        Task<ApiResponse<IEnumerable<PostDto>>> GetPostsForFriendsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PostDto>>>CheckFriendShipAndGetPostsAsync(
            SiteUser currentUser, SiteUser RouteUser);
        Task<ApiResponse<IEnumerable<PostDto>>> GetPostsForFriendsOfFriendsAsync(SiteUser user);
        Task<ApiResponse<bool>> UpdatePostPolicyAsync(SiteUser user, UpdatePostPolicyDto updatePostPolicyDto);
        Task<ApiResponse<bool>> UpdatePostReactPolicyAsync
            (SiteUser user, UpdatePostReactPolicyDto updatePostReactPolicy);
        Task<ApiResponse<bool>> UpdatePostCommentPolicyAsync(SiteUser user,
            UpdatePostCommentPolicyDto updatePostCommentPolicyDto);


    }
}

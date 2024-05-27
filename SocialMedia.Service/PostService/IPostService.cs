

using Microsoft.AspNetCore.Http;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.PostService
{
    public interface IPostService
    {
        Task<ApiResponse<PostDto>> AddPostAsync(SiteUser user, CreatePostDto createPostDto);
        Task<ApiResponse<PostDto>> GetPostByIdAsync(SiteUser user, string postId);
        Task<ApiResponse<PostDto>> DeletePostAsync(SiteUser user, string postId);
        Task<ApiResponse<PostDto>> UpdatePostAsync(SiteUser user, UpdatePostDto updatePostDto);
        Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsByPolicyAsync(SiteUser user, Policy policy);
        Task<ApiResponse<IEnumerable<List<PostDto>>>> GetPostsForFriendsAsync(SiteUser user);

        Task<ApiResponse<IEnumerable<List<PostDto>>>>
            CheckFriendShipAndGetPostsAsync(SiteUser currentUser, SiteUser RouteUser);
        Task<ApiResponse<IEnumerable<List<PostDto>>>> GetPostsForFriendsOfFriendsAsync(SiteUser user);
        Task<ApiResponse<PostDto>> GetPostByIdAsync(SiteUser currentUser, SiteUser routeUser, string postId);
        Task<ApiResponse<bool>> UpdatePostPolicyAsync(SiteUser user, UpdatePostPolicyDto updatePostPolicyDto);
        Task<ApiResponse<bool>> UpdatePostReactPolicyAsync
            (SiteUser user, UpdatePostReactPolicyDto updatePostReactPolicy);
        Task<ApiResponse<bool>> UpdatePostCommentPolicyAsync
            (SiteUser user, UpdatePostCommentPolicyDto updatePostCommentPolicyDto);

        Task<ApiResponse<bool>> MakePostsFriendsOnlyAsync(SiteUser user);

    }
}

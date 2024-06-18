

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.FollowerService
{
    public interface IFollowerService
    {
        Task<ApiResponse<Follower>> FollowAsync(FollowDto followDto, SiteUser user);
        Task<ApiResponse<Follower>> FollowAsync(SiteUser user, SiteUser follower);
        Task<ApiResponse<Follower>> UnfollowAsync(UnFollowDto followDto, SiteUser follower);
        Task<ApiResponse<Follower>> UnfollowAsync(string followId, string followerId);
        Task<ApiResponse<IEnumerable<Follower>>> GetAllFollowers(string userIdOrNameOrEmail);
        Task<ApiResponse<IEnumerable<Follower>>> GetAllFollowers(string userIdOrNameOrEmail, SiteUser user);
    }
}

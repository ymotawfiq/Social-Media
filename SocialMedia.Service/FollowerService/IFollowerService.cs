

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.FollowerService
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

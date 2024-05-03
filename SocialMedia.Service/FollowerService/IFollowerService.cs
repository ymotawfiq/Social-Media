

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.FollowerService
{
    public interface IFollowerService
    {
        Task<ApiResponse<Follower>> FollowAsync(FollowerDto followersDto);
        Task<ApiResponse<Follower>> UnfollowAsync(string userId, string followerId);
        Task<ApiResponse<IEnumerable<Follower>>> GetAllFollowers(string userId);
    }
}

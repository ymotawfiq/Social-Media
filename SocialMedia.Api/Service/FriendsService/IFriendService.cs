

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.FriendsService
{
    public interface IFriendService
    {
        Task<ApiResponse<Friend>> AddFriendAsync(AddFriendDto addFriendDto);
        Task<ApiResponse<Friend>> DeleteFriendAsync(string userId, string friendId);
        Task<ApiResponse<Friend>> DeleteFriendAsync(string id, SiteUser user);
        Task<ApiResponse<bool>> IsUserFriendAsync(string userId, string friendId);
        Task<ApiResponse<bool>> IsUserFriendOfFriendAsync(string userId, string friendId);
        Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(SiteUser user, SiteUser user1);
        Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<Friend>>> GetSharedFriendsAsync(SiteUser user, SiteUser routeUser);
    }
}

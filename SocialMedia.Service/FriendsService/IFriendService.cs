

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.FriendsService
{
    public interface IFriendService
    {
        Task<ApiResponse<Friend>> AddFriendAsync(AddFriendDto addFriendDto);
        Task<ApiResponse<Friend>> DeleteFriendAsync(string userId, string friendId);
        Task<ApiResponse<bool>> IsUserFriendAsync(string userId, string friendId);
        Task<ApiResponse<bool>> IsUserFriendOfFriendAsync(string userId, string friendId);
        Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(SiteUser user, SiteUser user1);
        Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<Friend>>> GetSharedFriendsAsync(SiteUser user, SiteUser routeUser);
    }
}

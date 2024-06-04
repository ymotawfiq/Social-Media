

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.FriendRequestService
{
    public interface IFriendRequestService
    {
        Task<ApiResponse<FriendRequest>> AddFriendRequestAsync(
            AddFriendRequestDto addFriendRequestDto, SiteUser user);
        Task<ApiResponse<FriendRequest>> UpdateFriendRequestAsync(
            UpdateFriendRequestDto updateFriendRequestDto);
        Task<ApiResponse<FriendRequest>> GetFriendRequestByIdAsync(string friendRequestId, SiteUser user);
        Task<ApiResponse<FriendRequest>> DeleteFriendRequestByAsync(SiteUser user, string friendRequestId);
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsAsync();
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsByUserIdAsync(string userId);
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsByUserNameAsync(SiteUser user);
    }
}

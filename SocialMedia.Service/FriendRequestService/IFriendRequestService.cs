

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
            UpdateFriendRequestDto updateFriendRequestDto, SiteUser user);
        Task<ApiResponse<FriendRequest>> GetFriendRequestByIdAsync(string friendRequestId, SiteUser user);
        Task<ApiResponse<FriendRequest>> DeleteFriendRequestAsync(SiteUser user, string friendRequestId);
        Task<ApiResponse<FriendRequest>> DeleteFriendRequestAsync(SiteUser sender, SiteUser receiver);
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsAsync();
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetSentFriendRequestsByUserIdAsync(string userId);
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetReceivedFriendRequestsByUserIdAsync(string userId);
    }
}

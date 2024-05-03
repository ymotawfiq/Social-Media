

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.FriendRequestService
{
    public interface IFriendRequestService
    {
        Task<ApiResponse<FriendRequest>> AddFriendRequestAsync(FriendRequestDto friendRequestDto);
        Task<ApiResponse<FriendRequest>> UpdateFriendRequestAsync(FriendRequestDto friendRequestDto);
        Task<ApiResponse<FriendRequest>> GetFriendRequestByIdAsync(Guid friendRequestId);
        Task<ApiResponse<FriendRequest>> DeleteFriendRequestByAsync(SiteUser user, Guid friendRequestId);
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsAsync();
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsByUserIdAsync(string userId);
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsByUserNameAsync(string userName);
    }
}

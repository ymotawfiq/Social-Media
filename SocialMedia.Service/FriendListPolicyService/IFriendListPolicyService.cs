

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.FriendListPolicyService
{
    public interface IFriendListPolicyService
    {
        Task<ApiResponse<FriendListPolicy>> AddFriendListPolicyAsync(
            AddFriendListPolicyDto addFriendListPolicyDto);
        Task<ApiResponse<FriendListPolicy>> UpdateFriendListPolicyAsync(
            UpdateFriendListPolicyDto updateFriendListPolicyDto);
        Task<ApiResponse<FriendListPolicy>> GetFriendListPolicyAsync(
            string friendListPolicyIdOrPolicyName);
        Task<ApiResponse<IEnumerable<FriendListPolicy>>> GetFriendListPoliciesAsync();
        Task<ApiResponse<FriendListPolicy>> DeleteFriendListPolicyAsync(
            string friendListPolicyIdOrPolicyName);
    }
}

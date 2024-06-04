


using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendListPolicyRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.FriendListPolicyService
{
    public class FriendListPolicyService : IFriendListPolicyService
    {
        private readonly IFriendListPolicyRepository _friendListPolicyRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IPolicyService _policyService;
        public FriendListPolicyService(IFriendListPolicyRepository _friendListPolicyRepository,
            UserManagerReturn _userManagerReturn, IPolicyService _policyService)
        {
            this._friendListPolicyRepository = _friendListPolicyRepository;
            this._userManagerReturn = _userManagerReturn;
            this._policyService = _policyService;
        }
        public async Task<ApiResponse<FriendListPolicy>> AddFriendListPolicyAsync(
            AddFriendListPolicyDto addFriendListPolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(
                addFriendListPolicyDto.PolicyIdOrName);
            if (policy != null && policy.ResponseObject!=null)
            {
                var existFriendListPolicy = await _friendListPolicyRepository
                    .GetFriendListPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (existFriendListPolicy == null)
                {
                    addFriendListPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                    var newFriendListPolicy = await _friendListPolicyRepository.AddFriendListPolicyAsync(
                        ConvertFromDto.ConvertFriendListPolicyDto_Add(addFriendListPolicyDto));
                    return StatusCodeReturn<FriendListPolicy>
                        ._201_Created("Friend list policy added successfully", newFriendListPolicy);
                }
                return StatusCodeReturn<FriendListPolicy>
                    ._403_Forbidden("Friend list policy already exists");
            }
            return StatusCodeReturn<FriendListPolicy>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<FriendListPolicy>> GetFriendListPolicyAsync
            (string friendListPolicyIdOrPolicyName)
        {
            var policy = await _policyService.GetPolicyByNameAsync(friendListPolicyIdOrPolicyName);
            FriendListPolicy friendListPolicy = null!;
            if (policy != null && policy.ResponseObject != null)
            {
                friendListPolicy = await _friendListPolicyRepository
                .GetFriendListPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (friendListPolicy != null)
                {
                    return StatusCodeReturn<FriendListPolicy>._200_Success(
                        "Friend list policy found successfully", friendListPolicy);
                }
                return StatusCodeReturn<FriendListPolicy>._404_NotFound("Friend list policy not found");
            }
            friendListPolicy = await _friendListPolicyRepository
                .GetFriendListPolicyByIdAsync(friendListPolicyIdOrPolicyName);
            if (friendListPolicy != null)
            {
                return StatusCodeReturn<FriendListPolicy>._200_Success(
                        "Friend list policy found successfully", friendListPolicy);
            }
            Guid _;
            bool isValid = Guid.TryParse(friendListPolicyIdOrPolicyName, out _);
            if (isValid)
            {
                return StatusCodeReturn<FriendListPolicy>._404_NotFound(
                        "Friend list policy not found");
            }
            return StatusCodeReturn<FriendListPolicy>._404_NotFound(
                        "Policy not found");
        }

        public async Task<ApiResponse<FriendListPolicy>> UpdateFriendListPolicyAsync(
            UpdateFriendListPolicyDto updateFriendListPolicyDto)
        {
            var friendListPolicy = await _friendListPolicyRepository.GetFriendListPolicyByIdAsync(
                updateFriendListPolicyDto.Id);
            if (friendListPolicy != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                updateFriendListPolicyDto.PolicyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var existFriendListPolicy = await _friendListPolicyRepository
                        .GetFriendListPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                    if (existFriendListPolicy == null)
                    {
                        var updatedFriendListPolicy = await _friendListPolicyRepository
                        .UpdateFriendListPolicyAsync(
                            ConvertFromDto.ConvertFriendListPolicyDto_Update(updateFriendListPolicyDto));
                        return StatusCodeReturn<FriendListPolicy>
                            ._200_Success("Friend list policy updated successfully", updatedFriendListPolicy);
                    }
                    return StatusCodeReturn<FriendListPolicy>
                        ._403_Forbidden("Friend list policy already exists");
                }
                return StatusCodeReturn<FriendListPolicy>
                        ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<FriendListPolicy>
                        ._404_NotFound("Friend list policy not found");
        }


        public async Task<ApiResponse<FriendListPolicy>> DeleteFriendListPolicyAsync
                (string friendListPolicyIdOrPolicyName)
        {
            var policy = await _policyService.GetPolicyByNameAsync(friendListPolicyIdOrPolicyName);
            FriendListPolicy friendListPolicy = null!;
            if (policy != null && policy.ResponseObject != null)
            {
                friendListPolicy = await _friendListPolicyRepository
                .GetFriendListPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (friendListPolicy != null)
                {
                    await _friendListPolicyRepository.DeleteFriendListPolicyByIdAsync(friendListPolicy.Id);
                    return StatusCodeReturn<FriendListPolicy>._200_Success(
                        "Friend list policy deleted successfully", friendListPolicy);
                }
                return StatusCodeReturn<FriendListPolicy>._404_NotFound("Friend list policy not found");
            }
            friendListPolicy = await _friendListPolicyRepository
                .GetFriendListPolicyByIdAsync(friendListPolicyIdOrPolicyName);
            if (friendListPolicy != null)
            {
                await _friendListPolicyRepository.DeleteFriendListPolicyByIdAsync(friendListPolicy.Id);
                return StatusCodeReturn<FriendListPolicy>._200_Success(
                        "Friend list policy deleted successfully", friendListPolicy);
            }
            Guid _;
            bool isValid = Guid.TryParse(friendListPolicyIdOrPolicyName, out _);
            if (isValid)
            {
                return StatusCodeReturn<FriendListPolicy>._404_NotFound(
                        "Friend list policy not found");
            }
            return StatusCodeReturn<FriendListPolicy>._404_NotFound(
                        "Policy not found");
        }

        public async Task<ApiResponse<IEnumerable<FriendListPolicy>>> GetFriendListPoliciesAsync()
        {
            var friendListPolicies = await _friendListPolicyRepository.GetFriendListPoliciesAsync();
            if (friendListPolicies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<FriendListPolicy>>
                    ._200_Success("No friend list policies found", friendListPolicies);
            }
            return StatusCodeReturn<IEnumerable<FriendListPolicy>>
                    ._200_Success("Friend list policies found successfully", friendListPolicies);
        }
    }
}



using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<SiteUser> _userManager;
        private readonly IPolicyService _policyService;
        public FriendListPolicyService(IFriendListPolicyRepository _friendListPolicyRepository,
            UserManager<SiteUser> _userManager, IPolicyService _policyService)
        {
            this._friendListPolicyRepository = _friendListPolicyRepository;
            this._userManager = _userManager;
            this._policyService = _policyService;
        }
        public async Task<ApiResponse<FriendListPolicy>> AddFriendListPolicyAsync(
            AddFriendListPolicyDto addFriendListPolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(
                addFriendListPolicyDto.PolicyIdOrName);
            if (policy != null)
            {
                var user = await new UserManagerReturn().GetUserByUserNameOrEmailOrIdAsync(
                    addFriendListPolicyDto.UserIdOrNameOrEmail);
                if (user != null)
                {
                    var newFriendListPolicy = await _friendListPolicyRepository.AddFriendListPolicyAsync(
                        ConvertFromDto.ConvertFriendListPolicyDto_Add(addFriendListPolicyDto));
                    return StatusCodeReturn<FriendListPolicy>
                        ._201_Created("Friend list policy added successfully", newFriendListPolicy);
                }
                return StatusCodeReturn<FriendListPolicy>
                    ._404_NotFound("User not found");
            }
            return StatusCodeReturn<FriendListPolicy>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<FriendListPolicy>> UpdateFriendListPolicyAsync(
            UpdateFriendListPolicyDto updateFriendListPolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(
                updateFriendListPolicyDto.PolicyIdOrName);
            if (policy.ResponseObject != null)
            {
                var user = await new UserManagerReturn().GetUserByUserNameOrEmailOrIdAsync(
                    updateFriendListPolicyDto.UserIdOrUserIdOrNameOrEmail); ;
                if (user != null)
                {
                    var friendListPolicy = await _friendListPolicyRepository.GetFriendListPolicyByUserIdAsync
                    (user.Id);
                    if (friendListPolicy != null)
                    {
                        friendListPolicy.PolicyId = policy.ResponseObject.Id;
                        updateFriendListPolicyDto.UserIdOrUserIdOrNameOrEmail = user.Id;
                        var updatedFriendListPolicy = await _friendListPolicyRepository.UpdateFriendListPolicyAsync(
                            ConvertFromDto.ConvertFriendListPolicyDto_Update(updateFriendListPolicyDto));
                        return StatusCodeReturn<FriendListPolicy>
                            ._200_Success("Friend list policy updated successfully", updatedFriendListPolicy);
                    }
                    return StatusCodeReturn<FriendListPolicy>
                        ._404_NotFound("Friend list policy not found");
                }
                return StatusCodeReturn<FriendListPolicy>
                        ._404_NotFound("User policy not found");
            }
            return StatusCodeReturn<FriendListPolicy>
                        ._404_NotFound("Policy not found");
        }

        


    }
}

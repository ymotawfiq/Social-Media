

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendListPolicyRepository;
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
                var user = await GetUserAsync(addFriendListPolicyDto.UserIdOrNameOrEmail);
                if (user != null)
                {
                    var newFriendListPolicy = await _friendListPolicyRepository.AddFriendListPolicyAsync(
                        ConvertFromDto.ConvertFriendListPolicyDto_Add(addFriendListPolicyDto));
                    return new ApiResponse<FriendListPolicy>
                    {
                        IsSuccess = true,
                        Message = "Friend list policy added successfully",
                        StatusCode = 200,
                        ResponseObject = newFriendListPolicy
                    };
                }
                return new ApiResponse<FriendListPolicy>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404,
                };
            }
            return new ApiResponse<FriendListPolicy>
            {
                IsSuccess = false,
                Message = "Policy not found",
                StatusCode = 404,
            };
        }

        public async Task<ApiResponse<FriendListPolicy>> UpdateFriendListPolicyAsync(
            UpdateFriendListPolicyDto updateFriendListPolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(
                updateFriendListPolicyDto.PolicyIdOrName);
            if (policy.ResponseObject != null)
            {
                var user = await GetUserAsync(updateFriendListPolicyDto.UserIdOrUserIdOrNameOrEmail);
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
                        return new ApiResponse<FriendListPolicy>
                        {
                            IsSuccess = true,
                            Message = "Friend list policy updated successfully",
                            StatusCode = 200,
                            ResponseObject = updatedFriendListPolicy
                        };
                    }
                    return new ApiResponse<FriendListPolicy>
                    {
                        IsSuccess = false,
                        Message = "Friend list policy not found",
                        StatusCode = 404,
                    };
                }
                return new ApiResponse<FriendListPolicy>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404,
                };
            }
            return new ApiResponse<FriendListPolicy>
            {
                IsSuccess = false,
                Message = "Policy not found",
                StatusCode = 404,
            };
        }


        private async Task<SiteUser> GetUserAsync(string userIdOrNameOrEmail)
        {
            var userByEmail = await _userManager.FindByEmailAsync(userIdOrNameOrEmail);
            var userById = await _userManager.FindByIdAsync(userIdOrNameOrEmail);
            var userByName = await _userManager.FindByNameAsync(userIdOrNameOrEmail);
            if (userByEmail != null)
            {
                return userByEmail;
            }
            return userById != null ? userById : userByName!;
        }
        


    }
}

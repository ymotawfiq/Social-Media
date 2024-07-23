using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Api.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;

namespace SocialMedia.Api.Service.AccountService.SettingsService
{
    public class SettingsService : ISettingsService
    {
        private readonly Policies policies = new();
        private readonly IPolicyService _policyService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IPolicyRepository _policyRepository;
        private readonly IPostRepository _postRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public SettingsService(IPolicyService _policyService, UserManager<SiteUser> _userManager, 
        IPolicyRepository _policyRepository, IPostRepository _postRepository, 
        UserManagerReturn _userManagerReturn)
        {
            this._policyRepository = _policyRepository;
            this._postRepository = _postRepository;
            this._policyService = _policyService;
            this._userManager = _userManager;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<bool>> UpdateAccountCommentPolicyAsync(SiteUser user, 
            string policyIdOrName)
        {
            var commentPolicy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (commentPolicy != null && commentPolicy.ResponseObject != null)
            {
                if (policies.CommentPolicies.Contains(commentPolicy.ResponseObject.PolicyType))
                {
                    user.CommentPolicyId = commentPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(user);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accont comment policy updated successfully", true);
                }
                return StatusCodeReturn<bool>
                    ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<bool>> UpdateAccountReactPolicyAsync(SiteUser user,
            string policyIdOrName)
        {
            var reactPolicy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (reactPolicy != null && reactPolicy.ResponseObject != null)
            {
                if (policies.ReactPolicies.Contains(reactPolicy.ResponseObject.PolicyType))
                {
                    user.ReactPolicyId = reactPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(user);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accont react policy updated successfully", true);
                }
                return StatusCodeReturn<bool>
                    ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<bool>> UpdateAccountPostsPolicyAsync(SiteUser user,
            string policyIdOrName)
        {
            var accountPostsPolicy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (accountPostsPolicy != null && accountPostsPolicy.ResponseObject != null)
            {
                if (policies.AccountPostPolicies.Contains(accountPostsPolicy.ResponseObject.PolicyType))
                {
                    user.AccountPostPolicyId = accountPostsPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(user);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accont posts policy updated successfully", true);
                }
                return StatusCodeReturn<bool>
                     ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<bool>> UpdateAccountFriendListPolicyAsync(SiteUser user,
            string policyIdOrName)
        {
            var accountFriendListPolicy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (accountFriendListPolicy != null && accountFriendListPolicy.ResponseObject != null)
            {
                if (policies.FriendListPolicies.Contains(accountFriendListPolicy.ResponseObject.PolicyType))
                {
                    user.FriendListPolicyId = accountFriendListPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(user);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accont friend list policy updated successfully", true);
                }
                return StatusCodeReturn<bool>
                     ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Policy not found");
        }   
                public async Task<ApiResponse<bool>> LockProfileAsync(SiteUser user)
        {
            var privatePolicy = await _policyRepository.GetPolicyByNameAsync("private");
            var friendsPolicy = await _policyRepository.GetPolicyByNameAsync("friends only");
            if (privatePolicy != null)
            {
                if(user.AccountPolicyId != privatePolicy.Id)
                {
                    if (friendsPolicy != null)
                    {
                        user.ReactPolicyId = user.CommentPolicyId = user.AccountPostPolicyId
                            = user.FriendListPolicyId = friendsPolicy.Id;
                        user.AccountPolicyId = privatePolicy.Id;
                        await _userManager.UpdateAsync(user);
                        await _postRepository.UpdateUserPostsPolicyToLockedAccountAsync(user.Id);
                        return StatusCodeReturn<bool>
                            ._200_Success("Profile locked successfully", true);
                    }
                    return StatusCodeReturn<bool>
                            ._404_NotFound("Friends only policy not found", false);
                }
                return StatusCodeReturn<bool>
                        ._403_Forbidden("Account already locked");
            }
            return StatusCodeReturn<bool>
                        ._404_NotFound("Private policy not found", false);
        }

        public async Task<ApiResponse<bool>> UnLockProfileAsync(SiteUser user)
        {
            var publicPolicy = await _policyRepository.GetPolicyByNameAsync("public");
            if (publicPolicy != null)
            {
                if(user.AccountPolicyId != publicPolicy.Id)
                {
                    user.ReactPolicyId = user.CommentPolicyId = user.AccountPostPolicyId
                    = user.FriendListPolicyId = user.AccountPolicyId = publicPolicy.Id;
                    await _userManager.UpdateAsync(user);
                    await _postRepository.UpdateUserPostsPolicyToUnLockedAccountAsync(user.Id);
                    return StatusCodeReturn<bool>
                        ._200_Success("Profile unlocked successfully", true);
                }
                return StatusCodeReturn<bool>
                        ._403_Forbidden("Account already public");
            }
            return StatusCodeReturn<bool>
                        ._404_NotFound("Public policy not found", false);
        }
        
        public async Task<ApiResponse<string>> DeleteAccountAsync(string userNameOrEmail)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userNameOrEmail);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "Account not found",
                    StatusCode = 404
                };
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "Failed to delete Account",
                    StatusCode = 400
                };
            }
            return new ApiResponse<string>
            {
                StatusCode = 200,
                Message = "Account deleted successfully",
                IsSuccess = true
            };
        }
        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return StatusCodeReturn<string>
                    ._404_NotFound("User not found");
            }
            var result = await _userManager.ResetPasswordAsync(user,
                resetPasswordDto.Token, resetPasswordDto.Password);
            if (result.Succeeded)
            {
                return StatusCodeReturn<string>
                    ._200_Success("Password reset successfully");
            }
            return StatusCodeReturn<string>
                ._400_BadRequest("Failed to reset password");
        }

    }
}
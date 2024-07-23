using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Api.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.AccountService.SettingsService
{
    public interface ISettingsService
    {
        Task<ApiResponse<bool>> UpdateAccountCommentPolicyAsync(SiteUser user, string policyIdOrName);
        Task<ApiResponse<bool>> UpdateAccountReactPolicyAsync(SiteUser user, string policyIdOrName);
        Task<ApiResponse<bool>> UpdateAccountFriendListPolicyAsync(SiteUser user, string policyIdOrName);
        Task<ApiResponse<bool>> LockProfileAsync(SiteUser user);
        Task<ApiResponse<bool>> UnLockProfileAsync(SiteUser user);
        Task<ApiResponse<bool>> UpdateAccountPostsPolicyAsync(SiteUser user, string policyIdOrName);
        Task<ApiResponse<string>> DeleteAccountAsync(string userNameOrEmail);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

    }
}
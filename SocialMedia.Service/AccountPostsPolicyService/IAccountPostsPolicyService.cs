

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.AccountPostsPolicyService
{
    public interface IAccountPostsPolicyService
    {
        Task<ApiResponse<AccountPostsPolicy>> AddAccountPostPolicyAsync(
            AddAccountPostsPolicyDto addAccountPostsPolicyDto);
        Task<ApiResponse<AccountPostsPolicy>> UpdateAccountPostPolicyAsync(
            UpdateAccountPostsPolicyDto updateAccountPostsPolicyDto);
        Task<ApiResponse<AccountPostsPolicy>> DeleteAccountPostPolicyByIdAsync(string postPolicyId);
        Task<ApiResponse<AccountPostsPolicy>> DeleteAccountPostPolicyAsync(
            string postPolicyIdOrPolicyIdOrPolicyName);
        Task<ApiResponse<AccountPostsPolicy>> DeleteAccountPostPolicyByPolicyIdAsync(string policyId);
        Task<ApiResponse<AccountPostsPolicy>> GetAccountPostPolicyByIdAsync(string postPolicyId);
        Task<ApiResponse<AccountPostsPolicy>> GetAccountPostPolicyByPolicyIdAsync(string policyId);
        Task<ApiResponse<AccountPostsPolicy>> GetAccountPostPolicyAsync(
            string postPolicyIdOrPolicyIdOrPolicyName);
        Task<ApiResponse<IEnumerable<AccountPostsPolicy>>> GetAccountPostPoliciesAsync();
    }
}

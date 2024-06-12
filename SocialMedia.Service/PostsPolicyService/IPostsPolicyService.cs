

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.PostsPolicyService
{
    public interface IPostsPolicyService
    {
        Task<ApiResponse<PostsPolicy>> AddAccountPostPolicyAsync(
            AddAccountPostsPolicyDto addAccountPostsPolicyDto);
        Task<ApiResponse<PostsPolicy>> UpdateAccountPostPolicyAsync(
            UpdateAccountPostsPolicyDto updateAccountPostsPolicyDto);
        Task<ApiResponse<PostsPolicy>> DeleteAccountPostPolicyByIdAsync(string postPolicyId);
        Task<ApiResponse<PostsPolicy>> DeleteAccountPostPolicyAsync(
            string postPolicyIdOrPolicyIdOrPolicyName);
        Task<ApiResponse<PostsPolicy>> DeleteAccountPostPolicyByPolicyIdAsync(string policyId);
        Task<ApiResponse<PostsPolicy>> GetAccountPostPolicyByIdAsync(string postPolicyId);
        Task<ApiResponse<PostsPolicy>> GetAccountPostPolicyByPolicyIdAsync(string policyId);
        Task<ApiResponse<PostsPolicy>> GetAccountPostPolicyAsync(
            string postPolicyIdOrPolicyIdOrPolicyName);
        Task<ApiResponse<IEnumerable<PostsPolicy>>> GetAccountPostPoliciesAsync();
    }
}

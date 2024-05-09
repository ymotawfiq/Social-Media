

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.AccountPolicyService
{
    public interface IAccountPolicyService
    {
        Task<ApiResponse<AccountPolicy>> AddAccountPolicyAsync(AddAccountPolicyDto addAccountPolicyDto);
        Task<ApiResponse<AccountPolicy>> UpdateAccountPolicyAsync(UpdateAccountPolicyDto updateAccountPolicyDto);
        Task<ApiResponse<AccountPolicy>> GetAccountPolicyByIdAsync(string accountPolicyId);
        Task<ApiResponse<AccountPolicy>> GetAccountPolicyByPolicyIdAsync(string policyId);
        Task<ApiResponse<AccountPolicy>> GetAccountPolicyByPolicyAsync(string policyIdOrName);
        Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByPolicyAsync(string policyIdOrName);
        Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByIdAsync(string accountPolicyId);
        Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByPolicyIdAsync(string policyId);
        Task<ApiResponse<IEnumerable<AccountPolicy>>> GetAccountPoliciesAsync();
    }
}

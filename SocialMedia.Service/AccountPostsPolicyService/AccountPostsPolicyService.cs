

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.AccountPostsPolicyRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;
using Twilio.Jwt.Taskrouter;

namespace SocialMedia.Service.AccountPostsPolicyService
{
    public class AccountPostsPolicyService : IAccountPostsPolicyService
    {
        private readonly IAccountPostsPolicyRepository _accountPostsPolicyRepository;
        private readonly IPolicyService _policyService;
        public AccountPostsPolicyService(IAccountPostsPolicyRepository _accountPostsPolicyRepository,
            IPolicyService _policyService)
        {
            this._accountPostsPolicyRepository = _accountPostsPolicyRepository;
            this._policyService = _policyService;
        }
        public async Task<ApiResponse<AccountPostsPolicy>> AddAccountPostPolicyAsync
            (AddAccountPostsPolicyDto addAccountPostsPolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync
                (addAccountPostsPolicyDto.PolicyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                var accountPostPolicy = await _accountPostsPolicyRepository
                    .GetAccountPostPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accountPostPolicy != null)
                {
                    return StatusCodeReturn<AccountPostsPolicy>
                        ._403_Forbidden("Account post policy already exists");
                }
                addAccountPostsPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                var newAccountPostsPolicy = await _accountPostsPolicyRepository.AddAccountPostPolicyAsync(
                    ConvertFromDto.ConvertAccountPostsPolicyDto_Add(addAccountPostsPolicyDto));
                return StatusCodeReturn<AccountPostsPolicy>
                    ._201_Created("Account posts policy created successfully", newAccountPostsPolicy);
            }
            return StatusCodeReturn<AccountPostsPolicy>
                ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<AccountPostsPolicy>> DeleteAccountPostPolicyAsync
            (string postPolicyIdOrPolicyIdOrPolicyName)
        {
            var accountPostsPolicy = await GetAccountPostsPolicyByIdOrPolicyAsync(
                postPolicyIdOrPolicyIdOrPolicyName);
            if (accountPostsPolicy != null)
            {
                await _accountPostsPolicyRepository.DeleteAccountPostPolicyByIdAsync(accountPostsPolicy.Id);
                return StatusCodeReturn<AccountPostsPolicy>
                    ._200_Success("Account post policy deleted successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<AccountPostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<AccountPostsPolicy>> DeleteAccountPostPolicyByIdAsync
            (string postPolicyId)
        {
            var accountPostsPolicy = await _accountPostsPolicyRepository.GetAccountPostPolicyByIdAsync(
                postPolicyId);
            if (accountPostsPolicy != null)
            {
                await _accountPostsPolicyRepository.DeleteAccountPostPolicyByIdAsync(accountPostsPolicy.Id);
                return StatusCodeReturn<AccountPostsPolicy>
                    ._200_Success("Account post policy deleted successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<AccountPostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<AccountPostsPolicy>> DeleteAccountPostPolicyByPolicyIdAsync
            (string policyId)
        {
            var accountPostsPolicy = await _accountPostsPolicyRepository.GetAccountPostPolicyByPolicyIdAsync(
                policyId);
            if (accountPostsPolicy != null)
            {
                await _accountPostsPolicyRepository.DeleteAccountPostPolicyByIdAsync(accountPostsPolicy.Id);
                return StatusCodeReturn<AccountPostsPolicy>
                    ._200_Success("Account post policy deleted successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<AccountPostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<IEnumerable<AccountPostsPolicy>>> GetAccountPostPoliciesAsync()
        {
            var accountPostsPolicies = await _accountPostsPolicyRepository.GetAccountPostPoliciesAsync();
            if (accountPostsPolicies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<AccountPostsPolicy>>
                    ._200_Success("No account posts policy found", accountPostsPolicies);
            }
            return StatusCodeReturn<IEnumerable<AccountPostsPolicy>>
                    ._200_Success("Account posts policy found successfully", accountPostsPolicies);
        }

        public async Task<ApiResponse<AccountPostsPolicy>> GetAccountPostPolicyAsync
            (string postPolicyIdOrPolicyIdOrPolicyName)
        {
            var accountPostsPolicy = await GetAccountPostsPolicyByIdOrPolicyAsync(
                postPolicyIdOrPolicyIdOrPolicyName);
            if (accountPostsPolicy != null)
            {
                return StatusCodeReturn<AccountPostsPolicy>
                    ._200_Success("Account post policy found successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<AccountPostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<AccountPostsPolicy>> GetAccountPostPolicyByIdAsync(string postPolicyId)
        {
            var accountPostsPolicy = await _accountPostsPolicyRepository.GetAccountPostPolicyByIdAsync(
                postPolicyId);
            if (accountPostsPolicy != null)
            {
                return StatusCodeReturn<AccountPostsPolicy>
                    ._200_Success("Account post policy found successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<AccountPostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<AccountPostsPolicy>> GetAccountPostPolicyByPolicyIdAsync(string policyId)
        {
            var accountPostsPolicy = await _accountPostsPolicyRepository.GetAccountPostPolicyByPolicyIdAsync(
                policyId);
            if (accountPostsPolicy != null)
            {
                return StatusCodeReturn<AccountPostsPolicy>
                    ._200_Success("Account post policy found successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<AccountPostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<AccountPostsPolicy>> UpdateAccountPostPolicyAsync
            (UpdateAccountPostsPolicyDto updateAccountPostsPolicyDto)
        {
            var accountPostsPolicy = await _accountPostsPolicyRepository.GetAccountPostPolicyByIdAsync(
                updateAccountPostsPolicyDto.Id);
            var policy = await _policyService
                .GetPolicyByIdOrNameAsync(updateAccountPostsPolicyDto.PolicyIdOrName);
            if (accountPostsPolicy != null && policy != null && policy.ResponseObject != null)
            {
                var accountPostPolicy = await _accountPostsPolicyRepository
                    .GetAccountPostPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accountPostPolicy != null)
                {
                    return StatusCodeReturn<AccountPostsPolicy>
                        ._403_Forbidden("Account post policy already exists");
                }
                await _accountPostsPolicyRepository.UpdateAccountPostPolicyAsync(accountPostsPolicy);
                return StatusCodeReturn<AccountPostsPolicy>
                    ._200_Success("Account post policy updated successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<AccountPostsPolicy>
                    ._404_NotFound("Account post policy not found");
        }

        private async Task<AccountPostsPolicy> GetAccountPostsPolicyByIdOrPolicyAsync
            (string postPolicyIdOrPolicyIdOrPolicyName)
        {
            var accountPostsPolicyById = await _accountPostsPolicyRepository.GetAccountPostPolicyByIdAsync(
                postPolicyIdOrPolicyIdOrPolicyName);
            var accountPostsPolicyByPolicyId = await _accountPostsPolicyRepository
                .GetAccountPostPolicyByPolicyIdAsync(postPolicyIdOrPolicyIdOrPolicyName);
            if (accountPostsPolicyById != null)
            {
                return accountPostsPolicyById;
            }
            else if (accountPostsPolicyByPolicyId != null)
            {
                return accountPostsPolicyByPolicyId;
            }
            var policy = await _policyService.GetPolicyByIdOrNameAsync(postPolicyIdOrPolicyIdOrPolicyName);
            if (policy != null && policy.ResponseObject!=null)
            {
                accountPostsPolicyByPolicyId = await _accountPostsPolicyRepository
                    .GetAccountPostPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accountPostsPolicyByPolicyId != null)
                {
                    return accountPostsPolicyByPolicyId;
                }
            }
            return null!;
        }
    }
}

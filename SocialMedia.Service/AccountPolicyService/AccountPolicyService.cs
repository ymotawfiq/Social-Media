

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.AccountPolicyRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.AccountPolicyService
{
    public class AccountPolicyService : IAccountPolicyService
    {
        private readonly IAccountPolicyRepository _accountPolicyRepository;
        private readonly IPolicyService _policyService;
        public AccountPolicyService(IAccountPolicyRepository _accountPolicyRepository,
            IPolicyService _policyService)
        {
            this._accountPolicyRepository = _accountPolicyRepository;
            this._policyService = _policyService;
        }
        public async Task<ApiResponse<AccountPolicy>> 
            AddAccountPolicyAsync(AddAccountPolicyDto addAccountPolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(addAccountPolicyDto.PolicyIdOrName);
            if (policy!=null && policy.ResponseObject != null)
            {
                var accountPolicyByPolicyId = await _accountPolicyRepository
                        .GetAccountPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accountPolicyByPolicyId == null)
                {
                    addAccountPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                    var accountPolicy = await _accountPolicyRepository.AddAccountPolicyAsync(
                        ConvertFromDto.ConvertFromAccountPolicyDto_Add(addAccountPolicyDto));
                    return StatusCodeReturn<AccountPolicy>
                        ._201_Created("Account policy added successfully", accountPolicy);
                }
                return StatusCodeReturn<AccountPolicy>
                    ._403_Forbidden("Account policy already exists");
                
            }
            return StatusCodeReturn<AccountPolicy>
                        ._404_NotFound("Policy not found"); ;
        }

        public async Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByPolicyAsync(
            string policyIdOrName)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (policy!=null && policy.ResponseObject != null)
            {
                var accoountPolicy = await _accountPolicyRepository
                    .GetAccountPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accoountPolicy != null)
                {
                    await _accountPolicyRepository.DeleteAccountPolicyByIdAsync(accoountPolicy.Id);
                    return StatusCodeReturn<AccountPolicy>
                        ._200_Success("Account policy deleted successfully", accoountPolicy);
                }
                return StatusCodeReturn<AccountPolicy>
                    ._404_NotFound("Account policy not found");
            }
            return StatusCodeReturn<AccountPolicy>
                    ._404_NotFound("Policy not found");
            
        }

        public async Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByIdAsync(string accountPolicyId)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByIdAsync(
                    accountPolicyId);
            if (accountPolicy != null)
            {
                await _accountPolicyRepository.DeleteAccountPolicyByIdAsync(accountPolicyId);
                return StatusCodeReturn<AccountPolicy>
                    ._200_Success("Account policy deleted successfully", accountPolicy);
            }
            return StatusCodeReturn<AccountPolicy>
                        ._404_NotFound("Account policy not found");
        }

        public async Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByPolicyIdAsync(string policyId)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByPolicyIdAsync(policyId);
            if (accountPolicy != null)
            {
                await _accountPolicyRepository.DeleteAccountPolicyByPolicyIdAsync(policyId);
                return StatusCodeReturn<AccountPolicy>
                    ._200_Success("Account policy deleted successfully", accountPolicy);
            }
            return StatusCodeReturn<AccountPolicy>
                        ._404_NotFound("Account policy not found");
        }

        public async Task<ApiResponse<IEnumerable<AccountPolicy>>> GetAccountPoliciesAsync()
        {
            var accountPolicies = await _accountPolicyRepository.GetAccountPoliciesAsync();
            if (accountPolicies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<AccountPolicy>>
                    ._200_Success("No account policies found", accountPolicies);
            }
            return StatusCodeReturn<IEnumerable<AccountPolicy>>
                    ._200_Success("Account policies found successfully", accountPolicies);
        }

        public async Task<ApiResponse<AccountPolicy>> GetAccountPolicyByPolicyAsync(string policyIdOrName)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                var accoountPolicy = await _accountPolicyRepository
                    .GetAccountPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accoountPolicy != null)
                {
                    return StatusCodeReturn<AccountPolicy>
                        ._200_Success("Account policy found successfully", accoountPolicy);
                }
                return StatusCodeReturn<AccountPolicy>
                    ._404_NotFound("Account policy not found");
            }
            return StatusCodeReturn<AccountPolicy>
                    ._404_NotFound("Policy not found");
            
        }

        public async Task<ApiResponse<AccountPolicy>> GetAccountPolicyByIdAsync(string accountPolicyId)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByIdAsync(
                    accountPolicyId);
            if (accountPolicy != null)
            {
                return StatusCodeReturn<AccountPolicy>
                    ._200_Success("Account policy found successfully", accountPolicy);
            }
            return StatusCodeReturn<AccountPolicy>
                        ._404_NotFound("Account policy not found");
        }

        public async Task<ApiResponse<AccountPolicy>> GetAccountPolicyByPolicyIdAsync(string policyId)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByPolicyIdAsync(policyId);
            if (accountPolicy != null)
            {
                return StatusCodeReturn<AccountPolicy>
                    ._200_Success("Account policy found successfully", accountPolicy);
            }
            return StatusCodeReturn<AccountPolicy>
                        ._404_NotFound("Account policy not found");
        }

        public async Task<ApiResponse<AccountPolicy>> UpdateAccountPolicyAsync(
            UpdateAccountPolicyDto updateAccountPolicyDto)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByIdAsync(
                updateAccountPolicyDto.Id);
            if (accountPolicy != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                    updateAccountPolicyDto.PolicyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var existAccountPolicy = await _accountPolicyRepository
                        .GetAccountPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                    if(existAccountPolicy == null)
                    {
                        updateAccountPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                        var updatedAccountPolicy = await _accountPolicyRepository.UpdateAccountPolicyAsync(
                            ConvertFromDto.ConvertFromAccountPolicyDto_Update(updateAccountPolicyDto));
                        return StatusCodeReturn<AccountPolicy>
                                ._200_Success("Account policy updated successfully", updatedAccountPolicy);
                    }
                    return StatusCodeReturn<AccountPolicy>
                        ._403_Forbidden("Account policy already exists");
                }
                return StatusCodeReturn<AccountPolicy>
                        ._404_NotFound("Policy not found");
            }

            return StatusCodeReturn<AccountPolicy>
                        ._404_NotFound("Account policy not found");
        }


    }
}

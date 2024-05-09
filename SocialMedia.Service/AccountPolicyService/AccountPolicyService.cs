

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.AccountPolicyRepository;
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
            if (policy.ResponseObject != null)
            {
                var accountPolicyByPolicyId = await _accountPolicyRepository
                        .GetAccountPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accountPolicyByPolicyId == null)
                {
                    addAccountPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                    var accountPolicy = await _accountPolicyRepository.AddAccountPolicyAsync(
                        ConvertFromDto.ConvertFromAccountPolicyDto_Add(addAccountPolicyDto));
                    return new ApiResponse<AccountPolicy>
                    {
                        IsSuccess = true,
                        Message = "Account policy added successfully",
                        StatusCode = 201,
                        ResponseObject = accountPolicy
                    };
                }
                return new ApiResponse<AccountPolicy>
                {
                    IsSuccess = false,
                    Message = "Account policy already exists",
                    StatusCode = 400,
                };
                
            }
            return new ApiResponse<AccountPolicy>
            {
                IsSuccess = false,
                Message = "Policy not found",
                StatusCode = 404,
            };
        }

        public async Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByPolicyAsync(string policyIdOrName)
        {
            try
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var accoountPolicy = await _accountPolicyRepository
                        .GetAccountPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                    if (accoountPolicy != null)
                    {
                        var deletedAccountPolicy = await _accountPolicyRepository
                            .DeleteAccountPolicyByIdAsync(accoountPolicy.Id);
                        return new ApiResponse<AccountPolicy>
                        {
                            IsSuccess = true,
                            Message = "Account policy deleted successfully",
                            StatusCode = 200,
                            ResponseObject = deletedAccountPolicy
                        };
                    }
                    return new ApiResponse<AccountPolicy>
                    {
                        IsSuccess = false,
                        Message = "Account policy not found",
                        StatusCode = 404,
                    };
                }
                return new ApiResponse<AccountPolicy>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByIdAsync(string accountPolicyId)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByIdAsync(
                    accountPolicyId);
            if (accountPolicy != null)
            {
                var deletedAccountPolicy = await _accountPolicyRepository
                    .DeleteAccountPolicyByIdAsync(accountPolicyId);
                return new ApiResponse<AccountPolicy>
                {
                    IsSuccess = true,
                    Message = "Account policy deleted successfully",
                    StatusCode = 200,
                    ResponseObject = deletedAccountPolicy
                };
            }
            return new ApiResponse<AccountPolicy>
            {
                IsSuccess = false,
                Message = "Account policy not found",
                StatusCode = 404,
            };
        }

        public async Task<ApiResponse<AccountPolicy>> DeleteAccountPolicyByPolicyIdAsync(string policyId)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByPolicyIdAsync(
                policyId);
            if (accountPolicy != null)
            {
                var deletedAccountPolicy = await _accountPolicyRepository
                    .DeleteAccountPolicyByPolicyIdAsync(policyId);
                return new ApiResponse<AccountPolicy>
                {
                    IsSuccess = true,
                    Message = "Account policy deleted successfully",
                    StatusCode = 200,
                    ResponseObject = deletedAccountPolicy
                };
            }
            return new ApiResponse<AccountPolicy>
            {
                IsSuccess = false,
                Message = "Account policy not found",
                StatusCode = 404,
            };
        }

        public async Task<ApiResponse<IEnumerable<AccountPolicy>>> GetAccountPoliciesAsync()
        {
            var accountPolicies = await _accountPolicyRepository.GetAccountPoliciesAsync();
            if (accountPolicies.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<AccountPolicy>>
                {
                    IsSuccess = true,
                    Message = "No account policies found",
                    StatusCode = 200,
                    ResponseObject = accountPolicies
                };
            }
            return new ApiResponse<IEnumerable<AccountPolicy>>
            {
                IsSuccess = true,
                Message = "Account policies found successfully",
                StatusCode = 200,
                ResponseObject = accountPolicies
            };
        }

        public async Task<ApiResponse<AccountPolicy>> GetAccountPolicyByPolicyAsync(string policyIdOrName)
        {
            try
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var accoountPolicy = await _accountPolicyRepository
                        .GetAccountPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                    if (accoountPolicy != null)
                    {
                        return new ApiResponse<AccountPolicy>
                        {
                            IsSuccess = true,
                            Message = "Account policy found successfully",
                            StatusCode = 200,
                            ResponseObject = accoountPolicy
                        };
                    }
                    return new ApiResponse<AccountPolicy>
                    {
                        IsSuccess = false,
                        Message = "Account policy not found",
                        StatusCode = 404,
                    };
                }
                return new ApiResponse<AccountPolicy>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<AccountPolicy>> GetAccountPolicyByIdAsync(string accountPolicyId)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByIdAsync(
                    accountPolicyId);
            if (accountPolicy != null)
            {
                return new ApiResponse<AccountPolicy>
                {
                    IsSuccess = true,
                    Message = "Account policy found successfully",
                    StatusCode = 200,
                    ResponseObject = accountPolicy
                };
            }
            return new ApiResponse<AccountPolicy>
            {
                IsSuccess = false,
                Message = "Account policy not found",
                StatusCode = 404,
            };
        }

        public async Task<ApiResponse<AccountPolicy>> GetAccountPolicyByPolicyIdAsync(string policyId)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByPolicyIdAsync(
                policyId);
            if (accountPolicy != null)
            {
                return new ApiResponse<AccountPolicy>
                {
                    IsSuccess = true,
                    Message = "Account policy found successfully",
                    StatusCode = 200,
                    ResponseObject = accountPolicy
                };
            }
            return new ApiResponse<AccountPolicy>
            {
                IsSuccess = false,
                Message = "Account policy not found",
                StatusCode = 404,
            };
        }

        public async Task<ApiResponse<AccountPolicy>> 
            UpdateAccountPolicyAsync(UpdateAccountPolicyDto updateAccountPolicyDto)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByIdAsync(
                updateAccountPolicyDto.Id);
            if (accountPolicy != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                    updateAccountPolicyDto.PolicyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var accountPolicyByPolicyId = await _accountPolicyRepository
                        .GetAccountPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                    if (accountPolicyByPolicyId == null)
                    {
                        updateAccountPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                        var updatedAccountPolicy = await _accountPolicyRepository.UpdateAccountPolicyAsync(
                            ConvertFromDto.ConvertFromAccountPolicyDto_Update(updateAccountPolicyDto));
                        return new ApiResponse<AccountPolicy>
                        {
                            IsSuccess = true,
                            Message = "Account policy updated successfully",
                            StatusCode = 200,
                            ResponseObject = updatedAccountPolicy
                        };
                    }
                    return new ApiResponse<AccountPolicy>
                    {
                        IsSuccess = false,
                        Message = "Account policy already exists",
                        StatusCode = 400,
                    };
                }
                return new ApiResponse<AccountPolicy>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404,
                };
            }
            
            return new ApiResponse<AccountPolicy>
            {
                IsSuccess = false,
                Message = "Account policy not found",
                StatusCode = 404,
            };
        }


    }
}

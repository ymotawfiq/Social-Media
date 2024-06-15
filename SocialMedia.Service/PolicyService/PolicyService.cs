

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.PolicyService
{
    public class PolicyService : IPolicyService
    {
        private readonly Policies policies = new();
        private readonly IPolicyRepository _policyRepository;
        public PolicyService(IPolicyRepository _policyRepository)
        {
            this._policyRepository = _policyRepository;
        }

        public async Task<ApiResponse<Policy>> AddPolicyAsync(AddPolicyDto addPolicyDto)
        {
            var existPolicy = await _policyRepository.GetPolicyByNameAsync(addPolicyDto.PolicyType);
            if (existPolicy == null)
            {
                if (policies.BasicPolicies.Contains(addPolicyDto.PolicyType.ToUpper()))
                {
                    var newPolicy = await _policyRepository.AddPolicyAsync(
                    ConvertFromDto.ConvertFromPolicyDto_Add(addPolicyDto));
                    return StatusCodeReturn<Policy>
                            ._201_Created("Policy added successfully", newPolicy);
                }
                return StatusCodeReturn<Policy>
                    ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<Policy>
                    ._403_Forbidden("Policy already exists");
            
        }

        public async Task<ApiResponse<Policy>> DeletePolicyByIdAsync(string policyId)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(policyId);
            if (policy != null)
            {
                await _policyRepository.DeletePolicyByIdAsync(policyId);
                return StatusCodeReturn<Policy>
                        ._200_Success("Policy deleted successfully", policy);
            }
            return StatusCodeReturn<Policy>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Policy>> DeletePolicyByIdOrNameAsync(string policyIdOrName)
        {
            var policy = await GetPolicyAsync(policyIdOrName);
            if (policy != null)
            {
                await _policyRepository.DeletePolicyByIdAsync(policy.Id);
                return StatusCodeReturn<Policy>
                        ._200_Success("Policy deleted successfully", policy);
                    
            }
            return StatusCodeReturn<Policy>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Policy>> DeletePolicyByNameAsync(string policyName)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync(policyName);
            if (policy != null)
            {
                await _policyRepository.DeletePolicyByIdAsync(policy.Id);
                return StatusCodeReturn<Policy>
                        ._200_Success("Policy deleted successfully", policy);
                    
            }
            return StatusCodeReturn<Policy>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<IEnumerable<Policy>>> GetPoliciesAsync()
        {
            var policies = await _policyRepository.GetPoliciesAsync();
            if (policies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<Policy>>
                    ._200_Success("No policies found", policies);
            }
            return StatusCodeReturn<IEnumerable<Policy>>
                    ._200_Success("Policies found successfully", policies);
        }

        public async Task<ApiResponse<Policy>> GetPolicyByIdAsync(string policyId)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(policyId);
            if (policy != null)
            {
                return StatusCodeReturn<Policy>
                    ._200_Success("Policy found successfully", policy);
            }
            return StatusCodeReturn<Policy>
                        ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Policy>> GetPolicyByIdOrNameAsync(string policyIdOrName)
        {
            var policy = await GetPolicyAsync(policyIdOrName);
            if (policy != null)
            {
                return StatusCodeReturn<Policy>
                    ._200_Success("Policy found successfully", policy);
                    
            }
            return StatusCodeReturn<Policy>
                        ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Policy>> GetPolicyByNameAsync(string policyName)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync(policyName);
            if (policy != null)
            {
                return StatusCodeReturn<Policy>
                    ._200_Success("Policy found successfully", policy);
                    
            }
            return StatusCodeReturn<Policy>
                        ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Policy>> UpdatePolicyAsync(UpdatePolicyDto updatePolicyDto)
        {
            var policyById = await _policyRepository.GetPolicyByIdAsync(updatePolicyDto.Id);
            if (policyById != null)
            {
                var policyByName = await _policyRepository.GetPolicyByNameAsync(updatePolicyDto.PolicyType);
                if (policyByName == null)
                {
                    if (policies.BasicPolicies.Contains(updatePolicyDto.PolicyType.ToUpper()))
                    {
                        var updatedPolicy = await _policyRepository.UpdatePolicyAsync(
                            ConvertFromDto.ConvertFromPolicyDto_Update(updatePolicyDto));
                        return StatusCodeReturn<Policy>
                                ._200_Success("Policy updated successfully", updatedPolicy);
                    }
                    return StatusCodeReturn<Policy>
                   ._403_Forbidden("Invalid policy");
                }
                return StatusCodeReturn<Policy>
                       ._403_Forbidden("Policy already exists");
            }
            return StatusCodeReturn<Policy>
                   ._404_NotFound("Policy not found");
        }


        private async Task<Policy> GetPolicyAsync(string policyIdOrName)
        {
            var policyById = await _policyRepository.GetPolicyByIdAsync(policyIdOrName);
            var policyByName = await _policyRepository.GetPolicyByNameAsync(policyIdOrName);
            return policyById == null ? policyByName! : policyById;
        }
    }
}

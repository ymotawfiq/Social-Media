

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
        private readonly IPolicyRepository _policyRepository;
        public PolicyService(IPolicyRepository _policyRepository)
        {
            this._policyRepository = _policyRepository;
        }

        public async Task<ApiResponse<Policy>> AddPolicyAsync(PolicyDto policyDto)
        {
            var newPolicy = await _policyRepository.AddPolicyAsync(
                ConvertFromDto.ConvertFromPolicyDto_Add(policyDto));
            if (newPolicy == null)
            {
                return StatusCodeReturn<Policy>
                    ._500_ServerError("Can't add policy");
            }
            var policy = await _policyRepository.GetPolicyByNameAsync(policyDto.PolicyType);
            if (policy != null)
            {
                return StatusCodeReturn<Policy>
                    ._400_BadRequest("Policy already exists");
            }
            return StatusCodeReturn<Policy>
                    ._201_Created("Policy added successfully", newPolicy);
        }

        public async Task<ApiResponse<Policy>> DeletePolicyByIdAsync(string policyId)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(policyId.ToString());
            if (policy == null)
            {
                return StatusCodeReturn<Policy>
                    ._404_NotFound("Policy not found");
            }
            var deletedPolicy = await _policyRepository.DeletePolicyByIdAsync(policyId);
            return StatusCodeReturn<Policy>
                    ._200_Success("Policy deleted successfully", deletedPolicy);
        }

        public async Task<ApiResponse<Policy>> DeletePolicyByIdOrNameAsync(string policyIdOrName)
        {
            try
            {
                var policy = await GetPolicyAsync(policyIdOrName);
                if (policy == null)
                {
                    return StatusCodeReturn<Policy>
                        ._404_NotFound("Policy not found");
                }
                var deletedPolicy = await _policyRepository.DeletePolicyByIdAsync(policy.Id);
                return StatusCodeReturn<Policy>
                        ._200_Success("Policy deleted successfully", deletedPolicy);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<Policy>> DeletePolicyByNameAsync(string policyName)
        {
            try
            {
                var policy = await _policyRepository.GetPolicyByNameAsync(policyName);
                if (policy == null)
                {
                    return StatusCodeReturn<Policy>
                        ._404_NotFound("Policy not found");
                }
                var deletedPolicy = await _policyRepository.DeletePolicyByIdAsync(policy.Id);
                return StatusCodeReturn<Policy>
                        ._200_Success("Policy deleted successfully", deletedPolicy);
            }
            catch (Exception)
            {
                throw;
            }
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
            if (policy == null)
            {
                return StatusCodeReturn<Policy>
                        ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<Policy>
                    ._200_Success("Policy found successfully", policy);
        }

        public async Task<ApiResponse<Policy>> GetPolicyByIdOrNameAsync(string policyIdOrName)
        {
            try
            {
                var policy = await GetPolicyAsync(policyIdOrName);
                if (policy == null)
                {
                    return StatusCodeReturn<Policy>
                            ._404_NotFound("Policy not found");
                }
                return StatusCodeReturn<Policy>
                        ._200_Success("Policy found successfully", policy);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<Policy>> GetPolicyByNameAsync(string policyName)
        {
            try
            {
                var policy = await _policyRepository.GetPolicyByNameAsync(policyName);
                if (policy == null)
                {
                    return StatusCodeReturn<Policy>
                            ._404_NotFound("Policy not found");
                }
                return StatusCodeReturn<Policy>
                        ._200_Success("Policy found successfully", policy);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<Policy>> UpdatePolicyAsync(PolicyDto policyDto)
        {
            if (policyDto.Id == null)
            {
                return StatusCodeReturn<Policy>
                            ._400_BadRequest("Policy id must not be null");
            }
            var updatedPolicy = await _policyRepository.UpdatePolicyAsync(
                ConvertFromDto.ConvertFromPolicyDto_Update(policyDto));
            return StatusCodeReturn<Policy>
                    ._200_Success("Policy updated successfully", updatedPolicy);
        }


        private async Task<Policy> GetPolicyAsync(string policyIdOrName)
        {
            var policyById = await _policyRepository.GetPolicyByIdAsync(policyIdOrName);
            var policyByName = await _policyRepository.GetPolicyByNameAsync(policyIdOrName);
            return policyById == null ? policyByName! : policyById;
        }
    }
}

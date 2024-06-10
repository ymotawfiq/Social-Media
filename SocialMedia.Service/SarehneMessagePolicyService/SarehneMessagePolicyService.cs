

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.SarehneMessagePolicyRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.SarehneMessagePolicyService
{
    public class SarehneMessagePolicyService : ISarehneMessagePolicyService
    {
        private readonly ISarehneMessagePolicyRepository _sarehneMessagePolicyRepository;
        private readonly IPolicyService _policyService;
        public SarehneMessagePolicyService(ISarehneMessagePolicyRepository _sarehneMessagePolicyRepository,
            IPolicyService _policyService)
        {
            this._sarehneMessagePolicyRepository = _sarehneMessagePolicyRepository;
            this._policyService = _policyService;
        }
        public async Task<ApiResponse<SarehneMessagePolicy>> AddPolicyAsync(
            AddSarehneMessagePolicyDto addSarehneMessagePolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(
                addSarehneMessagePolicyDto.PolicyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (messagePolicy == null)
                {
                    addSarehneMessagePolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                    var newMessagePolicy = await _sarehneMessagePolicyRepository.AddPolicyAsync(
                        ConvertFromDto.ConvertFromSarehneMessagePolicyDto_Add(addSarehneMessagePolicyDto));
                    return StatusCodeReturn<SarehneMessagePolicy>
                        ._201_Created("Message policy added successfully", newMessagePolicy);
                }
                return StatusCodeReturn<SarehneMessagePolicy>
                    ._403_Forbidden("Message policy already exists");
            }
            return StatusCodeReturn<SarehneMessagePolicy>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<SarehneMessagePolicy>> DeletePolicyByIdAsync(
            string sarehneMessagePolicyId)
        {
            var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                sarehneMessagePolicyId);
            if (messagePolicy != null)
            {
                await _sarehneMessagePolicyRepository.DeletePolicyByIdAsync(messagePolicy.Id);
                return StatusCodeReturn<SarehneMessagePolicy>
                    ._200_Success("Message policy deleted successfully", messagePolicy);
            }
            return StatusCodeReturn<SarehneMessagePolicy>
                ._404_NotFound("Message policy not found");
        }

        public async Task<ApiResponse<SarehneMessagePolicy>> DeletePolicyByPolicyAsync(string policyIdOrName)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (messagePolicy != null)
                {
                    await _sarehneMessagePolicyRepository.DeletePolicyByIdAsync(messagePolicy.Id);
                    return StatusCodeReturn<SarehneMessagePolicy>
                        ._200_Success("Message policy deleted successfully", messagePolicy);
                }
                return StatusCodeReturn<SarehneMessagePolicy>
                    ._404_NotFound("Message policy not found");
            }
            return StatusCodeReturn<SarehneMessagePolicy>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<IEnumerable<SarehneMessagePolicy>>> GetPoliciesAsync()
        {
            var policies = await _sarehneMessagePolicyRepository.GetPoliciesAsync();
            if (policies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<SarehneMessagePolicy>>
                    ._200_Success("No policies found", policies);
            }
            return StatusCodeReturn<IEnumerable<SarehneMessagePolicy>>
                    ._200_Success("Policies found successfully", policies);
        }

        public async Task<ApiResponse<SarehneMessagePolicy>> GetPolicyByIdAsync(string sarehneMessagePolicyId)
        {
            var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                sarehneMessagePolicyId);
            if (messagePolicy != null)
            {
                return StatusCodeReturn<SarehneMessagePolicy>
                    ._200_Success("Message policy found successfully", messagePolicy);
            }
            return StatusCodeReturn<SarehneMessagePolicy>
                ._404_NotFound("Message policy not found");
        }

        public async Task<ApiResponse<SarehneMessagePolicy>> GetPolicyByPolicyAsync(string policyIdOrName)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (messagePolicy != null)
                {
                    return StatusCodeReturn<SarehneMessagePolicy>
                        ._200_Success("Message policy found successfully", messagePolicy);
                }
                return StatusCodeReturn<SarehneMessagePolicy>
                    ._404_NotFound("Message policy not found");
            }
            return StatusCodeReturn<SarehneMessagePolicy>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<SarehneMessagePolicy>> UpdatePolicyAsync(
            UpdateSarehneMessagePolicyToAnotherDto updateSarehneMessagePolicyToAnotherDto)
        {
            var existMessagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByIdAsync(
                updateSarehneMessagePolicyToAnotherDto.Id);
            if (existMessagePolicy != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                updateSarehneMessagePolicyToAnotherDto.PolicyIdOrName);
                if (policy != null && policy.ResponseObject != null)
                {
                    var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                        policy.ResponseObject.Id);
                    if (messagePolicy == null)
                    {
                        updateSarehneMessagePolicyToAnotherDto.PolicyIdOrName = policy.ResponseObject.Id;
                        var updatedMessagePolicy = await _sarehneMessagePolicyRepository.AddPolicyAsync(
                            ConvertFromDto.ConvertFromSarehneMessagePolicyDto_Update(
                                updateSarehneMessagePolicyToAnotherDto));
                        return StatusCodeReturn<SarehneMessagePolicy>
                            ._200_Success("Message policy updated successfully", updatedMessagePolicy);
                    }
                    return StatusCodeReturn<SarehneMessagePolicy>
                        ._403_Forbidden("Message policy already exists");
                }
                return StatusCodeReturn<SarehneMessagePolicy>
                        ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<SarehneMessagePolicy>
                        ._404_NotFound("Message policy not found");
        }
    }
}

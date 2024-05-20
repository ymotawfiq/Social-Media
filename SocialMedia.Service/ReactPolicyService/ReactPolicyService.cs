

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.ReactPolicyRepository;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.ReactPolicyService
{
    public class ReactPolicyService : IReactPolicyService
    {
        private readonly IReactPolicyRepository _reactPolicyRepository;
        private readonly IPolicyRepository _policyRepository;
        public ReactPolicyService(IReactPolicyRepository _reactPolicyRepository,
            IPolicyRepository _policyRepository)
        {
            this._reactPolicyRepository = _reactPolicyRepository;
            this._policyRepository = _policyRepository;
        }
        public async Task<ApiResponse<ReactPolicy>> AddReactPolicyAsync(ReactPolicyDto reactPolicyDto)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(reactPolicyDto.PolicyId);
            if (policy == null)
            {
                return StatusCodeReturn<ReactPolicy>
                    ._404_NotFound("Policy not found");
            }
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(reactPolicyDto.PolicyId);
            if (reactPolicy != null)
            {
                return StatusCodeReturn<ReactPolicy>
                    ._400_BadRequest("React policy already exists");
            }
            var newReactPolicy = await _reactPolicyRepository.AddReactPolicyAsync(
                ConvertFromDto.ConvertFromReactPolicyDto_Add(reactPolicyDto));
            return StatusCodeReturn<ReactPolicy>
                    ._201_Created("React policy added successfully", newReactPolicy);
        }

        public async Task<ApiResponse<ReactPolicy>> DeleteReactPolicyByIdAsync(string reactPolicyId)
        {
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByIdAsync(reactPolicyId);
            if (reactPolicy == null)
            {
                return StatusCodeReturn<ReactPolicy>
                    ._404_NotFound("React policy not found");
            }
            var deletedReactPolicy = await _reactPolicyRepository.DeleteReactPolicyByIdAsync(reactPolicyId);
            return StatusCodeReturn<ReactPolicy>
                    ._200_Success("React policy deleted successfully", deletedReactPolicy);
        }

        public async Task<ApiResponse<IEnumerable<ReactPolicy>>> GetReactPoliciesAsync()
        {
            var reactPolicies = await _reactPolicyRepository.GetReactPoliciesAsync();
            if (reactPolicies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<ReactPolicy>>
                    ._200_Success("No react policies found", reactPolicies);
            }
            return StatusCodeReturn<IEnumerable<ReactPolicy>>
                    ._200_Success("React policies found successfully", reactPolicies);
        }

        public async Task<ApiResponse<ReactPolicy>> GetReactPolicyByIdAsync(string reactPolicyId)
        {
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByIdAsync(reactPolicyId);
            if (reactPolicy == null)
            {
                return StatusCodeReturn<ReactPolicy>
                    ._404_NotFound("React policy not found");
            }
            return StatusCodeReturn<ReactPolicy>
                ._200_Success("React policy found successfully", reactPolicy);
        }

        public async Task<ApiResponse<ReactPolicy>> UpdateReactPolicyAsync(ReactPolicyDto reactPolicyDto)
        {
            if (reactPolicyDto.Id == null)
            {
                return StatusCodeReturn<ReactPolicy>
                    ._400_BadRequest("React policy id must not be null");
            }
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByIdAsync(reactPolicyDto.Id);
            if (reactPolicy == null)
            {
                return StatusCodeReturn<ReactPolicy>
                    ._404_NotFound("React policy not found");
            }
            var policy = await _policyRepository.GetPolicyByIdAsync(reactPolicyDto.PolicyId);
            if (policy == null)
            {
                return StatusCodeReturn<ReactPolicy>
                    ._404_NotFound("Policy not found"); ;
            }
            var updatedReactPolicy = await _reactPolicyRepository.UpdateReactPolicyAsync(
                ConvertFromDto.ConvertFromReactPolicyDto_Update(reactPolicyDto));
            return StatusCodeReturn<ReactPolicy>
                ._200_Success("React policy updated successfully", updatedReactPolicy);
        }
    }
}

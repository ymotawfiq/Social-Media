

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.ReactPolicyRepository;
using SocialMedia.Repository.ReactRepository;

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
                return new ApiResponse<ReactPolicy>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(reactPolicyDto.PolicyId);
            if (reactPolicy != null)
            {
                return new ApiResponse<ReactPolicy>
                {
                    IsSuccess = false,
                    Message = "React policy already exists",
                    StatusCode = 400
                };
            }
            var newReactPolicy = await _reactPolicyRepository.AddReactPolicyAsync(
                ConvertFromDto.ConvertFromReactPolicyDto_Add(reactPolicyDto));
            return new ApiResponse<ReactPolicy>
            {
                IsSuccess = true,
                Message = "React policy added successfully",
                StatusCode = 201,
                ResponseObject = newReactPolicy
            };
        }

        public async Task<ApiResponse<ReactPolicy>> DeleteReactPolicyByIdAsync(string reactPolicyId)
        {
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByIdAsync(reactPolicyId);
            if (reactPolicy == null)
            {
                return new ApiResponse<ReactPolicy>
                {
                    IsSuccess = false,
                    Message = "React policy not found",
                    StatusCode = 404
                };
            }
            var deletedReactPolicy = await _reactPolicyRepository.DeleteReactPolicyByIdAsync(reactPolicyId);
            return new ApiResponse<ReactPolicy>
            {
                IsSuccess = true,
                Message = "React policy deleted successfully",
                StatusCode = 200,
                ResponseObject = deletedReactPolicy
            };
        }

        public async Task<ApiResponse<IEnumerable<ReactPolicy>>> GetReactPoliciesAsync()
        {
            var reactPolicies = await _reactPolicyRepository.GetReactPoliciesAsync();
            if (reactPolicies.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<ReactPolicy>>
                {
                    IsSuccess = true,
                    Message = "No react policies found",
                    StatusCode = 200,
                    ResponseObject = reactPolicies
                };
            }
            return new ApiResponse<IEnumerable<ReactPolicy>>
            {
                IsSuccess = true,
                Message = "React policies found successfully",
                StatusCode = 200,
                ResponseObject = reactPolicies
            };
        }

        public async Task<ApiResponse<ReactPolicy>> GetReactPolicyByIdAsync(string reactPolicyId)
        {
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByIdAsync(reactPolicyId);
            if (reactPolicy == null)
            {
                return new ApiResponse<ReactPolicy>
                {
                    IsSuccess = false,
                    Message = "React policy not found",
                    StatusCode = 404
                };
            }
            return new ApiResponse<ReactPolicy>
            {
                IsSuccess = true,
                Message = "React policy found successfully",
                StatusCode = 200,
                ResponseObject = reactPolicy
            };
        }

        public async Task<ApiResponse<ReactPolicy>> UpdateReactPolicyAsync(ReactPolicyDto reactPolicyDto)
        {
            if (reactPolicyDto.Id == null)
            {
                return new ApiResponse<ReactPolicy>
                {
                    IsSuccess = false,
                    Message = "React policy id must not be null",
                    StatusCode = 400
                };
            }
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByIdAsync(reactPolicyDto.Id);
            if (reactPolicy == null)
            {
                return new ApiResponse<ReactPolicy>
                {
                    IsSuccess = false,
                    Message = "React policy not found",
                    StatusCode = 404
                };
            }
            var policy = await _policyRepository.GetPolicyByIdAsync(reactPolicyDto.PolicyId);
            if (policy == null)
            {
                return new ApiResponse<ReactPolicy>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            var updatedReactPolicy = await _reactPolicyRepository.UpdateReactPolicyAsync(
                ConvertFromDto.ConvertFromReactPolicyDto_Update(reactPolicyDto));
            return new ApiResponse<ReactPolicy>
            {
                IsSuccess = true,
                Message = "React policy updated successfully",
                StatusCode = 200,
                ResponseObject = updatedReactPolicy
            };
        }
    }
}

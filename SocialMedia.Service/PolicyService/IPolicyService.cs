

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.PolicyService
{
    public interface IPolicyService
    {
        Task<ApiResponse<Policy>> AddPolicyAsync(AddPolicyDto addPolicyDto);
        Task<ApiResponse<Policy>> UpdatePolicyAsync(UpdatePolicyDto updatePolicyDto);
        Task<ApiResponse<Policy>> DeletePolicyByIdAsync(string policyId);
        Task<ApiResponse<Policy>> DeletePolicyByNameAsync(string policyName);
        Task<ApiResponse<Policy>> DeletePolicyByIdOrNameAsync(string policyIdOrName);
        Task<ApiResponse<Policy>> GetPolicyByIdAsync(string policyId);
        Task<ApiResponse<Policy>> GetPolicyByIdOrNameAsync(string policyIdOrName);
        Task<ApiResponse<Policy>> GetPolicyByNameAsync(string policyName);
        Task<ApiResponse<IEnumerable<Policy>>> GetPoliciesAsync();
    }
}

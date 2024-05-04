

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.PolicyService
{
    public interface IPolicyService
    {
        Task<ApiResponse<Policy>> AddPolicyAsync(PolicyDto policyDto);
        Task<ApiResponse<Policy>> UpdatePolicyAsync(PolicyDto policyDto);
        Task<ApiResponse<Policy>> DeletePolicyByIdAsync(Guid policyId);
        Task<ApiResponse<Policy>> GetPolicyByIdAsync(Guid policyId);
        Task<ApiResponse<IEnumerable<Policy>>> GetPoliciesAsync();
    }
}



using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.ReactPolicyService
{
    public interface IReactPolicyService
    {
        Task<ApiResponse<ReactPolicy>> AddReactPolicyAsync(ReactPolicyDto reactPolicyDto);
        Task<ApiResponse<ReactPolicy>> UpdateReactPolicyAsync(ReactPolicyDto reactPolicyDto);
        Task<ApiResponse<ReactPolicy>> DeleteReactPolicyByIdAsync(string reactPolicyId);
        Task<ApiResponse<ReactPolicy>> GetReactPolicyByIdAsync(string reactPolicyId);
        Task<ApiResponse<ReactPolicy>> GetReactPolicyAsync(string reactPolicyIdOrPolicyName);
        Task<ApiResponse<IEnumerable<ReactPolicy>>> GetReactPoliciesAsync();
    }
}

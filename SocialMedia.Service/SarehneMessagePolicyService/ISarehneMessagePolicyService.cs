
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.SarehneMessagePolicyService
{
    public interface ISarehneMessagePolicyService
    {
        Task<ApiResponse<SarehneMessagePolicy>> AddPolicyAsync(
            AddSarehneMessagePolicyDto addSarehneMessagePolicyDto);
        Task<ApiResponse<SarehneMessagePolicy>> UpdatePolicyAsync(
            UpdateSarehneMessagePolicyToAnotherDto updateSarehneMessagePolicyToAnotherDto);
        Task<ApiResponse<SarehneMessagePolicy>> GetPolicyByPolicyAsync(string policyIdOrName);
        Task<ApiResponse<SarehneMessagePolicy>> GetPolicyByIdAsync(string sarehneMessagePolicyId);
        Task<ApiResponse<SarehneMessagePolicy>> DeletePolicyByPolicyAsync(string policyIdOrName);
        Task<ApiResponse<SarehneMessagePolicy>> DeletePolicyByIdAsync(string sarehneMessagePolicyId);
        Task<ApiResponse<IEnumerable<SarehneMessagePolicy>>> GetPoliciesAsync();
    }
}

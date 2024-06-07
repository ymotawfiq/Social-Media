

using SocialMedia.Data.DTOs;

namespace SocialMedia.Service.GroupPolicyService
{
    public interface IGroupPolicyService
    {
        Task<object> AddGrouPolicyAsync(AddGroupPolicyDto addGroupPolicyDto);
        Task<object> UpdateGrouPolicyAsync(UpdateGroupPolicyDto updateGroupPolicyDto);
        Task<object> GetGrouPolicyByIdAsync(string groupPolicyId);
        Task<object> GetGrouPolicyByPolicyIdAsync(string policyId);
        Task<object> DeleteGrouPolicyByIdAsync(string groupPolicyId);
        Task<object> DeleteGrouPolicyByPolicyIdAsync(string policyId);
        Task<object> DeleteGrouPolicyByPolicyIdOrNameAsync(string policyIdOrName);
        Task<object> GetGrouPolicyByPolicyIdOrNameAsync(string policyIdOrName);
        Task<object> GetGrouPolicyByGroupPolicyIdOrPolicyIdOrNameAsync(string groupPolicyIdOrPolicyIdOrName);
        Task<object> DeleteGrouPolicyByGroupPolicyIdOrPolicyIdOrNameAsync(
            string groupPolicyIdOrPolicyIdOrName);
        Task<object> GetGrouPoliciesAsync();
    }
}



using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PolicyRepository
{
    public interface IPolicyRepository
    {
        Task<Policy> AddPolicyAsync(Policy policy);
        Task<Policy> UpdatePolicyAsync(Policy policy);
        Task<Policy> DeletePolicyByIdAsync(string policyId);
        Task<Policy> DeletePolicyAsync(string policyIdOrName);
        Task<Policy> GetPolicyByIdAsync(string policyId);
        Task<Policy> GetPolicyByNameAsync(string policyName);
        Task<IEnumerable<Policy>> GetPoliciesAsync();
        Task SaveChangesAsync();
    }
}

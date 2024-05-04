

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PolicyRepository
{
    public interface IPolicyRepository
    {
        Task<Policy> AddPolicyAsync(Policy policy);
        Task<Policy> UpdatePolicyAsync(Policy policy);
        Task<Policy> DeletePolicyByIdAsync(Guid policyId);
        Task<Policy> GetPolicyByIdAsync(Guid policyId);
        Task<IEnumerable<Policy>> GetPoliciesAsync();
        Task SaveChangesAsync();
    }
}

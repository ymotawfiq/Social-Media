

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.ReactPolicyRepository
{
    public interface IReactPolicyRepository
    {

        Task<ReactPolicy> AddReactPolicyAsync(ReactPolicy reactPolicy);
        Task<ReactPolicy> UpdateReactPolicyAsync(ReactPolicy reactPolicy);
        Task<ReactPolicy> DeleteReactPolicyByIdAsync(string reactPolicyId);
        Task<ReactPolicy> GetReactPolicyByIdAsync(string reactPolicyId);
        Task<ReactPolicy> GetReactPolicyByPolicyIdAsync(string policyId);
        Task<IEnumerable<ReactPolicy>> GetReactPoliciesAsync();
        Task SaveChangesAsync();

    }
}

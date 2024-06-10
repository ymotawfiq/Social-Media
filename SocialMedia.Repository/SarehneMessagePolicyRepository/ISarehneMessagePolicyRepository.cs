
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.SarehneMessagePolicyRepository
{
    public interface ISarehneMessagePolicyRepository
    {
        Task<SarehneMessagePolicy> AddPolicyAsync(SarehneMessagePolicy sarehneMessagePolicy);
        Task<SarehneMessagePolicy> UpdatePolicyAsync(SarehneMessagePolicy sarehneMessagePolicy);
        Task<SarehneMessagePolicy> GetPolicyByPolicyIdAsync(string policyId);
        Task<SarehneMessagePolicy> GetPolicyByIdAsync(string sarehneMessagePolicyId);
        Task<SarehneMessagePolicy> DeletePolicyByPolicyIdAsync(string policyId);
        Task<SarehneMessagePolicy> DeletePolicyByIdAsync(string sarehneMessagePolicyId);
        Task<IEnumerable<SarehneMessagePolicy>> GetPoliciesAsync();
        Task SaveChangesAsync();
    }
}

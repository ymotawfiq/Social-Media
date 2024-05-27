

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.AccountPostsPolicyRepository
{
    public interface IAccountPostsPolicyRepository
    {
        Task<AccountPostsPolicy> AddAccountPostPolicyAsync(AccountPostsPolicy postPolicy);
        Task<AccountPostsPolicy> UpdateAccountPostPolicyAsync(AccountPostsPolicy postPolicy);
        Task<AccountPostsPolicy> DeleteAccountPostPolicyByIdAsync(string postPolicyId);
        Task<AccountPostsPolicy> DeleteAccountPostPolicyByPolicyIdAsync(string policyId);
        Task<AccountPostsPolicy> GetAccountPostPolicyByIdAsync(string postPolicyId);
        Task<AccountPostsPolicy> GetAccountPostPolicyByPolicyIdAsync(string policyId);
        Task<IEnumerable<AccountPostsPolicy>> GetAccountPostPoliciesAsync();
        Task SaveChangesAsync();
    }
}

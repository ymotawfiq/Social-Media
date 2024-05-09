

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.AccountPolicyRepository
{
    public interface IAccountPolicyRepository
    {
        Task<AccountPolicy> AddAccountPolicyAsync(AccountPolicy accountPolicy);
        Task<AccountPolicy> UpdateAccountPolicyAsync(AccountPolicy accountPolicy);
        Task<AccountPolicy> GetAccountPolicyByIdAsync(string accountPolicyId);
        Task<AccountPolicy> GetAccountPolicyByPolicyIdAsync(string policyId);
        Task<AccountPolicy> DeleteAccountPolicyByIdAsync(string accountPolicyId);
        Task<AccountPolicy> DeleteAccountPolicyByPolicyIdAsync(string policyId);
        Task<IEnumerable<AccountPolicy>> GetAccountPoliciesAsync();
        Task SaveChangesAsync();
    }
}

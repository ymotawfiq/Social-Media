

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.AccountPolicyRepository
{
    public class AccountPolicyRepository : IAccountPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountPolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<AccountPolicy> AddAccountPolicyAsync(AccountPolicy accountPolicy)
        {
            try
            {
                await _dbContext.AccountPolicies.AddAsync(accountPolicy);
                await SaveChangesAsync();
                return accountPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPolicy> DeleteAccountPolicyByIdAsync(string accountPolicyId)
        {
            try
            {
                var accountPolicy = await GetAccountPolicyByIdAsync(accountPolicyId);
                _dbContext.AccountPolicies.Remove(accountPolicy);
                await SaveChangesAsync();
                return accountPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPolicy> DeleteAccountPolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                var accountPolicy = await GetAccountPolicyByPolicyIdAsync(policyId);
                _dbContext.AccountPolicies.Remove(accountPolicy);
                await SaveChangesAsync();
                return accountPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AccountPolicy>> GetAccountPoliciesAsync()
        {
            try
            {
                return await _dbContext.AccountPolicies.Include(e=>e.Policy).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPolicy> GetAccountPolicyByIdAsync(string accountPolicyId)
        {
            try
            {
                return (await _dbContext.AccountPolicies.Where(e => e.Id == accountPolicyId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPolicy> GetAccountPolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                return (await _dbContext.AccountPolicies.Where(e => e.PolicyId == policyId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<AccountPolicy> UpdateAccountPolicyAsync(AccountPolicy accountPolicy)
        {
            try
            {
                var accountPolicy1 = await GetAccountPolicyByIdAsync(accountPolicy.Id);
                accountPolicy1.PolicyId = accountPolicy.PolicyId;
                await SaveChangesAsync();
                return accountPolicy1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

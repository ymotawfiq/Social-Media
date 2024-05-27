

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using Twilio.Jwt.Taskrouter;

namespace SocialMedia.Repository.AccountPostsPolicyRepository
{
    public class AccountPostsPolicyRepository : IAccountPostsPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountPostsPolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<AccountPostsPolicy> AddAccountPostPolicyAsync(AccountPostsPolicy postPolicy)
        {
            try
            {
                await _dbContext.PostPolicies.AddAsync(postPolicy);
                await SaveChangesAsync();
                return postPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPostsPolicy> DeleteAccountPostPolicyByIdAsync(string postPolicyId)
        {
            try
            {
                var postPolicy = await GetAccountPostPolicyByIdAsync(postPolicyId);
                _dbContext.PostPolicies.Remove(postPolicy);
                await SaveChangesAsync();
                return postPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPostsPolicy> DeleteAccountPostPolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                var postPolicy = await GetAccountPostPolicyByPolicyIdAsync(policyId);
                _dbContext.PostPolicies.Remove(postPolicy);
                await SaveChangesAsync();
                return postPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AccountPostsPolicy>> GetAccountPostPoliciesAsync()
        {
            try
            {
                return await _dbContext.PostPolicies.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPostsPolicy> GetAccountPostPolicyByIdAsync(string postPolicyId)
        {
            try
            {
                return (await _dbContext.PostPolicies.Where(e => e.Id == postPolicyId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPostsPolicy> GetAccountPostPolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                return (await _dbContext.PostPolicies.Where(e => e.PolicyId == policyId)
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

        public async Task<AccountPostsPolicy> UpdateAccountPostPolicyAsync(AccountPostsPolicy postPolicy)
        {
            try
            {
                var oldPostPolicy = await GetAccountPostPolicyByIdAsync(postPolicy.Id);
                oldPostPolicy.PolicyId = postPolicy.PolicyId;
                _dbContext.PostPolicies.Update(oldPostPolicy);
                await SaveChangesAsync();
                return oldPostPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

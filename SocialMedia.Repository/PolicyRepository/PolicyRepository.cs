

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PolicyRepository
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<Policy> AddPolicyAsync(Policy policy)
        {
            try
            {
                await _dbContext.Policies.AddAsync(policy);
                await SaveChangesAsync();
                return policy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Policy> DeletePolicyByIdAsync(Guid policyId)
        {
            try
            {
                var policy = await GetPolicyByIdAsync(policyId);
                _dbContext.Remove(policy);
                await SaveChangesAsync();
                return policy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Policy>> GetPoliciesAsync()
        {
            try
            {
                return await _dbContext.Policies.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Policy> GetPolicyByIdAsync(Guid policyId)
        {
            try
            {
                return (await _dbContext.Policies.Where(e => e.Id == policyId).FirstOrDefaultAsync())!;
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

        public async Task<Policy> UpdatePolicyAsync(Policy policy)
        {
            try
            {
                var policy1 = await GetPolicyByIdAsync(policy.Id);
                policy1.PolicyType = policy.PolicyType;
                await SaveChangesAsync();
                return policy1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

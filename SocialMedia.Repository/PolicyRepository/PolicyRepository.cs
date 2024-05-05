

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
                policy.PolicyType = policy.PolicyType.ToUpper();
                await _dbContext.Policies.AddAsync(policy);
                await SaveChangesAsync();
                return policy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Policy> DeletePolicyAsync(string policyIdOrName)
        {
            try
            {
                var policyById = await GetPolicyByIdAsync(policyIdOrName);
                var policyByName = await GetPolicyByNameAsync(policyIdOrName);
                return policyById == null ? policyByName! : policyById;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Policy> DeletePolicyByIdAsync(string policyId)
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

        public async Task<Policy> GetPolicyByIdAsync(string policyId)
        {
            try
            {
                return (await _dbContext.Policies.Where(e => e.Id == policyId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Policy> GetPolicyByNameAsync(string policyName)
        {
            try
            {
                policyName = policyName.ToUpper();
                return (await _dbContext.Policies.Where(e => e.PolicyType == policyName).FirstOrDefaultAsync())!;
            }
            catch(Exception)
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
                var policy1 = await GetPolicyByIdAsync(policy.Id.ToString());
                policy1.PolicyType = policy.PolicyType.ToUpper();
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

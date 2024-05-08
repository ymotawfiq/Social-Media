
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.ReactPolicyRepository
{
    public class ReactPolicyRepository : IReactPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ReactPolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<ReactPolicy> AddReactPolicyAsync(ReactPolicy reactPolicy)
        {
            try
            {
                await _dbContext.ReactPolicies.AddAsync(reactPolicy);
                await SaveChangesAsync();
                return reactPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReactPolicy> DeleteReactPolicyByIdAsync(string reactPolicyId)
        {
            try
            {
                var reactPolicy = await GetReactPolicyByIdAsync(reactPolicyId);
                _dbContext.ReactPolicies.Remove(reactPolicy);
                await SaveChangesAsync();
                return reactPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ReactPolicy>> GetReactPoliciesAsync()
        {
            return await _dbContext.ReactPolicies.Include(e=>e.Policy).ToListAsync();
        }

        public async Task<ReactPolicy> GetReactPolicyByIdAsync(string reactPolicyId)
        {
            return (await _dbContext.ReactPolicies.Where(e => e.Id == reactPolicyId).FirstOrDefaultAsync())!;
        }

        public async Task<ReactPolicy> GetReactPolicyByPolicyIdAsync(string policyId)
        {
            return (await _dbContext.ReactPolicies.Where(e => e.PolicyId == policyId).FirstOrDefaultAsync())!;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ReactPolicy> UpdateReactPolicyAsync(ReactPolicy reactPolicy)
        {
            try
            {
                var reactPolicy1 = await GetReactPolicyByIdAsync(reactPolicy.Id);
                reactPolicy1.PolicyId = reactPolicy.PolicyId;
                await SaveChangesAsync();
                return reactPolicy1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

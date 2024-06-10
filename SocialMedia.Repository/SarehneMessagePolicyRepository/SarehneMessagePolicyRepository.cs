

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using Twilio.Jwt.Taskrouter;

namespace SocialMedia.Repository.SarehneMessagePolicyRepository
{
    public class SarehneMessagePolicyRepository : ISarehneMessagePolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SarehneMessagePolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<SarehneMessagePolicy> AddPolicyAsync(SarehneMessagePolicy sarehneMessagePolicy)
        {
            try
            {
                await _dbContext.SarehneMessagePolicies.AddAsync(sarehneMessagePolicy);
                await SaveChangesAsync();
                return sarehneMessagePolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SarehneMessagePolicy> DeletePolicyByIdAsync(string sarehneMessagePolicyId)
        {
            try
            {
                var messagePolicy = await GetPolicyByIdAsync(sarehneMessagePolicyId);
                _dbContext.SarehneMessagePolicies.Remove(messagePolicy);
                await SaveChangesAsync();
                return messagePolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SarehneMessagePolicy> DeletePolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                var messagePolicy = await GetPolicyByPolicyIdAsync(policyId);
                _dbContext.SarehneMessagePolicies.Remove(messagePolicy);
                await SaveChangesAsync();
                return messagePolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SarehneMessagePolicy>> GetPoliciesAsync()
        {
            try
            {
                return await _dbContext.SarehneMessagePolicies.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SarehneMessagePolicy> GetPolicyByIdAsync(string sarehneMessagePolicyId)
        {
            try
            {
                return (await _dbContext.SarehneMessagePolicies.Where(e => e.Id == sarehneMessagePolicyId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SarehneMessagePolicy> GetPolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                return (await _dbContext.SarehneMessagePolicies.Where(e => e.PolicyId == policyId)
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

        public async Task<SarehneMessagePolicy> UpdatePolicyAsync(SarehneMessagePolicy sarehneMessagePolicy)
        {
            try
            {
                var messagePolicy = await GetPolicyByIdAsync(sarehneMessagePolicy.Id);
                messagePolicy.PolicyId = sarehneMessagePolicy.PolicyId;
                await SaveChangesAsync();
                return messagePolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

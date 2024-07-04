

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.PolicyRepository
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Policy> AddAsync(Policy t)
        {
            try
            {
                t.PolicyType = t.PolicyType.ToUpper();
                await _dbContext.Policies.AddAsync(t);
                await SaveChangesAsync();
                return new Policy
                {
                    Id = t.Id,
                    PolicyType = t.PolicyType
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddRangeAsync(List<string> policies)
        {
            if((await _dbContext.Users.ToListAsync()).ToList().Count == 0
                || await _dbContext.Users.ToListAsync() == null)
            {
                for (int i = 0; i < policies.Count; i++)
                {
                    await _dbContext.Policies.AddAsync(new Policy
                    {
                        Id = Guid.NewGuid().ToString(),
                        PolicyType = policies[i].ToUpper()
                    });
                }
                return true;
            }
            return false;
        }

        public async Task<Policy> DeleteByIdAsync(string id)
        {
            try
            {
                var policy = await GetByIdAsync(id);
                _dbContext.Remove(policy);
                await SaveChangesAsync();
                return policy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Policy>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Policies.Select(e => new Policy
                {
                    PolicyType = e.PolicyType,
                    Id = e.Id
                }).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Policy> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.Policies.Select(e => new Policy
                {
                    PolicyType = e.PolicyType,
                    Id = e.Id
                }).Where(e => e.Id == id)
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
                return (await _dbContext.Policies.Select(e => new Policy
                {
                    PolicyType = e.PolicyType,
                    Id = e.Id
                }).Where(e => e.PolicyType == policyName).FirstOrDefaultAsync())!;
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

        public async Task<Policy> UpdateAsync(Policy t)
        {
            try
            {
                var policy1 = await GetByIdAsync(t.Id);
                policy1.PolicyType = t.PolicyType.ToUpper();
                await SaveChangesAsync();
                return new Policy
                {
                    Id = t.Id,
                    PolicyType = t.PolicyType
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

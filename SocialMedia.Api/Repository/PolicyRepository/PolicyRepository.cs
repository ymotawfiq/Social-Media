

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

        public async Task<int> AddRangeAsync()
        {
            if((await _dbContext.Users.ToListAsync()).ToList().Count == 0
                || await _dbContext.Users.ToListAsync() == null)
            {
                await _dbContext.Policies.AddRangeAsync(new List<Policy>{
                    new Policy{Id=Guid.NewGuid().ToString(), PolicyType="public".ToUpper()},
                    new Policy{Id=Guid.NewGuid().ToString(), PolicyType="private".ToUpper()},
                    new Policy{Id=Guid.NewGuid().ToString(), PolicyType="friends only".ToUpper()},
                    new Policy{Id=Guid.NewGuid().ToString(), PolicyType="friends of friends".ToUpper()},
                });
                await _dbContext.SaveChangesAsync();
                if((await _dbContext.Policies.ToListAsync()).ToList().Count>0)
                    return 1;
                return -1;
            }
            return 0;
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

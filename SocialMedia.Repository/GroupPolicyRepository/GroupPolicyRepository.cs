

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using Twilio.Jwt.Taskrouter;

namespace SocialMedia.Repository.GroupPolicyRepository
{
    public class GroupPolicyRepository : IGroupPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupPolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<GroupPolicy> AddGroupPolicyAsync(GroupPolicy groupPolicy)
        {
            try
            {
                await _dbContext.GroupPolicies.AddAsync(groupPolicy);
                await SaveChangesAsync();
                return groupPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupPolicy> DeleteGroupPolicyByIdAsync(string groupPolicyId)
        {
            try
            {
                var groupPolicy = (GroupPolicy)await GetGroupPolicyByIdAsync(groupPolicyId);
                _dbContext.GroupPolicies.Remove(groupPolicy);
                await SaveChangesAsync();
                return groupPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupPolicy> DeleteGroupPolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                var groupPolicy = await GetGroupPolicyByPolicyIdAsync(policyId);
                _dbContext.GroupPolicies.Remove(groupPolicy);
                await SaveChangesAsync();
                return groupPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GroupPolicy>> GetGroupPoliciesAsync()
        {
            try
            {
                return await _dbContext.GroupPolicies.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupPolicy> GetGroupPolicyByIdAsync(string groupPolicyId)
        {
            try
            {
                return (await _dbContext.GroupPolicies.Where(e => e.Id == groupPolicyId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupPolicy> GetGroupPolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                return (await _dbContext.GroupPolicies.Where(e => e.PolicyId == policyId)
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

        public async Task<GroupPolicy> UpdateGroupPolicyAsync(GroupPolicy groupPolicy)
        {
            try
            {
                var existGrouPolicy = await GetGroupPolicyByIdAsync(groupPolicy.Id);
                existGrouPolicy.PolicyId = groupPolicy.PolicyId;
                await SaveChangesAsync();
                return existGrouPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

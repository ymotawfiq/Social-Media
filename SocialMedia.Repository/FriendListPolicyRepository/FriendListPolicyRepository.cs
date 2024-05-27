

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.FriendListPolicyRepository
{
    public class FriendListPolicyRepository : IFriendListPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FriendListPolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<FriendListPolicy> AddFriendListPolicyAsync(FriendListPolicy friendListPolicy)
        {
            try
            {
                await _dbContext.FriendListPolicies.AddAsync(friendListPolicy);
                await SaveChangesAsync();
                return friendListPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendListPolicy> DeleteFriendListPolicyByIdAsync(string id)
        {
            var friendListPolicy = await GetFriendListPolicyByIdAsync(id);
            _dbContext.FriendListPolicies.Remove(friendListPolicy);
            await SaveChangesAsync();
            return friendListPolicy;
        }

        public async Task<IEnumerable<FriendListPolicy>> GetFriendListPoliciesAsync()
        {
            try
            {
                return await _dbContext.FriendListPolicies.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendListPolicy> GetFriendListPolicyByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.FriendListPolicies.Where(e => e.Id == id)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendListPolicy> GetFriendListPolicyByPolicyIdAsync(string policyId)
        {
            return (await _dbContext.FriendListPolicies.Where(e => e.PolicyId == policyId)
                .FirstOrDefaultAsync())!;
        }


        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<FriendListPolicy> UpdateFriendListPolicyAsync(FriendListPolicy friendListPolicy)
        {
            try
            {
                var oldFriendListPolicy = await GetFriendListPolicyByIdAsync
                    (friendListPolicy.Id);
                oldFriendListPolicy.PolicyId = friendListPolicy.PolicyId;
                _dbContext.FriendListPolicies.Update(oldFriendListPolicy);
                await SaveChangesAsync();
                return oldFriendListPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

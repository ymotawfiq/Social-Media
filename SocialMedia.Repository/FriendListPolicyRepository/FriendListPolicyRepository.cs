

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

        public async Task<FriendListPolicy> GetFriendListPolicyByUserIdAsync(string userId)
        {
            try
            {
                return (await _dbContext.FriendListPolicies.Where(e => e.UserId == userId)
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

        public async Task<FriendListPolicy> UpdateFriendListPolicyAsync(FriendListPolicy friendListPolicy)
        {
            try
            {
                var oldFriendListPolicy = await GetFriendListPolicyByUserIdAsync(friendListPolicy.UserId);
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

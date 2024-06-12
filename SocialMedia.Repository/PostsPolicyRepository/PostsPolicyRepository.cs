

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostsPolicyRepository
{
    public class PostsPolicyRepository : IPostsPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostsPolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<PostsPolicy> AddPostPolicyAsync(PostsPolicy postPolicy)
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

        public async Task<PostsPolicy> DeletePostPolicyByIdAsync(string postPolicyId)
        {
            try
            {
                var postPolicy = await GetPostPolicyByIdAsync(postPolicyId);
                _dbContext.PostPolicies.Remove(postPolicy);
                await SaveChangesAsync();
                return postPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostsPolicy> DeletePostPolicyByPolicyIdAsync(string policyId)
        {
            try
            {
                var postPolicy = await GetPostPolicyByPolicyIdAsync(policyId);
                _dbContext.PostPolicies.Remove(postPolicy);
                await SaveChangesAsync();
                return postPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostsPolicy>> GetPostPoliciesAsync()
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

        public async Task<PostsPolicy> GetPostPolicyByIdAsync(string postPolicyId)
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

        public async Task<PostsPolicy> GetPostPolicyByPolicyIdAsync(string policyId)
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

        public async Task<PostsPolicy> UpdatePostPolicyAsync(PostsPolicy postPolicy)
        {
            try
            {
                var oldPostPolicy = await GetPostPolicyByIdAsync(postPolicy.Id);
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

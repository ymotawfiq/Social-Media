

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.CommentPolicyRepository
{
    public class CommentPolicyRepository : ICommentPolicyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentPolicyRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<CommentPolicy> AddCommentPolicyAsync(CommentPolicy commentPolicy)
        {
            try
            {
                await _dbContext.CommentPolicies.AddAsync(commentPolicy);
                await SaveChangesAsync();
                return commentPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommentPolicy> DeleteCommentPolicyByIdAsync(string commentPolicyId)
        {
            try
            {
                var commentPolicy = await GetCommentPolicyByIdAsync(commentPolicyId);
                _dbContext.CommentPolicies.Remove(commentPolicy);
                await SaveChangesAsync();
                return commentPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CommentPolicy>> GetCommentPoliciesAsync()
        {
            return await _dbContext.CommentPolicies.ToListAsync();
        }

        public async Task<CommentPolicy> GetCommentPolicyByIdAsync(string commentPolicyId)
        {
            return (await _dbContext.CommentPolicies.Where(e => e.Id == commentPolicyId).FirstOrDefaultAsync())!;
        }

        public async Task<CommentPolicy> GetCommentPolicyByPolicyIdAsync(string policyId)
        {
            return (await _dbContext.CommentPolicies.Where(e => e.PolicyId == policyId).FirstOrDefaultAsync())!;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommentPolicy> UpdateCommentPolicyAsync(CommentPolicy commentPolicy)
        {
            try
            {
                var commentPolicy1 = await GetCommentPolicyByIdAsync(commentPolicy.Id);
                commentPolicy1.PolicyId = commentPolicy.PolicyId;
                await SaveChangesAsync();
                return commentPolicy1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

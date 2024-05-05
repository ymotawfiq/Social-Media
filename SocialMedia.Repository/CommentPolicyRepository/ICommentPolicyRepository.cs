

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.CommentPolicyRepository
{
    public interface ICommentPolicyRepository
    {


        Task<CommentPolicy> AddCommentPolicyAsync(CommentPolicy commentPolicy);
        Task<CommentPolicy> UpdateCommentPolicyAsync(CommentPolicy commentPolicy);
        Task<CommentPolicy> DeleteCommentPolicyByIdAsync(string commentPolicyId);
        Task<CommentPolicy> GetCommentPolicyByIdAsync(string commentPolicyId);
        Task<CommentPolicy> GetCommentPolicyByPolicyIdAsync(string policyId);
        Task<IEnumerable<CommentPolicy>> GetCommentPoliciesAsync();
        Task SaveChangesAsync();

    }
}

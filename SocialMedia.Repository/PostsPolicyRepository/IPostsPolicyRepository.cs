

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostsPolicyRepository
{
    public interface IPostsPolicyRepository
    {
        Task<PostsPolicy> AddPostPolicyAsync(PostsPolicy postPolicy);
        Task<PostsPolicy> UpdatePostPolicyAsync(PostsPolicy postPolicy);
        Task<PostsPolicy> DeletePostPolicyByIdAsync(string postPolicyId);
        Task<PostsPolicy> DeletePostPolicyByPolicyIdAsync(string policyId);
        Task<PostsPolicy> GetPostPolicyByIdAsync(string postPolicyId);
        Task<PostsPolicy> GetPostPolicyByPolicyIdAsync(string policyId);
        Task<IEnumerable<PostsPolicy>> GetPostPoliciesAsync();
        Task SaveChangesAsync();
    }
}

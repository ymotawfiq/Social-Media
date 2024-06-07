

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupPolicyRepository
{
    public interface IGroupPolicyRepository
    {
        Task<GroupPolicy> AddGroupPolicyAsync(GroupPolicy groupPolicy);
        Task<GroupPolicy> UpdateGroupPolicyAsync(GroupPolicy groupPolicy);
        Task<GroupPolicy> DeleteGroupPolicyByIdAsync(string groupPolicyId);
        Task<GroupPolicy> DeleteGroupPolicyByPolicyIdAsync(string policyId);
        Task<GroupPolicy> GetGroupPolicyByIdAsync(string groupPolicyId);
        Task<GroupPolicy> GetGroupPolicyByPolicyIdAsync(string policyId);
        Task<IEnumerable<GroupPolicy>> GetGroupPoliciesAsync();
        Task SaveChangesAsync();
    }
}

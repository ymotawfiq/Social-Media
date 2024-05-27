

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.FriendListPolicyRepository
{
    public interface IFriendListPolicyRepository
    {
        Task<FriendListPolicy> AddFriendListPolicyAsync(FriendListPolicy friendListPolicy);
        Task<FriendListPolicy> UpdateFriendListPolicyAsync(FriendListPolicy friendListPolicy);
        Task<FriendListPolicy> GetFriendListPolicyByIdAsync(string id);
        Task<FriendListPolicy> DeleteFriendListPolicyByIdAsync(string id);
        Task<FriendListPolicy> GetFriendListPolicyByPolicyIdAsync(string policyId);
        Task<IEnumerable<FriendListPolicy>> GetFriendListPoliciesAsync();
        Task SaveChangesAsync();

    }
}

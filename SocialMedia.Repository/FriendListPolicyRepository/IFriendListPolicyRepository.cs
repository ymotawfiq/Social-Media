

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.FriendListPolicyRepository
{
    public interface IFriendListPolicyRepository
    {
        Task<FriendListPolicy> AddFriendListPolicyAsync(FriendListPolicy friendListPolicy);
        Task<FriendListPolicy> UpdateFriendListPolicyAsync(FriendListPolicy friendListPolicy);
        Task<FriendListPolicy> GetFriendListPolicyByUserIdAsync(string userId);
        Task<FriendListPolicy> GetFriendListPolicyByIdAsync(string id);
        Task SaveChangesAsync();

    }
}

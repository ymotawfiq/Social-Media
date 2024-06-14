

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.FriendRequestRepository
{
    public interface IFriendRequestRepository
    {
        Task<FriendRequest> AddFriendRequestAsync(FriendRequest friendRequest);
        Task<FriendRequest> UpdateFriendRequestAsync(FriendRequest friendRequest);
        Task<FriendRequest> GetFriendRequestByIdAsync(string friendRequestId);
        Task<FriendRequest> GetFriendRequestByUserAndPersonIdAsync(string userId, string personId);
        Task<FriendRequest> DeleteFriendRequestByAsync(string friendRequestId);
        Task<IEnumerable<FriendRequest>> GetAllFriendRequestsAsync();
        Task<IEnumerable<FriendRequest>> GetSentFriendRequestsByUserIdAsync(string userId);
        Task<IEnumerable<FriendRequest>> GetReceivedFriendRequestsByUserIdAsync(string userId);
        Task SaveChangesAsync();
    }
}

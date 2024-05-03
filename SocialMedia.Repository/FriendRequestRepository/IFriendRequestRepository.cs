

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.FriendRequestRepository
{
    public interface IFriendRequestRepository
    {
        Task<FriendRequest> AddFriendRequestAsync(FriendRequest friendRequest);
        Task<FriendRequest> UpdateFriendRequestAsync(FriendRequest friendRequest);
        Task<FriendRequest> GetFriendRequestByIdAsync(Guid friendRequestId);
        Task<FriendRequest> GetFriendRequestByUserAndPersonIdAsync(string userId, string personId);
        Task<FriendRequest> DeleteFriendRequestByAsync(Guid friendRequestId);
        Task<IEnumerable<FriendRequest>> GetAllFriendRequestsAsync();
        Task<IEnumerable<FriendRequest>> GetAllFriendRequestsByUserIdAsync(string userId);
        Task<IEnumerable<FriendRequest>> GetAllFriendRequestsByUserNameAsync(string userName);
        Task SaveChangesAsync();
    }
}

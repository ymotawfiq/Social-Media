

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.FriendRequestRepository
{
    public interface IFriendRequestRepository : ICrud<FriendRequest>
    {
        Task<FriendRequest> GetByUserAndPersonIdAsync(string userId, string personId);
        Task<IEnumerable<FriendRequest>> GetSentFriendRequestsByUserIdAsync(string userId);
        Task<IEnumerable<FriendRequest>> GetReceivedFriendRequestsByUserIdAsync(string userId);
    }
}

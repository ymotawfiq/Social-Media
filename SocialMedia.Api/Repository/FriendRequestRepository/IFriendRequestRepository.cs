

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.FriendRequestRepository
{
    public interface IFriendRequestRepository : ICrud<FriendRequest>
    {
        Task<FriendRequest> GetByUserAndPersonIdAsync(string userId, string personId);
        Task<IEnumerable<FriendRequest>> GetSentFriendRequestsByUserIdAsync(string userId);
        Task<IEnumerable<FriendRequest>> GetReceivedFriendRequestsByUserIdAsync(string userId);
    }
}



using SocialMedia.Data.Models;

namespace SocialMedia.Repository.FriendsRepository
{
    public interface IFriendsRepository
    {
        Task<Friend> AddFriendAsync(Friend friend);
        Task<Friend> DeleteFriendAsync(string userId, string friendId);
        Task<Friend> GetFriendByUserAndFriendIdAsync(string userId, string friendId);
        Task<IEnumerable<Friend>> GetAllUserFriendsAsync(string userId);
        Task<IEnumerable<IEnumerable<Friend>>> GetUserFriendsOfFriendsAsync(string userId);
        Task <IEnumerable<Friend>> GetSharedFriendsAsync(string userId, string routeUserId);
        Task SaveChangesAsync();

    }
}

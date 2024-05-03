

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.FriendsRepository
{
    public interface IFriendsRepository
    {
        Task<Friend> AddFriendAsync(Friend friend);
        Task<Friend> DeleteFriendAsync(string userId, string friendId);
        Task<Friend> GetFriendByUserAndFriendIdAsync(string userId, string friendId);
        Task<IEnumerable<Friend>> GetAllUserFriendsAsync(string userId);
        Task SaveChangesAsync();

    }
}

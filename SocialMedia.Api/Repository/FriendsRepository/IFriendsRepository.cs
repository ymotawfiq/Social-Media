

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.FriendsRepository
{
    public interface IFriendsRepository : ICrud<Friend>
    {
        Task<Friend> DeleteFriendAsync(string userId, string friendId);
        Task<Friend> GetByUserAndFriendIdAsync(string userId, string friendId);
        Task<IEnumerable<Friend>> GetAllUserFriendsAsync(string userId);
        Task<IEnumerable<IEnumerable<Friend>>> GetUserFriendsOfFriendsAsync(string userId);
        Task <IEnumerable<Friend>> GetSharedFriendsAsync(string userId, string routeUserId);

    }
}

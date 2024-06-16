

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.UserChatRepository
{
    public interface IUserChatRepository : ICrud<UserChat>
    {
        Task<UserChat> GetByUser1AndUser2Async(string user1Id, string user2Id);
        Task<IEnumerable<UserChat>> GetUserChatsAsync(string userId);
    }
}

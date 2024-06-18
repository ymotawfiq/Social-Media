

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.UserChatRepository
{
    public interface IUserChatRepository : ICrud<UserChat>
    {
        Task<UserChat> GetByUser1AndUser2Async(string user1Id, string user2Id);
        Task<IEnumerable<UserChat>> GetUserChatsAsync(string userId);
    }
}

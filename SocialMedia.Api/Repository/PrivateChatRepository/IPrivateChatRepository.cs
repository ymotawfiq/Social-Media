using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.PrivateChatRepository
{
    public interface IPrivateChatRepository : ICrud<PrivateChat>
    {
        Task<PrivateChat> GetByChatIdAsync(string chatId);
        Task<IEnumerable<PrivateChat>> GetReceivedChatRequestsAsync(string user2Id);
        Task<IEnumerable<PrivateChat>> GetSentChatRequestsAsync(string user1Id);
        Task<PrivateChat> GetByMemberAndChatIdAsync(string chatId, string memberId);
        Task<PrivateChat> GetByMember1AndMember2IdAsync(string member1Id, string member2Id);
        Task<IEnumerable<PrivateChat>> GetUserChatsAsync(string userId);
    }
}

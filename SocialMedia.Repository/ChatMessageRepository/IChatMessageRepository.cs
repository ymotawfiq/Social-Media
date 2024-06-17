

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.ChatMessageRepository
{
    public interface IChatMessageRepository : ICrud<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetUserSentMessagesAsync(SiteUser user, string chatId);
        Task<bool> IsChatEmptyAsync(SiteUser user, string chatId);
    }
}

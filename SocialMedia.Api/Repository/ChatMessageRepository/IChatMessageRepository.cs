

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ChatMessageRepository
{
    public interface IChatMessageRepository : ICrud<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetUserSentMessagesAsync(SiteUser user, string chatId);
        Task<bool> IsChatEmptyAsync(SiteUser user, string chatId);
    }
}

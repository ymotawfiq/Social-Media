using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ChatMessageRepository
{
    public interface IChatMessageRepository : ICrud<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetMessagesByChatIdAsync(string chatId);
        Task<ChatMessage> GetByChatIdAndMessageAsync(string chatId, string messageId);
    }
}

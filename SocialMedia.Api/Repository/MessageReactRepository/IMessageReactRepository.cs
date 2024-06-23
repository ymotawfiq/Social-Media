using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.MessageReactRepository
{
    public interface IMessageReactRepository : ICrud<MessageReact>
    {
        Task<IEnumerable<MessageReact>> GetMessageReactsByMessageIdAsync(string messageId);
        Task<MessageReact> GetMessageReactByMessageAndUserIdAsync(string messageId, string userId);
    }
}

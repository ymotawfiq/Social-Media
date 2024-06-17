

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.MessageReactRepository
{
    public interface IMessageReactRepository : ICrud<MessageReact>
    {
        Task<IEnumerable<MessageReact>> GetMessageReactsAsync(string messageId);
    }
}

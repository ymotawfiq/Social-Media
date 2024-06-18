

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.MessageReactRepository
{
    public interface IMessageReactRepository : ICrud<MessageReact>
    {
        Task<IEnumerable<MessageReact>> GetMessageReactsAsync(string messageId);
    }
}

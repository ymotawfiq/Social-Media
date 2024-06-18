

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ArchievedChatRepository
{
    public interface IArchievedChatRepository : ICrud<ArchievedChat>
    {
        Task<ArchievedChat> GetByChatAndUserIdAsync(string chatId, string userId);
        Task<IEnumerable<ArchievedChat>> GetUserArchievedChatsAsync(string userId);
    }
}

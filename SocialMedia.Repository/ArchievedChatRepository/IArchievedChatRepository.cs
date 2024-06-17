

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.ArchievedChatRepository
{
    public interface IArchievedChatRepository : ICrud<ArchievedChat>
    {
        Task<ArchievedChat> GetByChatAndUserIdAsync(string chatId, string userId);
        Task<IEnumerable<ArchievedChat>> GetUserArchievedChatsAsync(string userId);
    }
}

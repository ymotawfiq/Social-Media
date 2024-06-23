using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ArchievedChatRepository
{
    public interface IArchievedChatRepository : ICrud<ArchievedChat>
    {
        Task<IEnumerable<ArchievedChat>> GetAllByUserIdAsync(string userId);
        Task<ArchievedChat> GetByUserAndChatIdAsync(string userId, string chatId);
    }
}

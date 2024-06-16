

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.ChatRequestRepository
{
    public interface IChatRequestRepository : ICrud<ChatRequest>
    {
        Task<IEnumerable<ChatRequest>> GetReceivedChatRequestsAsync(SiteUser user);
        Task<IEnumerable<ChatRequest>> GetSentChatRequestsAsync(SiteUser user);
        Task<ChatRequest> GetChatRequestAsync(SiteUser user1, SiteUser user2);
    }
}

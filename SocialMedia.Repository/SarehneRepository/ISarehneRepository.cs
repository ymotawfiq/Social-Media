

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.SarehneRepository
{
    public interface ISarehneRepository : ICrud<SarehneMessage>
    {
        Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId);
        Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId, string policyId);
    }
}

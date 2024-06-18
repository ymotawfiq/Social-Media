

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.SarehneRepository
{
    public interface ISarehneRepository : ICrud<SarehneMessage>
    {
        Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId);
        Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId, string policyId);
    }
}

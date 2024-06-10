

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.SarehneRepository
{
    public interface ISarehneRepository
    {
        Task<SarehneMessage> SendMessageAsync(SarehneMessage sarehneMessage);
        Task<SarehneMessage> UpdateMessagePolicyAsync(SarehneMessage sarehneMessage);
        Task<SarehneMessage> DeleteMessageAsync(string sarehneMessageId);
        Task<SarehneMessage> GetMessageAsync(string sarehneMessageId);
        Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId);
        Task<IEnumerable<SarehneMessage>> GetMessagesAsync(string userId, string policyId);
        Task SaveChangesAsync();
    }
}

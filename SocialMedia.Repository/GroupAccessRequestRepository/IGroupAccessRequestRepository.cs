

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.GroupAccessRequestRepository
{
    public interface IGroupAccessRequestRepository : ICrud<GroupAccessRequest>
    {
        Task<GroupAccessRequest> DeleteGroupAccessRequestAsync(string groupId, string userId);
        Task<GroupAccessRequest> GetGroupAccessRequestAsync(string groupId, string userId);
        Task<IEnumerable<GroupAccessRequest>> GetGroupAccessRequestsByUserIdAsync(string userId);
        Task<IEnumerable<GroupAccessRequest>> GetGroupAccessRequestsByGroupIdAsync(string groupId);
    }
}

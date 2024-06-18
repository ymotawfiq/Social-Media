

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.GroupAccessRequestRepository
{
    public interface IGroupAccessRequestRepository : ICrud<GroupAccessRequest>
    {
        Task<GroupAccessRequest> DeleteGroupAccessRequestAsync(string groupId, string userId);
        Task<GroupAccessRequest> GetGroupAccessRequestAsync(string groupId, string userId);
        Task<IEnumerable<GroupAccessRequest>> GetGroupAccessRequestsByUserIdAsync(string userId);
        Task<IEnumerable<GroupAccessRequest>> GetGroupAccessRequestsByGroupIdAsync(string groupId);
    }
}

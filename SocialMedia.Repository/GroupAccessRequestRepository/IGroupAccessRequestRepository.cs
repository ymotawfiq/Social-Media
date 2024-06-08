

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupAccessRequestRepository
{
    public interface IGroupAccessRequestRepository
    {
        Task<GroupAccessRequest> AddGroupAccessRequestAsync(GroupAccessRequest request);
        Task<GroupAccessRequest> DeleteGroupAccessRequestByIdAsync(string groupAccessRequestId);
        Task<GroupAccessRequest> DeleteGroupAccessRequestAsync(string groupId, string userId);
        Task<GroupAccessRequest> GetGroupAccessRequestAsync(string groupId, string userId);
        Task<GroupAccessRequest> GetGroupAccessRequestByIdAsync(string groupAccessRequestId);
        Task<IEnumerable<GroupAccessRequest>> GetGroupAccessRequestsByUserIdAsync(string userId);
        Task<IEnumerable<GroupAccessRequest>> GetGroupAccessRequestsByGroupIdAsync(string groupId);
        Task SaveChangesAsync();
    }
}

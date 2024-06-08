


using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupRepository
{
    public interface IGroupRepository
    {
        Task<Group> AddGroupAsync(Group group);
        Task<Group> UpdateGroupAsync(Group group);
        Task<Group> GetGroupByIdAsync(string groupId);
        Task<Group> DeleteGroupByIdAsync(string groupId);
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<IEnumerable<Group>> GetAllGroupsByUserIdAsync(string userId);
        Task SaveChangesAsync();
    }
}

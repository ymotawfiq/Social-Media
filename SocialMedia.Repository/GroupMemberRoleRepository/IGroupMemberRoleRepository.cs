

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupMemberRoleRepository
{
    public interface IGroupMemberRoleRepository
    {
        Task<GroupMemberRole> AddGroupMemberRoleAsync(GroupMemberRole groupMemberRole);
        Task<GroupMemberRole> DeleteGroupMemberRoleAsync(string groupMemberRoleId);
        Task<GroupMemberRole> GetGroupMemberRoleAsync(string groupMemberRoleId);
        Task<GroupMemberRole> DeleteGroupMemberRoleAsync(string groupMemberId, string roleId);
        Task<GroupMemberRole> GetGroupMemberRoleAsync(string groupMemberId, string roleId);
        Task<IEnumerable<GroupMemberRole>> GetMemberRolesAsync(string groupMemberId);
        Task SaveChangesAsync();
    }
}

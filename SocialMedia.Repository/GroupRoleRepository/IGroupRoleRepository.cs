

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupRoleRepository
{
    public interface IGroupRoleRepository
    {
        Task<GroupRole> AddGroupRoleAsync(GroupRole groupRole);
        Task<GroupRole> UpdateGroupRoleAsync(GroupRole groupRole);
        Task<GroupRole> GetGroupRoleByIdAsync(string groupRoleId);
        Task<GroupRole> GetGroupRoleByRoleNameAsync(string groupRoleName);
        Task<GroupRole> DeleteGroupRoleByIdAsync(string groupRoleId);
        Task<GroupRole> DeleteGroupRoleByRoleNameAsync(string groupRoleName);
        Task<IEnumerable<GroupRole>> GetGroupRolesAsync();
        Task SaveChangesAsync();
    }
}

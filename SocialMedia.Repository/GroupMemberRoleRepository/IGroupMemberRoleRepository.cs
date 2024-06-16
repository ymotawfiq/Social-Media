

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.GroupMemberRoleRepository
{
    public interface IGroupMemberRoleRepository : ICrud<GroupMemberRole>
    {
        Task<GroupMemberRole> DeleteGroupMemberRoleAsync(string groupMemberId, string roleId);
        Task<GroupMemberRole> GetGroupMemberRoleAsync(string groupMemberId, string roleId);
        Task<IEnumerable<GroupMemberRole>> GetMemberRolesAsync(string groupMemberId);
    }
}

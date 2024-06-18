

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.GroupMemberRoleRepository
{
    public interface IGroupMemberRoleRepository : ICrud<GroupMemberRole>
    {
        Task<GroupMemberRole> DeleteGroupMemberRoleAsync(string groupMemberId, string roleId);
        Task<GroupMemberRole> GetGroupMemberRoleAsync(string groupMemberId, string roleId);
        Task<IEnumerable<GroupMemberRole>> GetMemberRolesAsync(string groupMemberId);
    }
}


using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupMemberRoleRepository
{
    public interface IGroupMemberRoleRepository
    {
        Task<GroupMemberRole> AddGroupMemberRoleAsync(GroupMemberRole groupMember);
        Task<GroupMemberRole> DeleteGroupMemberRoleAsync(string groupMemberRoleId);
    }
}

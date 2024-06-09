

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupMemberRepository
{
    public interface IGroupMemberRepository
    {
        Task<GroupMember> AddGroupMemberAsync(GroupMember groupMember);
        Task<GroupMember> DeleteGroupMemberAsync(string userId, string groupId);
        Task<GroupMember> GetGroupMemberAsync(string userId, string groupId);
        Task<GroupMember> GetGroupMemberAsync(string groupMemberId);
        Task<GroupMember> DeleteGroupMemberAsync(string groupMemberId);
        Task<IEnumerable<GroupMember>> GetGroupMembersAsync(string groupId);
        Task<IEnumerable<GroupMember>> GetUserGroupsAsync(string userId);
        Task<IEnumerable<GroupMember>> GetUserJoinedGroupsAsync(string userId);
        Task SaveChangesAsync();
    }
}

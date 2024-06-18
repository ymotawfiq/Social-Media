

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.GroupMemberRepository
{
    public interface IGroupMemberRepository : ICrud<GroupMember>
    {
        Task<GroupMember> DeleteGroupMemberAsync(string userId, string groupId);
        Task<GroupMember> GetGroupMemberAsync(string userId, string groupId);
        Task<IEnumerable<GroupMember>> GetGroupMembersAsync(string groupId);
        Task<IEnumerable<GroupMember>> GetUserGroupsAsync(string userId);
        Task<IEnumerable<GroupMember>> GetUserJoinedGroupsAsync(string userId);
    }
}



using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.GroupManager
{
    public interface IGroupManager
    {

        Task<ApiResponse<bool>> IsInRoleAsync(SiteUser user, Group group, string role);
        Task<ApiResponse<bool>> AddToRoleAsync(SiteUser admin, SiteUser user, Group group, string role);
        Task<ApiResponse<bool>> AddToRoleAsync(string groupMemberId, SiteUser user, string role);
        Task<ApiResponse<bool>> RemoveFromRoleAsync(
            SiteUser admin, SiteUser user, Group group, string role);
        Task<ApiResponse<bool>> RemoveFromRoleAsync(string groupMemberId, SiteUser user, string role);
        Task<ApiResponse<IEnumerable<string>>> GetUserRolesAsync(string userId, string groupId);
        Task<ApiResponse<bool>> IsExistInGroupAsync(SiteUser user, Group group);
        Task<ApiResponse<bool>> AcceptGroupRequestAsync(string requestId, SiteUser user);
        Task<ApiResponse<bool>> RemoveFromGroupAsync(SiteUser admin, SiteUser user, Group group);
        Task<ApiResponse<bool>> RemoveFromGroupAsync(string groupMemberId, SiteUser user);
        Task<ApiResponse<IEnumerable<GroupAccessRequest>>> GetRequestsAsync(string groupId, SiteUser user);
        Task<ApiResponse<IEnumerable<GroupMember>>> GetGroupMembersAsync(string groupId);
        Task<ApiResponse<IEnumerable<GroupMember>>> GetUserJoinedGroupsAsync(SiteUser currentUser);


    }
}

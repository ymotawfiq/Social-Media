

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.GroupService
{
    public interface IGroupService
    {
        Task<ApiResponse<Group>> AddGroupAsync(AddGroupDto addGroupDto, SiteUser user);
        Task<ApiResponse<Group>> UpdateGroupAsync(UpdateGroupDto updateGroupDto, SiteUser user);
        Task<ApiResponse<Group>> UpdateGroupAsync(
            UpdateExistGroupPolicyDto updateExistGroupPolicyDto, SiteUser user);
        Task<ApiResponse<Group>> GetGroupByIdAsync(string groupId);
        Task<ApiResponse<Group>> DeleteGroupByIdAsync(string groupId, SiteUser user);
        Task<ApiResponse<IEnumerable<Group>>> GetAllGroupsAsync();
        Task<ApiResponse<IEnumerable<Group>>> GetAllGroupsByUserIdAsync(string userId);
    }
}

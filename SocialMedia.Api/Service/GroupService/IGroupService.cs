

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.GroupService
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

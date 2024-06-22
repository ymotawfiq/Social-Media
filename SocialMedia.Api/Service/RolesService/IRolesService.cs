
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;

namespace SocialMedia.Api.Service.RolesService
{
    public interface IRolesService
    {
        Task<ApiResponse<Role>> AddRoleAsync(AddRoleDto addGroupRoleDto);
        Task<ApiResponse<Role>> UpdateRoleAsync(UpdateRoleDto updateGroupRoleDto);
        Task<ApiResponse<Role>> GetRoleByIdAsync(string groupRoleId);
        Task<ApiResponse<Role>> GetRoleByIdOrNameAsync(string groupRoleIdOrName);
        Task<ApiResponse<Role>> GetRoleByRoleNameAsync(string groupRoleName);
        Task<ApiResponse<Role>> DeleteRoleByIdAsync(string groupRoleId);
        Task<ApiResponse<Role>> DeleteRoleByRoleNameAsync(string groupRoleName);
        Task<ApiResponse<IEnumerable<Role>>> GetRolesAsync();
    }
}

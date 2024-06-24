
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;

namespace SocialMedia.Api.Service.RolesService
{
    public interface IRolesService
    {
        Task<ApiResponse<Role>> AddRoleAsync(AddRoleDto addRoleDto);
        Task<ApiResponse<Role>> UpdateRoleAsync(UpdateRoleDto updateRoleDto);
        Task<ApiResponse<Role>> GetRoleByIdAsync(string RoleId);
        Task<ApiResponse<Role>> GetRoleByIdOrNameAsync(string RoleIdOrName);
        Task<ApiResponse<Role>> GetRoleByRoleNameAsync(string RoleName);
        Task<ApiResponse<Role>> DeleteRoleByIdAsync(string RoleId);
        Task<ApiResponse<Role>> DeleteRoleByRoleNameAsync(string RoleName);
        Task<ApiResponse<IEnumerable<Role>>> GetRolesAsync();
    }
}

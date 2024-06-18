
using SocialMedia.Api.Data.DTOs;

namespace SocialMedia.Api.Service.GroupRolesService
{
    public interface IGroupRolesService
    {
        Task<object> AddGroupRoleAsync(AddGroupRoleDto addGroupRoleDto);
        Task<object> UpdateGroupRoleAsync(UpdateGroupRoleDto updateGroupRoleDto);
        Task<object> GetGroupRoleByIdAsync(string groupRoleId);
        Task<object> GetGroupRoleByRoleNameAsync(string groupRoleName);
        Task<object> DeleteGroupRoleByIdAsync(string groupRoleId);
        Task<object> DeleteGroupRoleByRoleNameAsync(string groupRoleName);
        Task<object> GetGroupRolesAsync();
    }
}

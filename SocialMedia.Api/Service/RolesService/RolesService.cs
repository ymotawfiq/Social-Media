

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.RoleRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.RolesService
{
    public class RolesService : IRolesService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly Policies policies = new();
        public RolesService(IRoleRepository _roleRepository)
        {
            this._roleRepository = _roleRepository;
        }

        public async Task<ApiResponse<Role>> AddRoleAsync(AddRoleDto addGroupRoleDto)
        {
            var groupRole = await _roleRepository.GetRoleByRoleNameAsync(addGroupRoleDto.RoleName);
            if (groupRole == null)
            {
                if (policies.GroupRoles.Contains(addGroupRoleDto.RoleName.ToUpper()))
                {
                    var newGroupRole = await _roleRepository.AddAsync(ConvertFromDto
                    .ConvertFromRoleDto_Add(addGroupRoleDto));
                    return StatusCodeReturn<Role>
                        ._201_Created("Role added successfully", newGroupRole);
                }
                return StatusCodeReturn<Role>
                ._403_Forbidden("Invalid role");
            }
            return StatusCodeReturn<Role>
                ._403_Forbidden("Role already exists");
        }

        public async Task<ApiResponse<Role>> DeleteRoleByIdAsync(string groupRoleId)
        {
            var groupRole = await _roleRepository.GetByIdAsync(groupRoleId);
            if (groupRole != null)
            {
                await _roleRepository.DeleteByIdAsync(groupRoleId);
                return StatusCodeReturn<Role>
                    ._200_Success("Role deleted successfully", groupRole);
            }
            return StatusCodeReturn<Role>
                ._404_NotFound("Role not found");
        }

        public async Task<ApiResponse<Role>> DeleteRoleByRoleNameAsync(string groupRoleName)
        {
            var groupRole = await _roleRepository.GetRoleByRoleNameAsync(groupRoleName);
            if (groupRole != null)
            {
                await _roleRepository.DeleteRoleByRoleNameAsync(groupRoleName);
                return StatusCodeReturn<Role>
                    ._200_Success("Role deleted successfully", groupRole);
            }
            return StatusCodeReturn<Role>
                ._404_NotFound("Role not found");
        }

        public async Task<ApiResponse<Role>> GetRoleByIdAsync(string groupRoleId)
        {
            var groupRole = await _roleRepository.GetByIdAsync(groupRoleId);
            if (groupRole != null)
            {
                return StatusCodeReturn<Role>
                    ._200_Success("Role found successfully", groupRole);
            }
            return StatusCodeReturn<Role>
                ._404_NotFound("Role not found");
        }

        public async Task<ApiResponse<Role>> GetRoleByIdOrNameAsync(string groupRoleIdOrName)
        {
            var roleById = await _roleRepository.GetByIdAsync(groupRoleIdOrName);
            var roleByName = await _roleRepository.GetRoleByRoleNameAsync(groupRoleIdOrName);
            if (roleById == null)
            {
                if (roleByName != null)
                {
                    return StatusCodeReturn<Role>
                    ._200_Success("Role found successfully", roleByName);
                }
                return StatusCodeReturn<Role>
                ._404_NotFound("Role not found");
            }
            return StatusCodeReturn<Role>
                    ._200_Success("Role found successfully", roleById);
        }

        public async Task<ApiResponse<Role>> GetRoleByRoleNameAsync(string groupRoleName)
        {
            var groupRole = await _roleRepository.GetRoleByRoleNameAsync(groupRoleName);
            if (groupRole != null)
            {
                return StatusCodeReturn<Role>
                    ._200_Success("Role found successfully", groupRole);
            }
            return StatusCodeReturn<Role>
                ._404_NotFound("Role not found");
        }

        public async Task<ApiResponse<IEnumerable<Role>>> GetRolesAsync()
        {
            var groupRoles = await _roleRepository.GetAllAsync();
            if (groupRoles.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<Role>>
                    ._200_Success("No roles found", groupRoles);
            }
            return StatusCodeReturn<IEnumerable<Role>>
                    ._200_Success("Roles found successfully", groupRoles);
        }

        public async Task<ApiResponse<Role>> UpdateRoleAsync(UpdateRoleDto updateGroupRoleDto)
        {
            var groupRoleById = await _roleRepository.GetByIdAsync(updateGroupRoleDto.Id);
            if (groupRoleById != null)
            {
                var groupRoleByName = await _roleRepository.GetRoleByRoleNameAsync(
                    updateGroupRoleDto.RoleName);
                if (groupRoleByName == null)
                {
                    if (policies.GroupRoles.Contains(updateGroupRoleDto.RoleName.ToUpper()))
                    {
                        var updatedGroupRole = await _roleRepository.UpdateAsync(
                        ConvertFromDto.ConvertFromRoleDto_Update(updateGroupRoleDto));
                        return StatusCodeReturn<Role>
                            ._200_Success("Role updated successfully", updatedGroupRole);
                    }
                    return StatusCodeReturn<Role>
                    ._403_Forbidden("Invalid role");
                }
                return StatusCodeReturn<Role>
                    ._403_Forbidden("Role already exists");
            }
            return StatusCodeReturn<Role>
                    ._404_NotFound("Role not found");
        }
    }
}

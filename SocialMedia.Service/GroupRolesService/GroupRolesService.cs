

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Repository.GroupRoleRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.GroupRolesService
{
    public class GroupRolesService : IGroupRolesService
    {
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly Policies policies = new();
        public GroupRolesService(IGroupRoleRepository _groupRoleRepository)
        {
            this._groupRoleRepository = _groupRoleRepository;
        }

        public async Task<object> AddGroupRoleAsync(AddGroupRoleDto addGroupRoleDto)
        {
            var groupRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(addGroupRoleDto.RoleName);
            if (groupRole == null)
            {
                if (policies.GroupRoles.Contains(addGroupRoleDto.RoleName.ToUpper()))
                {
                    var newGroupRole = await _groupRoleRepository.AddAsync(ConvertFromDto
                    .ConvertFromGroupRoleDto_Add(addGroupRoleDto));
                    return StatusCodeReturn<object>
                        ._201_Created("Group role added successfully", newGroupRole);
                }
                return StatusCodeReturn<object>
                ._403_Forbidden("Invalid role");
            }
            return StatusCodeReturn<object>
                ._403_Forbidden("Group role already exists");
        }

        public async Task<object> DeleteGroupRoleByIdAsync(string groupRoleId)
        {
            var groupRole = await _groupRoleRepository.GetByIdAsync(groupRoleId);
            if (groupRole != null)
            {
                await _groupRoleRepository.DeleteByIdAsync(groupRoleId);
                return StatusCodeReturn<object>
                    ._200_Success("Group role deleted successfully", groupRole);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group role not found");
        }

        public async Task<object> DeleteGroupRoleByRoleNameAsync(string groupRoleName)
        {
            var groupRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(groupRoleName);
            if (groupRole != null)
            {
                await _groupRoleRepository.DeleteGroupRoleByRoleNameAsync(groupRoleName);
                return StatusCodeReturn<object>
                    ._200_Success("Group role deleted successfully", groupRole);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group role not found");
        }

        public async Task<object> GetGroupRoleByIdAsync(string groupRoleId)
        {
            var groupRole = await _groupRoleRepository.GetByIdAsync(groupRoleId);
            if (groupRole != null)
            {
                return StatusCodeReturn<object>
                    ._200_Success("Group role found successfully", groupRole);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group role not found");
        }

        public async Task<object> GetGroupRoleByRoleNameAsync(string groupRoleName)
        {
            var groupRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(groupRoleName);
            if (groupRole != null)
            {
                return StatusCodeReturn<object>
                    ._200_Success("Group role found successfully", groupRole);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group role not found");
        }

        public async Task<object> GetGroupRolesAsync()
        {
            var groupRoles = await _groupRoleRepository.GetAllAsync();
            if (groupRoles.ToList().Count == 0)
            {
                return StatusCodeReturn<object>
                    ._200_Success("No group roles found", groupRoles);
            }
            return StatusCodeReturn<object>
                    ._200_Success("Group roles found successfully", groupRoles);
        }

        public async Task<object> UpdateGroupRoleAsync(UpdateGroupRoleDto updateGroupRoleDto)
        {
            var groupRoleById = await _groupRoleRepository.GetByIdAsync(updateGroupRoleDto.Id);
            if (groupRoleById != null)
            {
                var groupRoleByName = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(
                    updateGroupRoleDto.RoleName);
                if (groupRoleByName == null)
                {
                    if (policies.GroupRoles.Contains(updateGroupRoleDto.RoleName.ToUpper()))
                    {
                        var updatedGroupRole = await _groupRoleRepository.UpdateAsync(
                        ConvertFromDto.ConvertFromGroupRoleDto_Update(updateGroupRoleDto));
                        return StatusCodeReturn<object>
                            ._200_Success("Group role updated successfully", updatedGroupRole);
                    }
                    return StatusCodeReturn<object>
                    ._403_Forbidden("Invalid group role");
                }
                return StatusCodeReturn<object>
                    ._403_Forbidden("Group role already exists");
            }
            return StatusCodeReturn<object>
                    ._404_NotFound("Group role not found");
        }
    }
}



using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Repository.GroupRoleRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.GroupRolesService
{
    public class GroupRolesService : IGroupRolesService
    {
        private readonly IGroupRoleRepository _groupRoleRepository;
        public GroupRolesService(IGroupRoleRepository _groupRoleRepository)
        {
            this._groupRoleRepository = _groupRoleRepository;
        }

        public async Task<object> AddGroupRoleAsync(AddGroupRoleDto addGroupRoleDto)
        {
            var groupRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(addGroupRoleDto.RoleName);
            if (groupRole == null)
            {
                var newGroupRole = await _groupRoleRepository.AddGroupRoleAsync(ConvertFromDto
                    .ConvertFromGroupRoleDto_Add(addGroupRoleDto));
                return StatusCodeReturn<object>
                    ._201_Created("Group role added successfully", newGroupRole);
            }
            return StatusCodeReturn<object>
                ._403_Forbidden("Group role already exists");
        }

        public async Task<object> DeleteGroupRoleByIdAsync(string groupRoleId)
        {
            var groupRole = await _groupRoleRepository.GetGroupRoleByIdAsync(groupRoleId);
            if (groupRole != null)
            {
                await _groupRoleRepository.DeleteGroupRoleByIdAsync(groupRoleId);
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
            var groupRole = await _groupRoleRepository.GetGroupRoleByIdAsync(groupRoleId);
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
            var groupRoles = await _groupRoleRepository.GetGroupRolesAsync();
            if (groupRoles.ToList().Count == 0)
            {
                return StatusCodeReturn<object>
                    ._200_Success("No group roles found", groupRoles);
            }
            return StatusCodeReturn<object>
                    ._200_Success("Group roles not found", groupRoles);
        }

        public async Task<object> UpdateGroupRoleAsync(UpdateGroupRoleDto updateGroupRoleDto)
        {
            var groupRoleById = await _groupRoleRepository.GetGroupRoleByIdAsync(updateGroupRoleDto.Id);
            if (groupRoleById != null)
            {
                var groupRoleByName = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(
                    updateGroupRoleDto.RoleName);
                if (groupRoleByName == null)
                {
                    var updatedGroupRole = await _groupRoleRepository.UpdateGroupRoleAsync(
                        ConvertFromDto.ConvertFromGroupRoleDto_Update(updateGroupRoleDto));
                    return StatusCodeReturn<object>
                        ._200_Success("Group role updated successfully", updatedGroupRole);
                }
                return StatusCodeReturn<object>
                    ._403_Forbidden("Group role already exists");
            }
            return StatusCodeReturn<object>
                    ._404_NotFound("Group role not found");
        }
    }
}

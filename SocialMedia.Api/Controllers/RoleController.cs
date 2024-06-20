using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.RolesService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        public RoleController(IRolesService _rolesService)
        {
            this._rolesService = _rolesService;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("addGroupRole")]
        public async Task<IActionResult> AddGroupRoleAsync([FromBody] AddGroupRoleDto addGroupRoleDto)
        {
            try
            {
                var response = await _rolesService.AddGroupRoleAsync(addGroupRoleDto);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateGroupRole")]
        public async Task<IActionResult> UpdateGroupRoleAsync([FromBody] UpdateGroupRoleDto updateGroupRoleDto)
        {
            try
            {
                var response = await _rolesService.UpdateGroupRoleAsync(updateGroupRoleDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupRoleById/{groupRoleId}")]
        public async Task<IActionResult> GetGroupRoleByIdAsync([FromRoute] string groupRoleId)
        {
            try
            {
                var response = await _rolesService.GetGroupRoleByIdAsync(groupRoleId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupRoleByName/{groupRoleName}")]
        public async Task<IActionResult> GetGroupRoleByNameAsync([FromRoute] string groupRoleName)
        {
            try
            {
                var response = await _rolesService.GetGroupRoleByRoleNameAsync(groupRoleName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupRoleById/{groupRoleId}")]
        public async Task<IActionResult> DeleteGroupRoleByIdAsync([FromRoute] string groupRoleId)
        {
            try
            {
                var response = await _rolesService.DeleteGroupRoleByIdAsync(groupRoleId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupRoleByName/{groupRoleName}")]
        public async Task<IActionResult> DeleteGroupRoleByNameAsync([FromRoute] string groupRoleName)
        {
            try
            {
                var response = await _rolesService.DeleteGroupRoleByRoleNameAsync(groupRoleName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupRoles")]
        public async Task<IActionResult> GetGroupRolesAsync()
        {
            try
            {
                var response = await _rolesService.GetGroupRolesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

    }
}

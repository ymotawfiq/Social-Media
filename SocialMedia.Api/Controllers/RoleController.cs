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
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto addRoleDto)
        {
            try
            {
                var response = await _rolesService.AddRoleAsync(addRoleDto);
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
        public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                var response = await _rolesService.UpdateRoleAsync(updateRoleDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupRoleById/{roleId}")]
        public async Task<IActionResult> GetRoleByIdAsync([FromRoute] string roleId)
        {
            try
            {
                var response = await _rolesService.GetRoleByIdAsync(roleId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupRoleByName/{roleName}")]
        public async Task<IActionResult> GetRoleByNameAsync([FromRoute] string roleName)
        {
            try
            {
                var response = await _rolesService.GetRoleByRoleNameAsync(roleName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupRoleById/{roleId}")]
        public async Task<IActionResult> DeleteRoleByIdAsync([FromRoute] string roleId)
        {
            try
            {
                var response = await _rolesService.DeleteRoleByIdAsync(roleId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupRoleByName/{roleName}")]
        public async Task<IActionResult> DeleteRoleByNameAsync([FromRoute] string roleName)
        {
            try
            {
                var response = await _rolesService.DeleteRoleByRoleNameAsync(roleName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupRoles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            try
            {
                var response = await _rolesService.GetRolesAsync();
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

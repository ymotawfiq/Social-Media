using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.GroupPolicyService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class GroupPolicyController : ControllerBase
    {

        private readonly IGroupPolicyService _groupPolicyService;
        public GroupPolicyController(IGroupPolicyService _groupPolicyService)
        {
            this._groupPolicyService = _groupPolicyService;
        }


        [Authorize(Roles ="Admin")]
        [HttpPost("addGroupPolicy")]
        public async Task<IActionResult> AddGroupPolicyAsync([FromBody] AddGroupPolicyDto addGroupPolicyDto)
        {
            try
            {
                var response = await _groupPolicyService.AddGrouPolicyAsync(addGroupPolicyDto);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateGroupPolicy")]
        public async Task<IActionResult> UpdateGroupPolicyAsync(
            [FromBody] UpdateGroupPolicyDto updateGroupPolicyDto)
        {
            try
            {
                var response = await _groupPolicyService.UpdateGrouPolicyAsync(updateGroupPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }


        [HttpGet("getGroupPolicy/{groupPolicyIdOrPolicyIdOrPolicyName}")]
        public async Task<IActionResult> GetGroupPolicyAsync(
            [FromRoute] string groupPolicyIdOrPolicyIdOrPolicyName)
        {
            try
            {
                var response = await _groupPolicyService.GetGrouPolicyByGroupPolicyIdOrPolicyIdOrNameAsync(
                    groupPolicyIdOrPolicyIdOrPolicyName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupPolicyByPolicyIdOrName/{groupPolicyIdOrName}")]
        public async Task<IActionResult> GetGroupPolicyByPolicyAsync([FromRoute] string groupPolicyIdOrName)
        {
            try
            {
                var response = await _groupPolicyService.GetGrouPolicyByPolicyIdOrNameAsync(
                    groupPolicyIdOrName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupPolicyById/{groupPolicyId}")]
        public async Task<IActionResult> GetGroupPolicyByIdAsync([FromRoute] string groupPolicyId)
        {
            try
            {
                var response = await _groupPolicyService.GetGrouPolicyByIdAsync(groupPolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupPolicyByPolcyId/{policyId}")]
        public async Task<IActionResult> GetGroupPolicyByPolicyIdAsync([FromRoute] string policyId)
        {
            try
            {
                var response = await _groupPolicyService.GetGrouPolicyByPolicyIdAsync(policyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupPolicyByPolicyIdOrName/{groupPolicyIdOrName}")]
        public async Task<IActionResult> DeleteGroupPolicyByPolicyAsync(
            [FromRoute] string groupPolicyIdOrName)
        {
            try
            {
                var response = await _groupPolicyService.DeleteGrouPolicyByPolicyIdOrNameAsync(
                    groupPolicyIdOrName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupPolicyById/{groupPolicyId}")]
        public async Task<IActionResult> DeleteGroupPolicyByIdAsync([FromRoute] string groupPolicyId)
        {
            try
            {
                var response = await _groupPolicyService.DeleteGrouPolicyByIdAsync(groupPolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupPolicyByPolcyId/{policyId}")]
        public async Task<IActionResult> DeleteGroupPolicyByPolicyIdAsync([FromRoute] string policyId)
        {
            try
            {
                var response = await _groupPolicyService.DeleteGrouPolicyByPolicyIdAsync(policyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupPolicy/{groupPolicyIdOrPolicyIdOrPolicyName}")]
        public async Task<IActionResult> DeleteGroupPolicyAsync(
            [FromRoute] string groupPolicyIdOrPolicyIdOrPolicyName)
        {
            try
            {
                var response = await _groupPolicyService.DeleteGrouPolicyByGroupPolicyIdOrPolicyIdOrNameAsync(
                    groupPolicyIdOrPolicyIdOrPolicyName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupPolicies")]
        public async Task<IActionResult> GetGroupPoliciesAsync()
        {
            try
            {
                var response = await _groupPolicyService.GetGrouPoliciesAsync();
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

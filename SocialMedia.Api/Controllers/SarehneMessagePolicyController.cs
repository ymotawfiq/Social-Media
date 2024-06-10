using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.SarehneMessagePolicyService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class SarehneMessagePolicyController : ControllerBase
    {
        private readonly ISarehneMessagePolicyService _sarehneMessagePolicyService;
        public SarehneMessagePolicyController(ISarehneMessagePolicyService _sarehneMessagePolicyService)
        {
            this._sarehneMessagePolicyService = _sarehneMessagePolicyService;
        }

        [Authorize(Roles ="Admin")]

        [HttpPost("addSarehneMessagePolicy")]
        public async Task<IActionResult> AddSarehneMessagePolicyAsync(
            [FromBody] AddSarehneMessagePolicyDto addSarehneMessagePolicyDto)
        {
            try
            {
                var response = await _sarehneMessagePolicyService.AddPolicyAsync(addSarehneMessagePolicyDto);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }


        [Authorize(Roles = "Admin")]

        [HttpPut("updateSarehneMessagePolicy")]
        public async Task<IActionResult> AddSarehneMessagePolicyAsync(
            [FromBody] UpdateSarehneMessagePolicyToAnotherDto updateSarehneMessagePolicyToAnotherDto)
        {
            try
            {
                var response = await _sarehneMessagePolicyService.UpdatePolicyAsync(
                    updateSarehneMessagePolicyToAnotherDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("deleteSarehneMessagePolicyById/{sarehneMessagePolicyId}")]
        public async Task<IActionResult> DeleteSarehneMessagePolicyByIdAsync(
            [FromRoute] string sarehneMessagePolicyId)
        {
            try
            {
                var response = await _sarehneMessagePolicyService.DeletePolicyByIdAsync(
                    sarehneMessagePolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("deleteSarehneMessagePolicyByPolicy/{policyIdOrName}")]
        public async Task<IActionResult> DeleteSarehneMessagePolicyByPolicyAsync(
            [FromRoute] string policyIdOrName)
        {
            try
            {
                var response = await _sarehneMessagePolicyService.DeletePolicyByPolicyAsync(policyIdOrName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSarehneMessagePolicyByPolicy/{policyIdOrName}")]
        public async Task<IActionResult> GetSarehneMessagePolicyByPolicyAsync(
            [FromRoute] string policyIdOrName)
        {
            try
            {
                var response = await _sarehneMessagePolicyService.GetPolicyByPolicyAsync(policyIdOrName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSarehneMessagePolicyById/{sarehneMessagePolicyId}")]
        public async Task<IActionResult> GetSarehneMessagePolicyByIdAsync(
            [FromRoute] string sarehneMessagePolicyId)
        {
            try
            {
                var response = await _sarehneMessagePolicyService.GetPolicyByIdAsync(sarehneMessagePolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSarehneMessagePolicies")]
        public async Task<IActionResult> GetSarehneMessagePoliciesAsync()
        {
            try
            {
                var response = await _sarehneMessagePolicyService.GetPoliciesAsync();
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Service.AccountPostsPolicyService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class AccountPostsPolicyController : ControllerBase
    {

        private readonly IAccountPostsPolicyService _accountPostsPolicyService;
        public AccountPostsPolicyController(IAccountPostsPolicyService _accountPostsPolicyService)
        {
            this._accountPostsPolicyService = _accountPostsPolicyService;
        }

        [HttpPost("addAccountPostsPolicy")]
        public async Task<IActionResult> AddAccountPostsPolicyAsync(
            [FromBody] AddAccountPostsPolicyDto addAccountPostsPolicyDto)
        {
            try
            {
                var response = await _accountPostsPolicyService
                    .AddAccountPostPolicyAsync(addAccountPostsPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updateAccountPostsPolicy")]
        public async Task<IActionResult> UpdateAccountPostsPolicyAsync(
            [FromBody] UpdateAccountPostsPolicyDto updateAccountPostsPolicyDto)
        {
            try
            {
                var response = await _accountPostsPolicyService
                    .UpdateAccountPostPolicyAsync(updateAccountPostsPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }


        [HttpGet("getAccountPostsPolicyById/{accountPolicyId}")]
        public async Task<IActionResult> GetAccountPostsPolicyByIdAsync(
            [FromRoute] string accountPolicyId)
        {
            try
            {
                var response = await _accountPostsPolicyService
                    .GetAccountPostPolicyByIdAsync(accountPolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getAccountPostsPolicyByPolicyId/{policyId}")]
        public async Task<IActionResult> GetAccountPostsPolicyByPolicyIdAsync(
            [FromRoute] string policyId)
        {
            try
            {
                var response = await _accountPostsPolicyService
                    .GetAccountPostPolicyByPolicyIdAsync(policyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getAccountPostsPolicy/{postPolicyIdOrPolicyIdOrPolicyName}")]
        public async Task<IActionResult> GetAccountPostsPolicyAsync(
            [FromRoute] string postPolicyIdOrPolicyIdOrPolicyName)
        {
            try
            {
                var response = await _accountPostsPolicyService
                    .GetAccountPostPolicyAsync(postPolicyIdOrPolicyIdOrPolicyName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteAccountPostsPolicyById/{accountPolicyId}")]
        public async Task<IActionResult> DeleteAccountPostsPolicyByIdAsync(
            [FromRoute] string accountPolicyId)
        {
            try
            {
                var response = await _accountPostsPolicyService
                    .DeleteAccountPostPolicyByIdAsync(accountPolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteAccountPostsPolicyByPolicyId/{policyId}")]
        public async Task<IActionResult> DeleteAccountPostsPolicyByPolicyIdAsync(
            [FromRoute] string policyId)
        {
            try
            {
                var response = await _accountPostsPolicyService
                    .DeleteAccountPostPolicyByIdAsync(policyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteAccountPostsPolicy/{postPolicyIdOrPolicyIdOrPolicyName}")]
        public async Task<IActionResult> DeleteAccountPostsPolicyAsync(
            [FromRoute] string postPolicyIdOrPolicyIdOrPolicyName)
        {
            try
            {
                var response = await _accountPostsPolicyService
                    .DeleteAccountPostPolicyAsync(postPolicyIdOrPolicyIdOrPolicyName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getAccountPostsPolicies")]
        public async Task<IActionResult> GetAccountPostsPoliciesAsync()
        {
            try
            {
                var response = await _accountPostsPolicyService.GetAccountPostPoliciesAsync();
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

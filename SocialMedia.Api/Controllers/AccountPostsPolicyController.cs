using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PostsPolicyService;

namespace SocialMedia.Api.Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class AccountPostsPolicyController : ControllerBase
    {

        private readonly IPostsPolicyService _postsPolicyService;
        public AccountPostsPolicyController(IPostsPolicyService _postsPolicyService)
        {
            this._postsPolicyService = _postsPolicyService;
        }

        [HttpPost("addAccountPostsPolicy")]
        public async Task<IActionResult> AddAccountPostsPolicyAsync(
            [FromBody] AddAccountPostsPolicyDto addAccountPostsPolicyDto)
        {
            try
            {
                var response = await _postsPolicyService
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
                var response = await _postsPolicyService
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
                var response = await _postsPolicyService
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
                var response = await _postsPolicyService
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
                var response = await _postsPolicyService
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
                var response = await _postsPolicyService
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
                var response = await _postsPolicyService
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
                var response = await _postsPolicyService
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
                var response = await _postsPolicyService.GetAccountPostPoliciesAsync();
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

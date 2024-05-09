using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Service.AccountPolicyService;

namespace SocialMedia.Api.Controllers
{

    [ApiController]
    public class AccountPolicyController : ControllerBase
    {
        private readonly IAccountPolicyService _accountPolicyService;
        public AccountPolicyController(IAccountPolicyService _accountPolicyService)
        {
            this._accountPolicyService = _accountPolicyService;
        }


        [Authorize(Roles ="Admin")]
        [HttpPost("addAccountPolicy")]
        public async Task<IActionResult> 
            AddAccountPolicyAsync([FromBody] AddAccountPolicyDto addAccountPolicyDto)
        {
            try
            {
                var response = await _accountPolicyService.AddAccountPolicyAsync(addAccountPolicyDto);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateAccountPolicy")]
        public async Task<IActionResult>
            UpdateAccountPolicyAsync([FromBody] UpdateAccountPolicyDto updateAccountPolicyDto)
        {
            try
            {
                var response = await _accountPolicyService.UpdateAccountPolicyAsync(updateAccountPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteAccountPolicyById/{accountPolicyId}")]
        public async Task<IActionResult>
            DeleteAccountPolicyByIdAsync([FromRoute] string accountPolicyId)
        {
            try
            {
                var response = await _accountPolicyService.DeleteAccountPolicyByIdAsync(accountPolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteAccountPolicyByPolicyId/{policyId}")]
        public async Task<IActionResult>
            DeleteAccountPolicyByPolicyIdAsync([FromRoute] string policyId)
        {
            try
            {
                var response = await _accountPolicyService.DeleteAccountPolicyByPolicyAsync(policyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteAccountPolicy/{policyIdOrName}")]
        public async Task<IActionResult>
            DeleteAccountPolicyByPolicyAsync([FromRoute] string policyIdOrName)
        {
            try
            {
                var response = await _accountPolicyService.DeleteAccountPolicyByPolicyAsync(policyIdOrName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("accountPolicy/{policyIdOrName}")]
        public async Task<IActionResult>
            GetAccountPolicyByPolicyAsync([FromRoute] string policyIdOrName)
        {
            try
            {
                var response = await _accountPolicyService.GetAccountPolicyByPolicyAsync(policyIdOrName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAccountPolicyById/{accountPolicyId}")]
        public async Task<IActionResult>
            GetAccountPolicyByIdAsync([FromRoute] string accountPolicyId)
        {
            try
            {
                var response = await _accountPolicyService.GetAccountPolicyByIdAsync(accountPolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAccountPolicyByPolicy/{policyId}")]
        public async Task<IActionResult>
            GetAccountPolicyByPolicyIdOrNameAsync([FromRoute] string policyId)
        {
            try
            {
                var response = await _accountPolicyService.GetAccountPolicyByPolicyIdAsync(policyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("accountPolicies")]
        public async Task<IActionResult> GetAccountPoliciesAsync()
        {
            try
            {
                var response = await _accountPolicyService.GetAccountPoliciesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }


    }
}

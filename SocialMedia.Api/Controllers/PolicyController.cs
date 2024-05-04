using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        public PolicyController(IPolicyService _policyService)
        {
            this._policyService = _policyService;
        }

        [HttpGet("policies")]
        public async Task<IActionResult> GetPoliciesAsync()
        {
            try
            {
                var response = await _policyService.GetPoliciesAsync();
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

        [HttpPost("addPolicy")]
        public async Task<IActionResult> AddPolicyAsync([FromBody] PolicyDto policyDto)
        {
            try
            {
                var response = await _policyService.AddPolicyAsync(policyDto);
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

        [HttpPut("updatePolicy")]
        public async Task<IActionResult> UpdatePolicyAsync([FromBody] PolicyDto policyDto)
        {
            try
            {
                var response = await _policyService.UpdatePolicyAsync(policyDto);
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

        [HttpGet("getPolicy/{policyId}")]
        public async Task<IActionResult> GetPolicyByIdAsync([FromRoute] Guid policyId)
        {
            try
            {
                var response = await _policyService.GetPolicyByIdAsync(policyId);
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

        [HttpDelete("deletePolicy/{policyId}")]
        public async Task<IActionResult> DeletePolicyByIdAsync([FromRoute] Guid policyId)
        {
            try
            {
                var response = await _policyService.DeletePolicyByIdAsync(policyId);
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

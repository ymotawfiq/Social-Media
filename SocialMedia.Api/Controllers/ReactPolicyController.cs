using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Service.ReactPolicyService;

namespace SocialMedia.Api.Controllers
{

    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ReactPolicyController : ControllerBase
    {
        private readonly IReactPolicyService _reactPolicyService;
        public ReactPolicyController(IReactPolicyService _reactPolicyService)
        {
            this._reactPolicyService = _reactPolicyService;
        }

        [HttpGet("reactPolicies")]
        public async Task<IActionResult> GetAllReactPoliciesAsync()
        {
            try
            {
                var response = await _reactPolicyService.GetReactPoliciesAsync();
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

        [HttpPost("addReactPolicy")]
        public async Task<IActionResult> AddReactPoliciesAsync([FromBody] ReactPolicyDto reactPolicyDto)
        {
            try
            {
                var response = await _reactPolicyService.AddReactPolicyAsync(reactPolicyDto);
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

        [HttpPut("updateReactPolicy")]
        public async Task<IActionResult> UpdateReactPolicyAsync([FromBody] ReactPolicyDto reactPolicyDto)
        {
            try
            {
                var response = await _reactPolicyService.UpdateReactPolicyAsync(reactPolicyDto);
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

        [HttpGet("reactPolicy/{reactPolicyId}")]
        public async Task<IActionResult> GetReactPolicyByIdAsync([FromRoute]string reactPolicyId)
        {
            try
            {
                var response = await _reactPolicyService.GetReactPolicyByIdAsync(reactPolicyId);
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

        [HttpDelete("deleteReactPolicy/{reactPolicyId}")]
        public async Task<IActionResult> DeleteReactPolicyByIdAsync([FromRoute] string reactPolicyId)
        {
            try
            {
                var response = await _reactPolicyService.DeleteReactPolicyByIdAsync(reactPolicyId);
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

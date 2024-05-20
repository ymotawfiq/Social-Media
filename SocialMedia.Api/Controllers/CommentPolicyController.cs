using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Service.CommentPolicyService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    public class CommentPolicyController : ControllerBase
    {
        private readonly ICommentPolicyService _commentPolicyService;
        public CommentPolicyController(ICommentPolicyService _commentPolicyService)
        {
            this._commentPolicyService = _commentPolicyService;
        }

        [Authorize(Roles = "User")]
        [HttpGet("commentPolicies")]
        public async Task<IActionResult> GetAllCommentPoliciesAsync()
        {
            try
            {
                var response = await _commentPolicyService.GetCommentPoliciesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addCommentPolicy")]
        public async Task<IActionResult> AddCommentPolicyAsync([FromBody] CommentPolicyDto commentPolicyDto)
        {
            try
            {
                var response = await _commentPolicyService.AddCommentPolicyAsync(commentPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateCommentPolicy")]
        public async Task<IActionResult> UpdateCommentPolicyAsync([FromBody] CommentPolicyDto commentPolicyDto)
        {
            try
            {
                var response = await _commentPolicyService.UpdateCommentPolicyAsync(commentPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("commentPolicy/{commentPolicyId}")]
        public async Task<IActionResult> AddCommentPolicyAsync([FromRoute] string commentPolicyId)
        {
            try
            {
                var response = await _commentPolicyService.GetCommentPolicyByIdAsync(commentPolicyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteCommentPolicy/{commentPolicyId}")]
        public async Task<IActionResult> DeleteCommentPolicyAsync([FromRoute] string commentPolicyId)
        {
            try
            {
                var response = await _commentPolicyService.DeleteCommentPolicyByIdAsync(commentPolicyId);
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

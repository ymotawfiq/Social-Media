using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.SpecialCommentReactsService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class SpecialCommentReactsController : ControllerBase
    {
        private readonly ISpecialCommentReactsService _specialCommentReactService;
        public SpecialCommentReactsController(ISpecialCommentReactsService _specialCommentReactService)
        {
            this._specialCommentReactService = _specialCommentReactService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addSpecialCommentReact")]
        public async Task<IActionResult> AddSpecialCommentReactAsunc(
            [FromBody] AddSpecialCommentReactsDto addSpecialCommentsReactsDto)
        {
            try
            {
                var response = await _specialCommentReactService.AddSpecialCommentReactsAsync(
                    addSpecialCommentsReactsDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateSpecialCommentReact")]
        public async Task<IActionResult> UpdateSpecialCommentReactAsunc(
                [FromBody] UpdateSpecialCommentReactsDto updateSpecialCommentsReactsDto)
        {
            try
            {
                var response = await _specialCommentReactService.UpdateSpecialCommentReactsAsync(
                    updateSpecialCommentsReactsDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSpecialCommentReactById/{specialCommentReactId}")]
        public async Task<IActionResult> GetSpecialCommentReactByIdAsunc(
            [FromRoute] string specialCommentReactId)
        {
            try
            {
                var response = await _specialCommentReactService.GetSpecialCommentReactsByIdAsync(
                    specialCommentReactId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSpecialCommentReactByReactId/{reactId}")]
        public async Task<IActionResult> GetSpecialCommentReactByReactAsunc(
                [FromRoute] string reactId)
        {
            try
            {
                var response = await _specialCommentReactService.GetSpecialCommentReactsByReactIdAsync(
                    reactId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSpecialCommentReacts")]
        public async Task<IActionResult> GetSpecialCommentReactByReactAsunc()
        {
            try
            {
                var response = await _specialCommentReactService.GetSpecialCommentReactsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteSpecialCommentReactById/{specialCommentReactId}")]
        public async Task<IActionResult> DeleteSpecialCommentReactByIdAsunc(
            [FromRoute] string specialCommentReactId)
        {
            try
            {
                var response = await _specialCommentReactService.DeleteSpecialCommentReactsByIdAsync(
                    specialCommentReactId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteSpecialCommentReactByReactId/{reactId}")]
        public async Task<IActionResult> DeleteSpecialCommentReactByReactAsunc(
                [FromRoute] string reactId)
        {
            try
            {
                var response = await _specialCommentReactService.DeleteSpecialCommentReactsByReactIdAsync(
                    reactId);
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

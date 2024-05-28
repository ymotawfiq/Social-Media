using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.SpecialPostReactService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class SpecialPostsReactsController : ControllerBase
    {
        private readonly ISpecialPostReactService _specialPostReactService;
        public SpecialPostsReactsController(ISpecialPostReactService _specialPostReactService)
        {
            this._specialPostReactService = _specialPostReactService;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("addSpecialPostReact")]
        public async Task<IActionResult> AddSpecialPostReactAsunc(
            [FromBody]AddSpecialPostsReactsDto addSpecialPostsReactsDto)
        {
            try
            {
                var response = await _specialPostReactService.AddSpecialPostReactsAsync(
                    addSpecialPostsReactsDto);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateSpecialPostReact")]
        public async Task<IActionResult> UpdateSpecialPostReactAsunc(
                [FromBody] UpdateSpecialPostsReactsDto updateSpecialPostsReactsDto)
        {
            try
            {
                var response = await _specialPostReactService.UpdateSpecialPostReactsAsync(
                    updateSpecialPostsReactsDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSpecialPostReactById/{specialPostReactId}")]
        public async Task<IActionResult> GetSpecialPostReactByIdAsunc(
            [FromRoute] string specialPostReactId)
        {
            try
            {
                var response = await _specialPostReactService.GetSpecialPostReactsByIdAsync(
                    specialPostReactId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSpecialPostReactByReactId/{reactId}")]
        public async Task<IActionResult> GetSpecialPostReactByReactAsunc(
                [FromRoute] string reactId)
        {
            try
            {
                var response = await _specialPostReactService.GetSpecialPostReactsByReactIdAsync(
                    reactId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSpecialPostReacts")]
        public async Task<IActionResult> GetSpecialPostReactByReactAsunc()
        {
            try
            {
                var response = await _specialPostReactService.GetSpecialPostReactsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteSpecialPostReactById/{specialPostReactId}")]
        public async Task<IActionResult> DeleteSpecialPostReactByIdAsunc(
            [FromRoute] string specialPostReactId)
        {
            try
            {
                var response = await _specialPostReactService.DeleteSpecialPostReactsByIdAsync(
                    specialPostReactId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteSpecialPostReactByReactId/{reactId}")]
        public async Task<IActionResult> DeleteSpecialPostReactByReactAsunc(
                [FromRoute] string reactId)
        {
            try
            {
                var response = await _specialPostReactService.DeleteSpecialPostReactsByReactIdAsync(
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

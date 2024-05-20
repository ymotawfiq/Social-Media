using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.ReactService;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ReactsController : ControllerBase
    {
        private readonly IReactService _reactService;
        public ReactsController(IReactService _reactService)
        {
            this._reactService = _reactService;
        }

        [HttpPost("add-react")]
        public async Task<IActionResult> AddReactAsync([FromBody] ReactDto reactDto)
        {
            try
            {
                var response = await _reactService.AddReactAsync(reactDto);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpPut("update-react")]
        public async Task<IActionResult> UpdateReactAsync([FromBody] ReactDto reactDto)
        {
            try
            {
                var response = await _reactService.UpdateReactAsync(reactDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getReactById/{reactId}")]
        public async Task<IActionResult> GetReactByIdAsync([FromRoute] Guid reactId)
        {
            try
            {
                var response = await _reactService.GetReactByIdAsync(reactId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getAllReacts")]
        public async Task<IActionResult> GetAllReactsAsync()
        {
            try
            {
                var response = await _reactService.GetAllReactsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteReactById/{reactId}")]
        public async Task<IActionResult> DeleteReactByIdAsync([FromRoute] Guid reactId)
        {
            try
            {
                var response = await _reactService.DeleteReactByIdAsync(reactId);
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

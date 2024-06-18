using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.ReactService;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]

    public class ReactsController : ControllerBase
    {
        private readonly IReactService _reactService;
        public ReactsController(IReactService _reactService)
        {
            this._reactService = _reactService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-react")]
        public async Task<IActionResult> AddReactAsync([FromBody] AddReactDto addReactDto)
        {
            try
            {
                var response = await _reactService.AddReactAsync(addReactDto);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-react")]
        public async Task<IActionResult> UpdateReactAsync([FromBody] UpdateReactDto updateReactDto)
        {
            try
            {
                var response = await _reactService.UpdateReactAsync(updateReactDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getReactById/{reactId}")]
        public async Task<IActionResult> GetReactByIdAsync([FromRoute] string reactId)
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

        [HttpGet("getReactByName/{reactName}")]
        public async Task<IActionResult> GetReactByNameAsync([FromRoute] string reactName)
        {
            try
            {
                var response = await _reactService.GetReactByNameAsync(reactName);
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteReactById/{reactId}")]
        public async Task<IActionResult> DeleteReactByIdAsync([FromRoute] string reactId)
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteReactByName/{reactName}")]
        public async Task<IActionResult> DeleteReactByNameAsync([FromRoute] string reactName)
        {
            try
            {
                var response = await _reactService.DeleteReactByNameAsync(reactName);
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

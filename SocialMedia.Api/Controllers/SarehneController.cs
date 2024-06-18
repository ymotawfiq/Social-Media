using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.SarehneService;

namespace SocialMedia.Api.Api.Controllers
{
    [ApiController]
    public class SarehneController : ControllerBase
    {
        private readonly ISarehneService _sarehneService;
        private readonly UserManagerReturn _userManagerReturn;
        public SarehneController(ISarehneService _sarehneService, UserManagerReturn _userManagerReturn)
        {
            this._sarehneService = _sarehneService;
            this._userManagerReturn = _userManagerReturn;
        }


        [HttpPost("sendSarehneMessage")]
        public async Task<IActionResult> SendSarehneMessageAsync(
            [FromBody] SendSarahaMessageDto sendSarahaMessageDto)
        {
            try
            {
                if(HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    var response1 = await _sarehneService.SendMessageAsync(sendSarahaMessageDto, user);
                    return Ok(response1);
                }
                var response2 = await _sarehneService.SendMessageAsync(sendSarahaMessageDto, null!);
                return Ok(response2);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSarehneMessage/{messageId}")]
        public async Task<IActionResult> GetSarehneMessageAsync([FromRoute] string messageId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response1 = await _sarehneService.GetMessageAsync(messageId, user);
                        return Ok(response1);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                    ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteSarehneMessage/{messageId}")]
        public async Task<IActionResult> DeleteSarehneMessageAsync([FromRoute] string messageId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response1 = await _sarehneService.DeleteMessageAsync(messageId, user);
                        return Ok(response1);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                    ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSarehneMessages")]
        public async Task<IActionResult> GetSarehneMessagesAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response1 = await _sarehneService.GetMessagesAsync(user);
                        return Ok(response1);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                    ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getSarehneMessages/{userIdOrNameOrEmail}")]
        public async Task<IActionResult> GetPublicUserSarehneMessagesAsync(
            [FromRoute] string userIdOrNameOrEmail)
        {
            try
            {
                var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userIdOrNameOrEmail);
                if (user != null)
                {
                    var response1 = await _sarehneService.GetPublicMessagesAsync(user);
                    return Ok(response1);
                }
                return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.MessageReactService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class MessageReactController : ControllerBase
    {
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IMessageReactService _messageReactService;
        public MessageReactController(UserManagerReturn _userManagerReturn, 
            IMessageReactService _messageReactService)
        {
            this._messageReactService = _messageReactService;
            this._userManagerReturn = _userManagerReturn;
        }

        [HttpPost("addMessageReact")]
        public async Task<IActionResult> AddMessageReactAsync([FromBody] AddMessageReactDto addMessageReactDto)
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
                        var response = await _messageReactService.ReactToMessageAsync(
                            addMessageReactDto, user);
                        return Ok(response);
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

        [HttpDelete("unReactToMessageByMessageId/{messageId}")]
        public async Task<IActionResult> UnReactToMessageByMessageIdAsync([FromRoute] string messageId)
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
                        var response = await _messageReactService.UnReactToMessageByMessageIdAsync(
                            messageId, user);
                        return Ok(response);
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

        [HttpDelete("unReactToMessageByMessageReactId/{messageReactId}")]
        public async Task<IActionResult> UnReactToMessageByMessageReactIdAsync(
            [FromRoute] string messageReactId)
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
                        var response = await _messageReactService.UnReactToMessageByMessageReactIdAsync(
                            messageReactId, user);
                        return Ok(response);
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


        [HttpGet("getMessageReactsByMessageId/{messageReactId}")]
        public async Task<IActionResult> UnReactsToMessageByMessageIdAsync([FromRoute] string messageId)
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
                        var response = await _messageReactService.GetReactsByMessageIdAsync(messageId, user);
                        return Ok(response);
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


    }
}

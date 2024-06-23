using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.ChatManagerService;
using SocialMedia.Api.Service.ChatMessageService;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IChatMessageService _chatMessageService;
        public ChatMessageController(UserManagerReturn _userManagerReturn,
            IChatMessageService _chatMessageService)
        {
            this._chatMessageService = _chatMessageService;
            this._userManagerReturn = _userManagerReturn;
        }


        [HttpPost("sendChatMessage")]
        public async Task<IActionResult> SendChatMessageAsync([FromBody] AddChatMessageDto addChatMessageDto)
        {
            try
            {
                if(HttpContext.User != null && HttpContext.User.Identity != null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _chatMessageService.SendMessageAsync(addChatMessageDto, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                            ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                    ._401_UnAuthorized());
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updateChatMessage")]
        public async Task<IActionResult> UpdateChatMessageAsync(
            [FromBody] UpdateChatMessageDto updateChatMessageDto)
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
                        var response = await _chatMessageService.UnpdateMessageAsync(
                            updateChatMessageDto, user);
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

        [HttpDelete("unSendChatMessage/{messageId}")]
        public async Task<IActionResult> UnSendChatMessageAsync([FromRoute] string messageId)
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
                        var response = await _chatMessageService.UnSendMessageAsync(messageId, user);
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

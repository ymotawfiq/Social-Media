using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.ChatMessageService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.MessageReactService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly IChatMessageService _chatMessageService;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IMessageReactService _messageReactService;
        public ChatMessageController(IChatMessageService _chatMessageService,
            UserManagerReturn _userManagerReturn, IMessageReactService _messageReactService)
        {
            this._chatMessageService = _chatMessageService;
            this._userManagerReturn = _userManagerReturn;
            this._messageReactService = _messageReactService;
        }

        [HttpPost("sendChatMessage")]
        public async Task<IActionResult> SendChatMessageAsync([FromBody] AddChatMessageDto addChatMessageDto)
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
                        var response = await _chatMessageService.SendMessageAsync(addChatMessageDto, user);
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

        [HttpPost("replayToChatMessage")]
        public async Task<IActionResult> ReplayToChatMessageAsync(
                [FromBody] AddChatMessageReplayDto addChatMessageReplayDto)
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
                        var response = await _chatMessageService.ReplayToMessageAsync(
                            addChatMessageReplayDto, user);
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

        [HttpPost("reactToChatMessage")]
        public async Task<IActionResult> ReactToChatMessageAsync(
        [FromBody] AddMessageReactDto addMessageReactDto)
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
                        var response = await _messageReactService.AddReactToMessageAsync(
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
                        var response = await _chatMessageService.UpdateMessageAsync(updateChatMessageDto,
                            user);
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

        [HttpPut("updateReactToChatMessage")]
        public async Task<IActionResult> UpdateReactToChatMessageAsync(
                [FromBody] UpdateMessageReactDto updateMessageReactDto)
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
                        var response = await _messageReactService.UpdateReactToMessageAsync(
                            updateMessageReactDto, user);
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

        [HttpGet("getUserChatMessages/{chatId}")]
        public async Task<IActionResult> GetUserChatMessagesAsync([FromRoute] string chatId)
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
                        var response = await _chatMessageService.GetUserMessagesAsync(user, chatId);
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

        [HttpGet("getChatMessage/{chatId}/{messageId}")]
        public async Task<IActionResult> GetChatMessageAsync(
                [FromRoute] string messageId, [FromRoute] string chatId)
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
                        var response = await _chatMessageService.GetMessageAsync(messageId, chatId, user);
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

        [HttpGet("getChatMessageReactById/{messageReactId}")]
        public async Task<IActionResult> GetChatMessageReactByIdAsync([FromRoute] string messageReactId)
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
                        var response = await _messageReactService.GetReactToMessageAsync(messageReactId, user);
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

        [HttpDelete("deleteChatMessage/{chatId}/{messageId}")]
        public async Task<IActionResult> DeleteChatMessageAsync(
            [FromRoute] string messageId, [FromRoute] string chatId)
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
                        var response = await _chatMessageService.DeleteMessageAsync(messageId, chatId, user);
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

        [HttpGet("getChatMessageReactsByMessageId/{messageId}")]
        public async Task<IActionResult> GetChatMessageReactsByMessageIdAsync([FromRoute] string messageId)
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
                        var response = await _messageReactService.GetMessageReactsAsync(messageId, user);
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

        [HttpDelete("deleteChatMessageReactById/{messageReactId}")]
        public async Task<IActionResult> DeleteChatMessageReactByIdAsync([FromRoute] string messageReactId)
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
                        var response = await _messageReactService.DeleteReactToMessageAsync(
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


    }
}

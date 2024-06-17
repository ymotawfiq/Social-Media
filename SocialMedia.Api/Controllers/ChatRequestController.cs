using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Repository.ChatRequestRepository;
using SocialMedia.Service.ChatRequestService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class ChatRequestController : ControllerBase
    {

        private readonly IChatRequestService _chatRequestService;
        private readonly UserManagerReturn _userManagerReturn;
        public ChatRequestController(IChatRequestService _chatRequestService,
            UserManagerReturn _userManagerReturn)
        {
            this._chatRequestService = _chatRequestService;
            this._userManagerReturn = _userManagerReturn;
        }

        [HttpPost("sendChatRequest")]
        public async Task<IActionResult> SendChatRequestAsync([FromBody] AddChatRequestDto addChatRequestDto)
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
                        var routeUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                            addChatRequestDto.UserIdOrNameOrEmail);
                        if (routeUser != null)
                        {
                            var response = await _chatRequestService.AddChatRequestAsync(addChatRequestDto,
                                user);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                            ._404_NotFound("User you want to send chat request not found"));
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

        [HttpPost("acceptChatRequest/{chatRequestId}")]
        public async Task<IActionResult> AcceptChatRequestAsync([FromRoute] string chatRequestId)
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
                            var response = await _chatRequestService.AcceptChatRequestAsync(chatRequestId,
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

        [HttpDelete("deleteChatRequest/{chatRequestId}")]
        public async Task<IActionResult> DeleteChatRequestAsync([FromRoute] string chatRequestId)
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
                        var response = await _chatRequestService.DeleteChatRequestByIdAsync(chatRequestId,
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

        [HttpGet("getChatRequest/{chatRequestId}")]
        public async Task<IActionResult> GetChatRequestAsync([FromRoute] string chatRequestId)
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
                        var response = await _chatRequestService.GetChatRequestByIdAsync(chatRequestId,
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

        [HttpGet("getSentChatRequest")]
        public async Task<IActionResult> GetSentChatRequestsAsync()
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
                        var response = await _chatRequestService.GetSentChatRequestsAsync(user);
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

        [HttpGet("getReceivedChatRequest")]
        public async Task<IActionResult> GetReceivedChatRequestsAsync()
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
                        var response = await _chatRequestService.GetReceivedChatRequestsAsync(user);
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

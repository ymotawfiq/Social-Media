using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.ChatService;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly UserManagerReturn _userManagerReturn;
        private readonly IChatService _chatService;
        public ChatController(UserManagerReturn _userManagerReturn, IChatService _chatService)
        {
            this._chatService = _chatService;
            this._userManagerReturn = _userManagerReturn;
        }

        [HttpPost("addGroupChat")]
        public async Task<IActionResult> AddNonPrivateChatAsync([FromBody] AddChatDto addChatDto)
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
                        var response = await _chatService.AddNonPrivateChatAsync(addChatDto, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._401_UnAuthorized());
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpPost("addprivateChat")]
        public async Task<IActionResult> AddPrivateChatAsync([FromBody] AddChatDto addChatDto)
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
                        var response = await _chatService.AddPrivateChatAsync(addChatDto, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getChat/{id}")]
        public async Task<IActionResult> GetChatAsync([FromRoute] string id)
        {
            var response = await _chatService.GetChatAsync(id);
            return Ok(response);
        }


    }
}

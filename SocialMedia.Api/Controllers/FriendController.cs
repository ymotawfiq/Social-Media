using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    public class FriendController : ControllerBase
    {

        private readonly IFriendService _friendService;
        private readonly UserManagerReturn _userManagerReturn;
        public FriendController(IFriendService _friendService, UserManagerReturn _userManagerReturn)
        {
            this._friendService = _friendService;
            this._userManagerReturn = _userManagerReturn;
        }


        [HttpGet("friends/{userIdOrUserName}")]
        public async Task<IActionResult> GetFriensdByUserNameAsync([FromRoute] string userIdOrUserName)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var routeUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                            userIdOrUserName);
                        if (routeUser != null)
                        {
                            var response = await _friendService.GetAllUserFriendsAsync(user, routeUser);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                                ._404_NotFound("User you want to get friend list not found"));
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

        [HttpGet("sharedFriends/{userIdOrUserName}")]
        public async Task<IActionResult> GetSharedFriensdAsync([FromRoute] string userIdOrUserName)
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
                            userIdOrUserName);
                        if (routeUser != null)
                        {
                            var response = await _friendService.GetSharedFriendsAsync(user, routeUser);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                                ._404_NotFound("Route user not found"));
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

        [HttpGet("friends")]
        public async Task<IActionResult> GetFriensAsync()
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
                        var response = await _friendService.GetAllUserFriendsAsync(user);
                        return Ok(response);
                    }
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

        [HttpDelete("unFriend/{friendIdOrUserNameOrEmail}")]
        public async Task<IActionResult> DeleteFriendAsync([FromRoute] string friendIdOrUserNameOrEmail)
        {
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
                            friendIdOrUserNameOrEmail);
                            if (routeUser != null)
                            {
                                var response = await _friendService.DeleteFriendAsync(user.Id, routeUser.Id);
                                return Ok(response);
                            }
                            return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                                ._404_NotFound("User not found in your friend list"));
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
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Service.FriendsService;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    public class FriendController : ControllerBase
    {

        private readonly IFriendService _friendService;
        private readonly UserManager<SiteUser> _userManager;
        public FriendController(IFriendService _friendService, UserManager<SiteUser> _userManager)
        {
            this._friendService = _friendService;
            this._userManager = _userManager;
        }

        [HttpGet("friends/{userName}")]
        public async Task<IActionResult> GetFriensdByUserNameAsync([FromRoute] string userName)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var userByUserName = await _userManager.FindByNameAsync(userName);
                    if (user != null && userByUserName != null)
                    {
                        if(await _userManager.IsInRoleAsync(user, "Admin")
                            || user.Id == userByUserName.Id || !userByUserName.IsFriendListPrivate)
                        {
                            var response = await _friendService.GetAllUserFriendsAsync(user.Id);
                            return Ok(response);
                        }
                    }
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                    {
                        StatusCode = 403,
                        IsSuccess = false,
                        Message = "Forbidden"
                    });
                }
                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
                {
                    StatusCode = 401,
                    IsSuccess = false,
                    Message = "Unauthorized"
                });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
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
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _friendService.GetAllUserFriendsAsync(user.Id);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                    {
                        StatusCode = 403,
                        IsSuccess = false,
                        Message = "Forbidden"
                    });
                }
                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
                {
                    StatusCode = 401,
                    IsSuccess = false,
                    Message = "Unauthorized"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("deleteFriend/{friendIdOrUserNameOrEmail}")]
        public async Task<IActionResult> DeleteFriendAsync([FromRoute] string friendIdOrUserNameOrEmail)
        {
            {
                try
                {
                    if (HttpContext.User != null && HttpContext.User.Identity != null
                        && HttpContext.User.Identity.Name != null)
                    {
                        var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                        var routeUser = await GetUserAsync(friendIdOrUserNameOrEmail);
                        if (user != null && routeUser != null)
                        {
                            var response = await _friendService.DeleteFriendAsync(user.Id, routeUser.Id);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                        {
                            StatusCode = 403,
                            IsSuccess = false,
                            Message = "Forbidden"
                        });
                    }
                    return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
                    {
                        StatusCode = 401,
                        IsSuccess = false,
                        Message = "Unauthorized"
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                    {
                        StatusCode = 500,
                        IsSuccess = false,
                        Message = ex.Message
                    });
                }
            }
        }


        private async Task<SiteUser> GetUserAsync(string userIdOrUserNameOrEmail)
        {
            var userById = await _userManager.FindByIdAsync(userIdOrUserNameOrEmail);
            var userByName = await _userManager.FindByNameAsync(userIdOrUserNameOrEmail);
            var userByEmail = await _userManager.FindByEmailAsync(userIdOrUserNameOrEmail);
            if (userById != null)
            {
                return userById;
            }
            else if (userByName != null)
            {
                return userByName;
            }
            else if (userByEmail != null)
            {
                return userByEmail;
            }
            return null;
        }


    }
}

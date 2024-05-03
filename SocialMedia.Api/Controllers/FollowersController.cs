using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Service.FollowerService;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    public class FollowersController : ControllerBase
    {

        private readonly IFollowerService _followerService;
        private readonly UserManager<SiteUser> _userManager;
        public FollowersController(IFollowerService _followerService, UserManager<SiteUser> _userManager)
        {
            this._followerService = _followerService;
            this._userManager = _userManager;
        }


        [HttpPost("Follow")]
        public async Task<IActionResult> FollowAsync([FromBody] FollowDto followDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null
                    && HttpContext.User.Identity.Name != null)
                {
                    var follower = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var user = await GetUsetAsync(followDto.UserIdOrUserNameOrEmail);
                    var followerDto = new FollowerDto
                    {
                        UserId = user!.Id,
                        FollowerId = follower!.Id
                    };
                    var response = await _followerService.FollowAsync(followerDto);
                    return Ok(response);
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

        [Authorize(Roles ="Admin")]
        [HttpGet("adminFollow")]
        public async Task<IActionResult> FollowAsync(string userIdOrUserNameOrEmail
            , string followerIdOrUserNameOrEmail)
        {
            try
            {
                var user = await GetUsetAsync(userIdOrUserNameOrEmail);
                var follower = await GetUsetAsync(followerIdOrUserNameOrEmail);
                var followerDto = new FollowerDto
                {
                    UserId = user.Id,
                    FollowerId = follower.Id
                };
                var response = await _followerService.FollowAsync(followerDto);
                return Ok(response);
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

        [HttpGet("followers")]
        public async Task<IActionResult> FollowersAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var response = await _followerService.GetAllFollowers(user!.Id);
                    return Ok(response);
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

        [Authorize(Roles ="Admin")]
        [HttpGet("followers/{userIdOrName}")]
        public async Task<IActionResult> FollowersAsync([FromRoute] string userIdOrName)
        {
            try
            {
                var user = await GetUsetAsync(userIdOrName);
                var response = await _followerService.GetAllFollowers(user.Id);
                return Ok(response);
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

        [HttpPost("unfollow")]
        public async Task<IActionResult> UnfollowAsync(string userIdOrUserNameOrEmail)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var follower = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var user = await GetUsetAsync(userIdOrUserNameOrEmail);
                    var response = await _followerService.UnfollowAsync(user.Id, follower!.Id);
                    return Ok(response);
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


        private async Task<SiteUser> GetUsetAsync(string PersonIdOrUserNameOrEmail)
        {
            var userByName = await _userManager.FindByNameAsync(PersonIdOrUserNameOrEmail);
            var userById = await _userManager.FindByIdAsync(PersonIdOrUserNameOrEmail);
            var userByEmail = await _userManager.FindByEmailAsync(PersonIdOrUserNameOrEmail);
            if (userByEmail != null)
            {
                return userByEmail;
            }
            else if (userById != null)
            {
                return userById;
            }
            else if (userByName != null)
            {
                return userByName;
            }
            return null!;
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Service.FollowerService;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    public class FollowersController : ControllerBase
    {

        private readonly IFollowerService _followerService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IBlockRepository _blockRepository;
        public FollowersController(IFollowerService _followerService, UserManager<SiteUser> _userManager,
            IBlockRepository _blockRepository)
        {
            this._followerService = _followerService;
            this._userManager = _userManager;
            this._blockRepository = _blockRepository;
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
                    if(follower!=null && user != null)
                    {
                        if (user.Id != follower.Id)
                        {
                            var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                                user.Id, follower.Id);
                            if (isBlocked == null)
                            {
                                var followerDto = new FollowerDto
                                {
                                    UserId = user!.Id,
                                    FollowerId = follower!.Id
                                };
                                var response = await _followerService.FollowAsync(followerDto);
                                return Ok(response);
                            }
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                        {
                            StatusCode = 403,
                            Message = "Forbidden",
                            IsSuccess = false
                        });
                    }
                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                    {
                        StatusCode = 406,
                        Message = "User you want to follow not found",
                        IsSuccess = false
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

        [Authorize(Roles ="Admin")]
        [HttpGet("adminFollow")]
        public async Task<IActionResult> FollowAsync(string userIdOrUserNameOrEmail
            , string followerIdOrUserNameOrEmail)
        {
            try
            {
                var user = await GetUsetAsync(userIdOrUserNameOrEmail);
                var follower = await GetUsetAsync(followerIdOrUserNameOrEmail);
                if(user!=null && follower != null)
                {
                    if (user.Id != follower.Id)
                    {
                        var followerDto = new FollowerDto
                        {
                            UserId = user.Id,
                            FollowerId = follower.Id
                        };
                        var response = await _followerService.FollowAsync(followerDto);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                    {
                        StatusCode = 403,
                        Message = "Forbidden",
                        IsSuccess = false
                    });
                }
                return StatusCode(StatusCodes.Status406NotAcceptable, new ApiResponse<string>
                {
                    StatusCode = 406,
                    Message = "Not Acceptable",
                    IsSuccess = false
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

        [HttpGet("followers")]
        public async Task<IActionResult> FollowersAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _followerService.GetAllFollowers(user!.Id);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Message = "User not found"
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


        [HttpGet("followers/{userIdOrName}")]
        public async Task<IActionResult> FollowersAsync([FromRoute] string userIdOrName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name!=null)
                {
                    var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var user = await GetUsetAsync(userIdOrName);
                    if (currentUser != null && user!=null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            user.Id, currentUser.Id);
                        if (isBlocked == null)
                        {
                            var response = await _followerService.GetAllFollowers(user.Id);
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
                    if (user != null && follower!=null)
                    {
                        if(user.Id != follower.Id)
                        {
                            var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                                user.Id, follower.Id);
                            if (isBlocked == null)
                            {
                                var response = await _followerService.UnfollowAsync(user.Id, follower.Id);
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
                    return StatusCode(StatusCodes.Status406NotAcceptable, new ApiResponse<string>
                    {
                        StatusCode = 406,
                        Message = "Not Acceptable",
                        IsSuccess = false
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

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
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    public class FollowersController : ControllerBase
    {

        private readonly IFollowerService _followerService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IBlockRepository _blockRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public FollowersController(IFollowerService _followerService, UserManager<SiteUser> _userManager,
            IBlockRepository _blockRepository, UserManagerReturn _userManagerReturn)
        {
            this._followerService = _followerService;
            this._userManager = _userManager;
            this._blockRepository = _blockRepository;
            this._userManagerReturn = _userManagerReturn;
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
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        followDto.UserIdOrUserNameOrEmail);
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
                        return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                            ._403_Forbidden());
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._404_NotFound("User you want to follow not found"));
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

        [Authorize(Roles ="Admin")]
        [HttpGet("adminFollow")]
        public async Task<IActionResult> FollowAsync(string userIdOrUserNameOrEmail
            , string followerIdOrUserNameOrEmail)
        {
            try
            {
                var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        userIdOrUserNameOrEmail);
                var follower = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        followerIdOrUserNameOrEmail);
                if (user!=null && follower != null)
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
                    return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                    ._403_Forbidden());
                }
                return StatusCode(StatusCodes.Status406NotAcceptable, StatusCodeReturn<string>
                    ._406_NotAcceptable());
                
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
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


        [HttpGet("followers/{userIdOrName}")]
        public async Task<IActionResult> FollowersAsync([FromRoute] string userIdOrName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name!=null)
                {
                    var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        userIdOrName);
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
                    return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                    ._403_Forbidden());
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

        [HttpPost("unfollow")]
        public async Task<IActionResult> UnfollowAsync(string userIdOrUserNameOrEmail)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var follower = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        userIdOrUserNameOrEmail); 
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
                        return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                            ._403_Forbidden());
                    }
                    return StatusCode(StatusCodes.Status406NotAcceptable, StatusCodeReturn<string>
                    ._406_NotAcceptable());
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

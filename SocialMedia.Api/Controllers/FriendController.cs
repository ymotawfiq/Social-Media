using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    public class FriendController : ControllerBase
    {

        private readonly IFriendService _friendService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IBlockRepository _blockRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public FriendController(IFriendService _friendService, UserManager<SiteUser> _userManager,
            IBlockRepository _blockRepository, UserManagerReturn _userManagerReturn)
        {
            this._friendService = _friendService;
            this._userManager = _userManager;
            this._blockRepository = _blockRepository;
            this._userManagerReturn = _userManagerReturn;
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
                            var response = await _friendService.GetAllUserFriendsAsync(userByUserName.Id);
                            return Ok(response);
                        }
                    }
                    return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                    ._403_Forbidden());
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

        [HttpDelete("unFriend/{friendIdOrUserNameOrEmail}")]
        public async Task<IActionResult> DeleteFriendAsync([FromRoute] string friendIdOrUserNameOrEmail)
        {
            {
                try
                {
                    if (HttpContext.User != null && HttpContext.User.Identity != null
                        && HttpContext.User.Identity.Name != null)
                    {
                        var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                        var routeUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                            friendIdOrUserNameOrEmail);
                        if (user != null && routeUser != null)
                        {
                            if (user.Id != routeUser.Id)
                            {
                                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                                user.Id, routeUser.Id);
                                if (isBlocked == null)
                                {
                                    var response = await _friendService.DeleteFriendAsync(user.Id, routeUser.Id);
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
}

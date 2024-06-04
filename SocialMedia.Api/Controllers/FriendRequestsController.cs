using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Service.FriendRequestService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{

    [ApiController]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public FriendRequestsController(IFriendRequestService _friendRequestService,
            UserManager<SiteUser> _userManager, IFriendRequestRepository _friendRequestRepository,
             UserManagerReturn _userManagerReturn)
        {
            this._friendRequestService = _friendRequestService;
            this._userManager = _userManager;
            this._friendRequestRepository = _friendRequestRepository;
            this._userManagerReturn = _userManagerReturn;
        }

        [HttpPost("sendFriendRequest")]
        public async Task<IActionResult> AddFriendRequestAsync(
            [FromBody] AddFriendRequestDto addFriendRequestDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var friendRequestPerson = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        addFriendRequestDto.PersonIdOrUserNameOrEmail);
                    if (user != null && friendRequestPerson!=null)
                    {
                        if (user.Id != friendRequestPerson.Id)
                        {
                            var response = await _friendRequestService.AddFriendRequestAsync
                                (addFriendRequestDto, user);
                            return Ok(response);
                            
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                            ._403_Forbidden());
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

        [HttpPut("updateFriendRequest")]
        public async Task<IActionResult> UpdateFriendRequestAsync(
            [FromBody] UpdateFriendRequestDto updateFriendRequestDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {  
                        if (updateFriendRequestDto.FriendRequestId != null)
                        {
                            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(
                            updateFriendRequestDto.FriendRequestId);
                            if (friendRequest != null)
                            {
                                if (await _userManager.IsInRoleAsync(user, "Admin")
                                    || user.Id == friendRequest.UserWhoReceivedId)
                                {
                                    var response = await _friendRequestService
                                        .UpdateFriendRequestAsync(updateFriendRequestDto);
                                    return Ok(response);
                                }
                            }   
                        }
                        return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                            ._404_NotFound("Friend request not found"));
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

        [HttpDelete("deleteFriendRequest/{friendRequestId}")]
        public async Task<IActionResult> DeleteFriendRequestAsync([FromRoute] string friendRequestId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var friendRequest = await _friendRequestRepository
                            .GetFriendRequestByIdAsync(friendRequestId);
                        if (friendRequest != null)
                        {
                            if (await _userManager.IsInRoleAsync(user, "Admin")
                                || user.Id == friendRequest.UserWhoSendId
                                || user.Id == friendRequest.UserWhoReceivedId)
                            {
                                var response = await _friendRequestService.DeleteFriendRequestByAsync(
                                    user, friendRequestId);
                                return Ok(response);
                            }
                        }
                        return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                            ._404_NotFound("Friend request not found"));
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

        [HttpDelete("unSendFriendRequest/{userWhoReceivedIdOrUserName}")]
        public async Task<IActionResult> UnSendFriendRequestAsync(
            [FromRoute] string userWhoReceivedIdOrUserName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var sender = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var receiver = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        userWhoReceivedIdOrUserName);
                    if (sender != null && receiver != null)
                    {
                        var friendRequest = await _friendRequestRepository.GetFriendRequestByUserAndPersonIdAsync
                            (sender.Id, receiver.Id);
                        if (friendRequest == null || sender.Id == receiver.Id)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                    ._403_Forbidden());
                        }
                        else
                        {
                            if (await _userManager.IsInRoleAsync(sender, "Admin")
                                || sender.Id == friendRequest.UserWhoSendId)
                            {
                                var response = await _friendRequestService.DeleteFriendRequestByAsync(
                                    sender, friendRequest.Id);
                                return Ok(response);
                            }
                        }
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

        [Authorize(Roles ="Admin")]
        [HttpGet("allFriendRequests")]
        public async Task<IActionResult> GetAllFriendsRequestsAsync()
        {
            try
            {
                var response = await _friendRequestService.GetAllFriendRequestsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("friendRequests")]
        public async Task<IActionResult> GetUserFriendRequestsAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _friendRequestService.GetAllFriendRequestsByUserIdAsync(user.Id);
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

        [HttpGet("friendRequests/{userIdOrUserName}")]
        public async Task<IActionResult> GetAllFriendsRequestsByUserAsync([FromRoute] string userIdOrUserName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var routeUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                            userIdOrUserName);
                        if (await _userManager.IsInRoleAsync(user, "Admin") || user.Id == routeUser.Id)
                        {
                            var response = await _friendRequestService.GetAllFriendRequestsByUserIdAsync(
                                routeUser.Id);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                            ._403_Forbidden());
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

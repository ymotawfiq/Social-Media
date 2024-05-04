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

namespace SocialMedia.Api.Controllers
{

    [ApiController]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IBlockRepository _blockRepository;
        public FriendRequestsController(IFriendRequestService _friendRequestService,
            UserManager<SiteUser> _userManager, IFriendRequestRepository _friendRequestRepository,
            IBlockRepository _blockRepository)
        {
            this._friendRequestService = _friendRequestService;
            this._userManager = _userManager;
            this._friendRequestRepository = _friendRequestRepository;
            this._blockRepository = _blockRepository;
        }

        [HttpPost("sendFriendRequest")]
        public async Task<IActionResult> AddFriendRequestAsync([FromBody] AddFriendRequestDto addFriendRequestDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var friendRequestPerson = await GetUsetAsync(addFriendRequestDto.PersonIdOrUserNameOrEmail);
                    if (user != null && friendRequestPerson!=null)
                    {
                        if (user.Id != friendRequestPerson.Id)
                        {
                            var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                                user.Id, friendRequestPerson.Id);
                            if (isBlocked == null)
                            {
                                var friendRequestDto = new FriendRequestDto
                                {
                                    UserId = user.Id,
                                    PersonId = friendRequestPerson.Id
                                };
                                var response = await _friendRequestService.AddFriendRequestAsync
                                    (friendRequestDto);
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

        [HttpPut("updateFriendRequest")]
        public async Task<IActionResult> UpdateFriendRequestAsync
            ([FromBody] UpdateFriendRequestDto updateFriendRequestDto)
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
                            new Guid(updateFriendRequestDto.FriendRequestId));
                            if (friendRequest != null)
                            {
                                var friendRequestDto = new FriendRequestDto
                                {
                                    UserId = friendRequest.UserWhoSendId,
                                    PersonId = friendRequest.UserWhoReceivedId,
                                    IsAccepted = updateFriendRequestDto.IsAccepted,
                                    Id = updateFriendRequestDto.FriendRequestId
                                };
                                if (await _userManager.IsInRoleAsync(user, "Admin")
                                    || user.Id == friendRequestDto.PersonId)
                                {
                                    var response = await _friendRequestService.UpdateFriendRequestAsync(friendRequestDto);
                                    return Ok(response);
                                }
                            }   
                        }
                        return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                        {
                            StatusCode = 404,
                            IsSuccess = false,
                            Message = "Friend request not found"
                        });
                    }
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

        [HttpDelete("deleteFriendRequest/{friendRequestId}")]
        public async Task<IActionResult> DeleteFriendRequestAsync([FromRoute] Guid friendRequestId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
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
                        return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                        {
                            StatusCode = 404,
                            IsSuccess = false,
                            Message = "Friend request not found"
                        });
                    }
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

        [HttpDelete("unSendFriendRequest/{userWhoReceivedIdOrUserName}")]
        public async Task<IActionResult> UnSendFriendRequestAsync([FromRoute] string userWhoReceivedIdOrUserName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var sender = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var receiver = await GetUsetAsync(userWhoReceivedIdOrUserName);
                    if (sender != null && receiver != null)
                    {
                        var friendRequest = await _friendRequestRepository.GetFriendRequestByUserAndPersonIdAsync
                            (sender.Id, receiver.Id);
                        if (friendRequest == null || sender.Id == receiver.Id)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                            {
                                StatusCode = 403,
                                IsSuccess = false,
                                Message = "Forbidden"
                            });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
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
                        var routeUser = await GetUsetAsync(userIdOrUserName);
                        if (await _userManager.IsInRoleAsync(user, "Admin") || user.Id == routeUser.Id)
                        {
                            var response = await _friendRequestService.GetAllFriendRequestsByUserIdAsync(
                                routeUser.Id);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                        {
                            StatusCode = 403,
                            IsSuccess = false,
                            Message = "Forbidden"
                        });
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
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
        public FriendRequestsController(IFriendRequestService _friendRequestService,
            UserManager<SiteUser> _userManager, IFriendRequestRepository _friendRequestRepository)
        {
            this._friendRequestService = _friendRequestService;
            this._userManager = _userManager;
            this._friendRequestRepository = _friendRequestRepository;
        }

        [HttpPost("addFriendRequest")]
        public async Task<IActionResult> AddFriendRequestAsync([FromBody] FriendRequestDto friendRequestDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        friendRequestDto.UserId = user.Id;
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            var response = await _friendRequestService.AddFriendRequestAsync(friendRequestDto);
                            return Ok(response);
                        }  
                        if (user.Id == friendRequestDto.UserId)
                        {
                            var response = await _friendRequestService.AddFriendRequestAsync(friendRequestDto);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                        {
                            StatusCode = 403,
                            IsSuccess = false,
                            Message = "Forbidden"
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
        public async Task<IActionResult> UpdateFriendRequestAsync([FromBody] FriendRequestDto friendRequestDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        
                        if (friendRequestDto.Id != null)
                        {
                            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(
                            new Guid(friendRequestDto.Id));
                            friendRequestDto.UserId = friendRequest.UserId;
                            friendRequestDto.PersonId = friendRequest.PersonId;
                            if (await _userManager.IsInRoleAsync(user, "Admin"))
                            {
                                var response = await _friendRequestService.UpdateFriendRequestAsync(friendRequestDto);
                                return Ok(response);
                            }
                            if (user.Id == friendRequestDto.PersonId)
                            {
                                
                                var response = await _friendRequestService.UpdateFriendRequestAsync
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
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            var response = await _friendRequestService.DeleteFriendRequestByAsync(
                                user, friendRequestId);
                            return Ok(response);
                        }
                        var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
                        if (friendRequest != null)
                        {
                            if (user.Id == friendRequest.UserId)
                            {
                                var response = await _friendRequestService.DeleteFriendRequestByAsync(
                                    user, friendRequestId);
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
        [HttpGet("getAllFriendRequests")]
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

        [HttpGet("getAllFriendRequestsByUserId/{userId}")]
        public async Task<IActionResult> GetAllFriendsRequestsByUserIdAsync([FromRoute] string userId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Admin") || user.Id == userId)
                        {
                            var response = await _friendRequestService.GetAllFriendRequestsByUserIdAsync(userId);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                        {
                            StatusCode = 403,
                            IsSuccess = false,
                            Message = "Forbidden"
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

        [HttpGet("getAllFriendRequestsByUserName/{userName}")]
        public async Task<IActionResult> GetAllFriendsRequestsByUserNameAsync([FromRoute] string userName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Admin") || user.UserName == userName)
                        {
                            var response = await _friendRequestService.GetAllFriendRequestsByUserNameAsync
                                (userName);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                        {
                            StatusCode = 403,
                            IsSuccess = false,
                            Message = "Forbidden"
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

    }
}

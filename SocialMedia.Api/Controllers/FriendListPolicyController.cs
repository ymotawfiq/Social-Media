using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendListPolicyRepository;
using SocialMedia.Service.FriendListPolicyService;

namespace SocialMedia.Api.Controllers
{

    [ApiController]
    public class FriendListPolicyController : ControllerBase
    {
        private readonly IFriendListPolicyService _friendListPolicyService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IFriendListPolicyRepository _friendListPolicyRepository;
        public FriendListPolicyController(IFriendListPolicyService _friendListPolicyService,
            UserManager<SiteUser> _userManager, IFriendListPolicyRepository _friendListPolicyRepository)
        {
            this._friendListPolicyService = _friendListPolicyService;
            this._userManager = _userManager;
            this._friendListPolicyRepository = _friendListPolicyRepository;
        }


        [HttpGet("getFriendListPolicy")]
        public async Task<IActionResult> GetFriendListPolicyAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                    if (user != null)
                    {
                        var friendListPolicy = await _friendListPolicyRepository
                            .GetFriendListPolicyByUserIdAsync(user.Id);
                        if (friendListPolicy != null)
                        {
                            var response = await _friendListPolicyRepository.GetFriendListPolicyByUserIdAsync(
                                                                user.Id);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                        {
                            StatusCode = 404,
                            IsSuccess = false,
                            Message = "Friend list policy not found"
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

        [HttpPut("updateFriendListPolicy")]
        public async Task<IActionResult> UpdateFriendListPolicyAsync([FromBody]
        UpdateFriendListPolicyDto updateFriendListPolicyDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name!=null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    
                    if (user != null)
                    {
                        var friendListPolicy = await _friendListPolicyRepository
                            .GetFriendListPolicyByUserIdAsync(user.Id);
                        if (friendListPolicy != null)
                        {
                            if(friendListPolicy.UserId == user.Id)
                            {
                                var response = await _friendListPolicyService.UpdateFriendListPolicyAsync(
                                    updateFriendListPolicyDto);
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
                            Message = "Friend list policy not found"
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


    }
}

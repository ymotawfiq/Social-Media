using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Service.BlockService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IBlockService _blockService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly UserManagerReturn _userManagerReturn;
        public BlockController(IBlockService _blockService, UserManager<SiteUser> _userManager,
            UserManagerReturn _userManagerReturn)
        {
            this._blockService = _blockService;
            this._userManager = _userManager;
            this._userManagerReturn = _userManagerReturn;
        }


        [HttpPost("block")]
        public async Task<IActionResult> BlockAsync([FromBody] BlockUserDto blockUserDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var blockedUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync
                        (blockUserDto.UserIdOrUserNameOrEmail);
                    if (user != null && blockedUser != null)
                    {
                        if (user.Id != blockedUser.Id)
                        {
                            var blockDto = new BlockDto
                            {
                                BlockedUserId = blockedUser.Id,
                                UserId = user.Id
                            };
                            var response = await _blockService.BlockUserAsync(blockDto);
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
                        Message = "User you want to block not found"
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

        [HttpPut("unblock")]
        public async Task<IActionResult> UnBlockAsync([FromBody] BlockUserDto blockUserDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var blockedUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync
                        (blockUserDto.UserIdOrUserNameOrEmail);
                    if (user != null && blockedUser != null)
                    {
                        if (user.Id != blockedUser.Id)
                        {
                            var blockDto = new BlockDto
                            {
                                BlockedUserId = blockedUser.Id,
                                UserId = user.Id
                            };
                            var response = await _blockService.UnBlockUserAsync(blockDto);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                            ._403_Forbidden());
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User you want to block not found"));
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
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("blockList")]
        public async Task<IActionResult> BlockListAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                            var response = await _blockService.GetUserBlockListAsync(user.Id);
                            return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Message = "User you want to block not found"
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
}

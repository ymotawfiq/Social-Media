using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.UserSavedPostsFoldersService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class UserPostsFoldersController : ControllerBase
    {
        private readonly IUserSavedPostsFolderService _userSavedPostsFolderService;
        private readonly UserManager<SiteUser> _userManager;
        public UserPostsFoldersController(IUserSavedPostsFolderService _userSavedPostsFolderService,
            UserManager<SiteUser> _userManager)
        {
            this._userSavedPostsFolderService = _userSavedPostsFolderService;
            this._userManager = _userManager;
        }

        [HttpPost("addFolder")]
        public async Task<IActionResult> AddFolderAsync(
            [FromBody] AddUserSavedPostsFolderDto addUserSavedPostsFolderDto)
        {
            try
            {
                if(HttpContext.User !=null && HttpContext.User.Identity != null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _userSavedPostsFolderService.AddUserSavedPostsFoldersAsync(
                            user, addUserSavedPostsFolderDto);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
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

        [HttpPut("updateFolder")]
        public async Task<IActionResult> UpdateFolderAsync(
            [FromBody] UpdateUserSavedPostsFolderDto updateUserSavedPostsFolderDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _userSavedPostsFolderService.UpdateFolderNameAsync(
                            user, updateUserSavedPostsFolderDto);
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

        [HttpGet("getFolder/{folderId}")]
        public async Task<IActionResult> GetFolderAsync(
            [FromRoute] string folderId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _userSavedPostsFolderService
                            .GetUserSavedPostsFoldersByFolderIdAsync(user, folderId);
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

        [HttpDelete("deleteFolder/{folderId}")]
        public async Task<IActionResult> DeleteFolderAsync(
            [FromRoute] string folderId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _userSavedPostsFolderService
                            .DeleteUserSavedPostsFoldersByFolderIdAsync(user, folderId);
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

        [HttpGet("getFolders")]
        public async Task<IActionResult> GetUserFoldersAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _userSavedPostsFolderService
                            .GetUserFoldersByUserAsync(user);
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



    }
}

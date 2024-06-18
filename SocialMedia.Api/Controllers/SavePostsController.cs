using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.SavedPostsService;

namespace SocialMedia.Api.Api.Controllers
{
    [ApiController]
    public class SavePostsController : ControllerBase
    {
        private readonly ISavedPostsService _savedPostsService;
        private readonly UserManager<SiteUser> _userManager;
        public SavePostsController(ISavedPostsService _savedPostsService, UserManager<SiteUser> _userManager)
        {
            this._savedPostsService = _savedPostsService;
            this._userManager = _userManager;
        }

        [HttpPost("savePost")]
        public async Task<IActionResult> SavePostAsync([FromBody] SavePostDto savePostDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _savedPostsService.SavePostAsync(user, savePostDto);
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

        [HttpPut("unSavePost")]
        public async Task<IActionResult> UnSavePostAsync([FromBody] string postId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _savedPostsService.UnSavePostAsync(user, postId);
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

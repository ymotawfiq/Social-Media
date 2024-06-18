using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PagePostsService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class PagePostsController : ControllerBase
    {
        private readonly IPagePostsService _pagePostsService;
        private readonly UserManagerReturn _userManagerReturn;
        public PagePostsController(IPagePostsService _pagePostsService, UserManagerReturn _userManagerReturn)
        {
            this._pagePostsService = _pagePostsService;
            this._userManagerReturn = _userManagerReturn;
        }

        [HttpPost("addPagePost")]
        public async Task<IActionResult> AddPagePostAsync([FromForm] AddPagePostDto addPagePostDto)
        {
            try
            {
                if(HttpContext.User != null && HttpContext.User.Identity != null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _pagePostsService.AddPagePostAsync(addPagePostDto, user);
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

        [HttpDelete("deletePagePostById/{pagePostId}")]
        public async Task<IActionResult> DeletePagePostAsync([FromRoute] string pagePostId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _pagePostsService.DeletePagePostByIdAsync(pagePostId, user);
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


        [HttpDelete("deletePagePostByPostId/{postId}")]
        public async Task<IActionResult> DeletePagePostByPostIdAsync([FromRoute] string postId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _pagePostsService.DeletePagePostByPostIdAsync(postId, user);
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

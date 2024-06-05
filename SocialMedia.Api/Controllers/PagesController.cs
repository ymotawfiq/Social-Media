using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PageService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class PagesController : ControllerBase
    {

        private readonly IPageService _pageService;
        private readonly UserManagerReturn _userManagerReturn;

        public PagesController(IPageService _pageService, UserManagerReturn _userManagerReturn)
        {
            this._pageService = _pageService;
            this._userManagerReturn = _userManagerReturn;
        }


        [HttpPost("addPage")]
        public async Task<IActionResult> AddPageAsync([FromBody] AddPageDto addPageDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _pageService.AddPageAsync(addPageDto, user);
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

        [HttpPut("updatePage")]
        public async Task<IActionResult> UpdatePageAsync([FromBody] UpdatePageDto updatePageDto)
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
                        var response = await _pageService.UpdatePageAsync(updatePageDto, user);
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

        [HttpGet("page/{pageId}")]
        public async Task<IActionResult> GetPageByIdAsync([FromRoute] string pageId)
        {
            try
            {
                var response = await _pageService.GetPageByIdAsync(pageId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }


        [HttpDelete("deletePage/{pageId}")]
        public async Task<IActionResult> DeletePageByIdAsync([FromRoute] string pageId)
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
                        var response = await _pageService.DeletePageByIdAsync(pageId, user);
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


        [HttpGet("pages/{userId}")]
        public async Task<IActionResult> GetPagesByUserIdAsync([FromRoute] string userId)
        {
            try
            {
                var response = await _pageService.GetPagesByUserIdAsync(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getPages/{userName}")]
        public async Task<IActionResult> GetPagesByUserNameAsync([FromRoute] string userName)
        {
            try
            {
                var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userName);
                if (user != null)
                {
                    var response = await _pageService.GetPagesByUserIdAsync(user.Id);
                    return Ok(response);
                }
                return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }


    }
}

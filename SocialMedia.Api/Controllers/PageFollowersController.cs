using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PagesFollowersService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class PageFollowersController : ControllerBase
    {
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IPagesFollowersService _pagesFollowersService;
        public PageFollowersController(UserManagerReturn _userManagerReturn,
            IPagesFollowersService _pagesFollowersService)
        {
            this._pagesFollowersService = _pagesFollowersService;
            this._userManagerReturn = _userManagerReturn;
        }

        [HttpPost("followPage")]
        public async Task<IActionResult> FollowPageAsync(FollowPageDto followPageDto)
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
                        var response = await _pagesFollowersService.FollowPageAsync(followPageDto, user);
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

        [HttpPost("followPageByUser")]
        public async Task<IActionResult> FollowPageAsync(FollowPageUserDto followPageUserDto)
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
                        var response = await _pagesFollowersService.FollowPageAsync(followPageUserDto);
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

        [HttpPut("unFollowPageByPageId")]
        public async Task<IActionResult> UnFollowPageAsync(UnFollowPageDto unFollowPageDto)
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
                        var response = await _pagesFollowersService.UnFollowPageAsync(unFollowPageDto, user);
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

        [HttpPut("unFollowPageById/{Id}")]
        public async Task<IActionResult> UnFollowPageAsync(string Id)
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
                        var response = await _pagesFollowersService.UnFollowPageAsync(Id, user);
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

        [HttpPut("getPageFollowerByPageId/{pageId}")]
        public async Task<IActionResult> GetPageFollowerByPageIdAsync(string pageId)
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
                        var response = await _pagesFollowersService.GetPageFollowerAsync(pageId, user);
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

        [HttpGet("getPageFollowerById/{Id}")]
        public async Task<IActionResult> GetPageFollowersByIdAsync(string Id)
        {
            try
            {
                var response = await _pagesFollowersService.GetPageFollowerAsync(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getPageFollowersByPageId/{pageId}")]
        public async Task<IActionResult> GetPageFollowersByPageIdAsync(string pageId)
        {
            try
            {
                var response = await _pagesFollowersService.GetPageFollowersByPageIdAsync(pageId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }



    }
}

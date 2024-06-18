using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PostReactsService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class PostReactsController : ControllerBase
    {

        private readonly UserManager<SiteUser> _userManager;
        private readonly IPostReactsService _postReactsService;
        public PostReactsController(UserManager<SiteUser> _userManager, IPostReactsService _postReactsService)
        {
            this._postReactsService = _postReactsService;
            this._userManager = _userManager;
        }


        [HttpPost("addPostReact")]
        public async Task<IActionResult> AddPostReactAsync([FromBody] AddPostReactDto addPostReactDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postReactsService.AddPostReactAsync(addPostReactDto, user);
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

        [HttpPut("updatePostReact")]
        public async Task<IActionResult> UpdatePostReactAsync([FromBody] UpdatePostReactDto updatePostReactDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postReactsService.UpdatePostReactAsync(updatePostReactDto, user);
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

        [HttpGet("getPostReactById/{postReactId}")]
        public async Task<IActionResult> GetPostReactByIdAsync([FromRoute] string postReactId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postReactsService.GetPostReactByIdAsync(user, postReactId);
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

        [HttpGet("getPostReactByPostId/{PostId}")]
        public async Task<IActionResult> GetPostReactByPostIdAsync([FromRoute] string PostId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postReactsService.GetPostReactByUserIdAndPostIdAsync
                            (user.Id, PostId);
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

        [HttpGet("deletePostReactById/{postReactId}")]
        public async Task<IActionResult> DeletePostReactByIdAsync([FromRoute] string postReactId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postReactsService.DeletePostReactByIdAsync(postReactId, user);
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

        [HttpDelete("DeletePostReactByPostId/{PostId}")]
        public async Task<IActionResult> DeletePostReactByPostIdAsync([FromRoute] string PostId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postReactsService.DeletePostReactByUserIdAndPostIdAsync
                            (user.Id, PostId);
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

        [HttpGet("getPostReactsByPostId/{postId}")]
        public async Task<IActionResult> GetPostReactsAsync([FromRoute] string postId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postReactsService.GetPostReactsByPostIdAsync(postId, user);
                        return Ok(response);
                    }
                }
                var response1 = await _postReactsService.GetPostReactsByPostIdAsync(postId);
                return Ok(response1);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                            ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getPostReactsByUser")]
        public async Task<IActionResult> GetPostReactByUserAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postReactsService.GetPostReactsByUserIdAsync(user.Id);
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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.BlockService;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PostService;

namespace SocialMedia.Api.Controllers
{

    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly IUserPostsRepository _userPostsRepository;
        private readonly IBlockService _blockService;
        private readonly UserManagerReturn _userManagerReturn;
        public PostController(IPostService _postService, UserManager<SiteUser> _userManager,
            IPostRepository _postRepository,
            IUserPostsRepository _userPostsRepository, IBlockService _blockService, 
             UserManagerReturn _userManagerReturn)
        {
            this._postService = _postService;
            this._userManager = _userManager;
            this._postRepository = _postRepository;
            this._userPostsRepository = _userPostsRepository;
            this._blockService = _blockService;
            this._userManagerReturn = _userManagerReturn;
        }

        [HttpPost("post")]
        public async Task<IActionResult> PosTAsync([FromForm] AddPostDto createPostDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name!=null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postService.AddPostAsync(user, createPostDto);
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


        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetPostByIdASync([FromRoute] string postId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (currentUser != null)
                    {
                        var post = await _postRepository.IsPostExistsAsync(postId);
                        if (post != null)
                        {
                            var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                            if (userPost != null)
                            {
                                var routeUser = await _userManager.FindByIdAsync(userPost.UserId);
                                if (routeUser != null)
                                {
                                    var response = await _postService
                                        .GetPostByIdAsync(currentUser, routeUser, post.Id);
                                    return Ok(response);
                                }
                            }
                            return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                                ._404_NotFound("User post not found"));
                        }
                    }
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

        [HttpDelete("post/{postId}")]
        public async Task<IActionResult> DeletePostByIdASync([FromRoute] string postId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    
                    if (user != null)
                    {
                        var userPost = await _userPostsRepository.GetUserPostByIdAsync(postId);
                        if (userPost != null)
                        {
                            var response = await _postService.DeletePostAsync(user, postId);
                            return Ok(response);
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                            ._403_Forbidden());
                    }
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



        [HttpGet("posts/{userIdOrUserName}")]
        public async Task<IActionResult> GetUserPostsAsync([FromRoute] string userIdOrUserName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var routeUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        userIdOrUserName);
                    if (currentUser != null && routeUser != null)
                    {
                        var isBlocked = await _blockService
                            .GetBlockByUserIdAndBlockedUserIdAsync(routeUser.Id, currentUser.Id);
                        if (isBlocked.ResponseObject != null)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                                ._403_Forbidden());
                        }
                        var response = await _postService.GetUserPostsAsync(routeUser);
                        if (currentUser.Id == routeUser.Id)
                        {
                            return Ok(response);
                        }
                        else
                        {
                            var response1 = await _postService
                                .CheckFriendShipAndGetPostsAsync(currentUser, routeUser);
                            return Ok(response1);
                        }
                    }
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

        [HttpGet("posts")]
        public async Task<IActionResult> GetUserPostsAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (currentUser != null)
                    {
                        var response = await _postService.GetUserPostsAsync(currentUser);
                        return Ok(response);
                    }
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


        [HttpPut("updatePost")]
        public async Task<IActionResult> UpdatePostAsync([FromForm] UpdatePostDto updatePostDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var post = await _postRepository.IsPostExistsAsync(updatePostDto.PostId);
                    if (user != null && post != null)
                    {
                        var response = await _postService.UpdatePostAsync(user, updatePostDto);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                    ._404_NotFound("Post not found"));
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

        [HttpPut("updatePostPolicy")]
        public async Task<IActionResult> UpdatePostPolicyAsync([FromBody] UpdatePostPolicyDto updatePostPolicyDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name!=null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postService.UpdatePostPolicyAsync(user, updatePostPolicyDto);
                        return Ok(response);
                    }
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

        [HttpPut("updatePostReactPolicy")]
        public async Task<IActionResult> UpdatePostReactPolicyAsync
            ([FromBody] UpdatePostReactPolicyDto updatePostReactPolicyDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postService
                            .UpdatePostReactPolicyAsync(user, updatePostReactPolicyDto);
                        return Ok(response);
                    }
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

        [HttpPut("updatePostCommentPolicy")]
        public async Task<IActionResult> UpdatePostCommentPolicyAsync
            ([FromBody] UpdatePostCommentPolicyDto updatePostCommentPolicyDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _postService
                            .UpdatePostCommentPolicyAsync(user, updatePostCommentPolicyDto);
                        return Ok(response);
                    }
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

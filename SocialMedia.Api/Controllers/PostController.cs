using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.ReactPolicyRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.BlockService;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.PostService;

namespace SocialMedia.Api.Controllers
{

    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserPostsRepository _userPostsRepository;
        private readonly IFriendService _friendService;
        private readonly IBlockService _blockService;
        public PostController(IPostService _postService, UserManager<SiteUser> _userManager,
            IPostRepository _postRepository, IPolicyRepository _policyRepository,
            ApplicationDbContext _dbContext, IUserPostsRepository _userPostsRepository,
            IFriendService _friendService, IBlockService _blockService)
        {
            this._postService = _postService;
            this._userManager = _userManager;
            this._postRepository = _postRepository;
            this._policyRepository = _policyRepository;
            this._dbContext = _dbContext;
            this._userPostsRepository = _userPostsRepository;
            this._friendService = _friendService;
            this._blockService = _blockService;
        }

        [HttpPost("post")]
        public async Task<IActionResult> PosTAsync([FromForm] CreatePostDto createPostDto)
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
                            var routeUser = await _userManager.FindByIdAsync(userPost.UserId);
                            if (routeUser != null)
                            {
                                var response = await _postService
                                    .GetPostByIdAsync(currentUser, routeUser, post.Id);
                                return Ok(response);
                            }
                        }
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



        [HttpGet("posts/{userIdOrUserName}")]
        public async Task<IActionResult> GetUserPostsAsync([FromRoute] string userIdOrUserName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var routeUser = await GetUserAsync(userIdOrUserName);
                    if (currentUser != null && routeUser != null)
                    {
                        var isBlocked = await _blockService
                            .GetBlockByUserIdAndBlockedUserIdAsync(routeUser.Id, currentUser.Id);
                        if (isBlocked.ResponseObject != null)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                            {
                                StatusCode = 403,
                                IsSuccess = false,
                                Message = "Forbidden"
                            });
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
                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Message = "Post not found"
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


        private async Task<SiteUser> GetUserAsync(string userIdOrUserName)
        {
            var userById = await _userManager.FindByIdAsync(userIdOrUserName);
            var userByUserName = await _userManager.FindByNameAsync(userIdOrUserName);
            return userById == null ? userByUserName! : userById;
        }

    }
}

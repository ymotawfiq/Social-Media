

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.PostReactsRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.PostReactsService
{
    public class PostReactsService : IPostReactsService
    {
        private readonly IPostReactsRepository _postReactsRepository;
        private readonly IPostRepository _postRepository;
        private readonly IReactRepository _reactRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IUserPostsRepository _userPostsRepository;
        public PostReactsService(IPostReactsRepository _postReactsRepository,
            IPostRepository _postRepository,
            IReactRepository _reactRepository, IBlockRepository _blockRepository,
            IUserPostsRepository _userPostsRepository)
        {
            this._postReactsRepository = _postReactsRepository;
            this._postRepository = _postRepository;
            this._reactRepository = _reactRepository;
            this._blockRepository = _blockRepository;
            this._userPostsRepository = _userPostsRepository;
        }
        public async Task<ApiResponse<PostReacts>> AddPostReactAsync(AddPostReactDto addPostReactDto,
            SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(addPostReactDto.PostId);
            if (post != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                if (userPost != null)
                {
                    var isBlockedUser = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    user.Id, userPost.UserId);
                    if (isBlockedUser != null)
                    {
                        var react = await _reactRepository.GetReactByIdAsync(addPostReactDto.ReactId);
                        if (react != null)
                        {
                            var existPostReact = await _postReactsRepository
                                .GetPostReactByUserIdAndPostIdAsync(user.Id, addPostReactDto.PostId);
                            if (existPostReact == null)
                            {
                                var newPostReact = await _postReactsRepository.AddPostReactAsync(
                                ConvertFromDto.ConvertFromPostReactsDto_Add(addPostReactDto, user));
                                return StatusCodeReturn<PostReacts>
                                    ._201_Created("Post react added successfully", newPostReact);
                            }
                            return StatusCodeReturn<PostReacts>
                                    ._403_Forbidden("Post react already exists");
                        }
                        return StatusCodeReturn<PostReacts>
                            ._404_NotFound("React not found");
                    }
                    return StatusCodeReturn<PostReacts>
                    ._403_Forbidden();
                }
                return StatusCodeReturn<PostReacts>
                    ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<PostReacts>> DeletePostReactByIdAsync(string Id)
        {
            var postReact = await _postReactsRepository.GetPostReactByIdAsync(Id);
            if (postReact != null)
            {
                await _postReactsRepository.DeletePostReactByIdAsync(Id);
                return StatusCodeReturn<PostReacts>
                    ._200_Success("Post react deleted successfully");
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post react not found");
        }

        public async Task<ApiResponse<PostReacts>> DeletePostReactByUserIdAndPostIdAsync(string userId,
            string postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post != null)
            {
                var postReact = await _postReactsRepository.GetPostReactByUserIdAndPostIdAsync(userId, postId);
                if (postReact != null)
                {
                    await _postReactsRepository.DeletePostReactByUserIdAndPostIdAsync(userId, postId);
                    return StatusCodeReturn<PostReacts>
                        ._200_Success("Post react deleted successfully", postReact);
                }
                return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post react not found");
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<PostReacts>> GetPostReactByIdAsync(SiteUser user, string Id)
        {
            var postReact = await _postReactsRepository.GetPostReactByIdAsync(Id);
            if (postReact != null)
            {
                var post = await _postRepository.GetPostByIdAsync(postReact.PostId);
                if (post != null)
                {
                    var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                    if (userPost != null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            user.Id, userPost.UserId);
                        if (isBlocked == null)
                        {
                            return StatusCodeReturn<PostReacts>
                                ._200_Success("Post react found successfully");
                        }
                        return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostReacts>
                    ._404_NotFound("User post not found");
                }
                return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post react not found");
        }

        public async Task<ApiResponse<PostReacts>> GetPostReactByUserIdAndPostIdAsync(string userId,
            string postId)
        {
            var postReact = await _postReactsRepository.GetPostReactByUserIdAndPostIdAsync
                (userId, postId);
            if (postReact != null)
            {
                var post = await _postRepository.GetPostByIdAsync(postReact.PostId);
                if (post != null)
                {
                    var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                    if (userPost != null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            userId, userPost.UserId);
                        if (isBlocked == null)
                        {
                            return StatusCodeReturn<PostReacts>
                                ._200_Success("Post react found successfully");
                        }
                        return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostReacts>
                    ._404_NotFound("User post not found");
                }
                return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post react not found");
        }

        public async Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByPostIdAsync(
            string postId, SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(postId);
                if (userPost != null)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, userPost.UserId);
                    if (isBlocked == null)
                    {
                        var postReact = await _postReactsRepository.GetPostReactsByPostIdAsync(postId);
                        if (postReact.ToList().Count == 0)
                        {
                            return StatusCodeReturn<IEnumerable<PostReacts>>
                                ._200_Success("No post reacts found", postReact);
                        }
                        return StatusCodeReturn<IEnumerable<PostReacts>>
                                ._200_Success("Post reacts found successfully", postReact);
                    }
                    return StatusCodeReturn<IEnumerable<PostReacts>>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<IEnumerable<PostReacts>>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<IEnumerable<PostReacts>>
                        ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByPostIdAsync(string postId)
        {
            var postReact = await _postReactsRepository.GetPostReactsByPostIdAsync(postId);
            if (postReact.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostReacts>>
                    ._200_Success("No post reacts found", postReact);
            }
            return StatusCodeReturn<IEnumerable<PostReacts>>
                    ._200_Success("Post reacts found successfully", postReact);
        }

        public async Task<ApiResponse<PostReacts>> UpdatePostReactAsync(UpdatePostReactDto updatePostReactDto,
            SiteUser user)
        {
            var postReact = await _postReactsRepository.GetPostReactByIdAsync(updatePostReactDto.Id);
            if (postReact != null)
            {
                var post = await _postRepository.GetPostByIdAsync(updatePostReactDto.PostId);
                if (post != null)
                {
                    var react = await _reactRepository.GetReactByIdAsync(updatePostReactDto.ReactId);
                    if (react != null)
                    {
                        postReact.ReactId = updatePostReactDto.ReactId;
                        postReact = await _postReactsRepository.UpdatePostReactAsync(postReact);
                        return StatusCodeReturn<PostReacts>
                            ._200_Success("Post react updated successfully", postReact);
                    }
                    return StatusCodeReturn<PostReacts>
                        ._404_NotFound("React not found");
                }
                return StatusCodeReturn<PostReacts>
                        ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostReacts>
                        ._404_NotFound("Post react not found");
        }
        
    }
}

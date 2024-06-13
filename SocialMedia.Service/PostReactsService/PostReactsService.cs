

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostReactsRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Repository.SpecialPostsReactsRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.FriendsService;
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
        private readonly IPolicyRepository _policyRepository;
        private readonly IFriendService _friendService;
        private readonly ISpecialPostsReactsRepository _specialPostsReactsRepository;
        public PostReactsService(IPostReactsRepository _postReactsRepository,
            IPostRepository _postRepository, 
            IReactRepository _reactRepository, IBlockRepository _blockRepository,
            IUserPostsRepository _userPostsRepository, IPolicyRepository _policyRepository,
            IFriendService _friendService, ISpecialPostsReactsRepository _specialPostsReactsRepository)
        {
            this._postReactsRepository = _postReactsRepository;
            this._postRepository = _postRepository;
            this._reactRepository = _reactRepository;
            this._blockRepository = _blockRepository;
            this._userPostsRepository = _userPostsRepository;
            this._policyRepository = _policyRepository;
            this._friendService = _friendService;
            this._specialPostsReactsRepository = _specialPostsReactsRepository;
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
                        var postReact = await _specialPostsReactsRepository.GetSpecialPostReactsByReactIdAsync(
                            addPostReactDto.ReactId);
                        if (postReact != null)
                        {
                            var checkPolicy = await CheckPolicyAsync(userPost.UserId, user.Id, post.Id);
                            if (checkPolicy.IsSuccess)
                            {
                                var existPostReact = await _postReactsRepository
                                .GetPostReactByUserIdAndPostIdAsync(user.Id, addPostReactDto.PostId);
                                if (existPostReact == null)
                                {
                                    addPostReactDto.ReactId = postReact.Id;
                                    var newPostReact = await _postReactsRepository.AddPostReactAsync(
                                    ConvertFromDto.ConvertFromPostReactsDto_Add(addPostReactDto, user));
                                    return StatusCodeReturn<PostReacts>
                                        ._201_Created("Post react added successfully", newPostReact);
                                }
                                return StatusCodeReturn<PostReacts>
                                        ._403_Forbidden("Post react already exists");
                            }
                            return StatusCodeReturn<PostReacts>
                                ._403_Forbidden();
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

        public async Task<ApiResponse<PostReacts>> DeletePostReactByIdAsync(string Id, SiteUser user)
        {
            var postReact = await _postReactsRepository.GetPostReactByIdAsync(Id);
            if (postReact != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(postReact.PostId);
                if (userPost != null)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    user.Id, userPost.UserId);
                    if (isBlocked == null)
                    {
                        if (user.Id == postReact.UserId)
                        {
                            await _postReactsRepository.DeletePostReactByIdAsync(Id);
                            return StatusCodeReturn<PostReacts>
                                ._200_Success("Post react deleted successfully");
                        }
                        return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                }
                return StatusCodeReturn<PostReacts>
                    ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post react not found");
        }

        public async Task<ApiResponse<PostReacts>> DeletePostReactByUserIdAndPostIdAsync(string userId,
            string postId)
        {
            var postReact = await _postReactsRepository.GetPostReactByUserIdAndPostIdAsync(userId, postId);
            if (postReact != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(postReact.PostId);
                if (userPost != null)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    userId, userPost.UserId);
                    if (isBlocked == null)
                    {
                        if (userId == postReact.UserId)
                        {
                            await _postReactsRepository.DeletePostReactByIdAsync(postReact.Id);
                            return StatusCodeReturn<PostReacts>
                                ._200_Success("Post react deleted successfully");
                        }
                        return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                }
                return StatusCodeReturn<PostReacts>
                    ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post react not found");
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

        public async Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByUserIdAsync(string userId)
        {
            var postReact = await _postReactsRepository.GetPostReactsByUserIdAsync(userId);
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
                    var react = await _specialPostsReactsRepository.GetSpecialPostReactsByReactIdAsync(
                        updatePostReactDto.ReactId);
                    if (react != null)
                    {
                        if(postReact.UserId == user.Id)
                        {
                            postReact.SpecialPostReactId = react.Id;
                            postReact = await _postReactsRepository.UpdatePostReactAsync(postReact);
                            return StatusCodeReturn<PostReacts>
                                ._200_Success("Post react updated successfully", postReact);
                        }
                        return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostReacts>
                        ._404_NotFound("Special post react not found");
                }
                return StatusCodeReturn<PostReacts>
                        ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostReacts>
                        ._404_NotFound("Post react not found");
        }

        private async Task<ApiResponse<PostReacts>> CheckPolicyAsync(string userId,
            string userWhoWantsToReactId, string postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post != null)
            {
                var policy = await _policyRepository.GetPolicyByIdAsync(post.ReactPolicyId);
                if (policy != null)
                {
                    if (policy.PolicyType == "PRIVATE")
                    {
                        return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                    }
                    else if (policy.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await _friendService.IsUserFriendAsync(userId,
                            userWhoWantsToReactId);
                        if (isFriend == null || !isFriend!.ResponseObject)
                        {
                            return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                        }
                    }
                    else if (policy.PolicyType == "FRIENDS OF FRIENDS")
                    {
                        var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(userId,
                            userWhoWantsToReactId);
                        if (isFriendOfFriend == null || !isFriendOfFriend!.ResponseObject)
                        {
                            return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                        }
                    }
                    return StatusCodeReturn<PostReacts>
                        ._200_Success("You can react", new PostReacts { });
                }
                return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<PostReacts>
                        ._404_NotFound("Post not found");
        } 

    }
}

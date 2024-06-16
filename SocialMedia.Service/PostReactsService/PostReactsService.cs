

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
        private readonly IPolicyRepository _policyRepository;
        private readonly IFriendService _friendService;
        public PostReactsService(
            IPostReactsRepository _postReactsRepository,
            IPostRepository _postRepository, 
            IReactRepository _reactRepository, IBlockRepository _blockRepository,
            IPolicyRepository _policyRepository,
            IFriendService _friendService)
        {
            this._postReactsRepository = _postReactsRepository;
            this._postRepository = _postRepository;
            this._reactRepository = _reactRepository;
            this._blockRepository = _blockRepository;
            this._policyRepository = _policyRepository;
            this._friendService = _friendService;
        }

        public async Task<ApiResponse<PostReacts>> AddPostReactAsync(AddPostReactDto addPostReactDto,
            SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(addPostReactDto.PostId);
            if ((await CheckToReactAsync<PostReacts>(user.Id, post)).IsSuccess)
            {
                var postReact = await _reactRepository.GetByIdAsync(addPostReactDto.ReactId);
                if (postReact != null)
                {
                    var checkPolicy = await CheckPolicyAsync(post.UserId, user.Id, post);
                    if (checkPolicy.IsSuccess)
                    {
                        var existPostReact = await _postReactsRepository
                        .GetPostReactByUserIdAndPostIdAsync(user.Id, addPostReactDto.PostId);
                        if (existPostReact == null)
                        {
                            addPostReactDto.ReactId = postReact.Id;
                            var newPostReact = await _postReactsRepository.AddAsync(
                            ConvertFromDto.ConvertFromPostReactsDto_Add(addPostReactDto, user));
                            return StatusCodeReturn<PostReacts>
                                ._201_Created("Reacted successfully", newPostReact);
                        }
                        return StatusCodeReturn<PostReacts>
                                ._403_Forbidden("Already reacted with post");
                    }
                    return StatusCodeReturn<PostReacts>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostReacts>
                    ._404_NotFound("React not found");
            }
            return await CheckToReactAsync<PostReacts>(user.Id, post);
        }

        public async Task<ApiResponse<PostReacts>> DeletePostReactByIdAsync(string Id, SiteUser user)
        {
            var postReact = await _postReactsRepository.GetByIdAsync(Id);
            if (postReact != null)
            {
                var post = await _postRepository.GetPostByIdAsync(postReact.PostId);
                if ((await CheckToReactAsync<PostReacts>(user.Id, post)).IsSuccess)
                {
                    if (user.Id == postReact.UserId)
                    {
                        await _postReactsRepository.DeleteByIdAsync(Id);
                        return StatusCodeReturn<PostReacts>
                            ._200_Success("Post react deleted successfully");
                    }
                    return StatusCodeReturn<PostReacts>
                        ._403_Forbidden();
                }
                return await CheckToReactAsync<PostReacts>(user.Id, post);
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<PostReacts>> DeletePostReactByUserIdAndPostIdAsync(string userId,
            string postId)
        {
            var postReact = await _postReactsRepository.GetPostReactByUserIdAndPostIdAsync(userId, postId);
            if (postReact != null)
            {
                var post = await _postRepository.GetPostByIdAsync(postReact.PostId);
                if((await CheckToReactAsync<PostReacts>(userId, post)).IsSuccess)
                {
                    if (userId == postReact.UserId)
                    {
                        await _postReactsRepository.DeleteByIdAsync(postReact.Id);
                        return StatusCodeReturn<PostReacts>
                            ._200_Success("Post react deleted successfully");
                    }
                    return StatusCodeReturn<PostReacts>
                        ._403_Forbidden();
                }
                return await CheckToReactAsync<PostReacts>(userId, post);
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post react not found");
        }

        public async Task<ApiResponse<PostReacts>> GetPostReactByIdAsync(SiteUser user, string Id)
        {
            var postReact = await _postReactsRepository.GetByIdAsync(Id);
            if (postReact != null)
            {
                var post = await _postRepository.GetPostByIdAsync(postReact.PostId);
                if ((await CheckToReactAsync<PostReacts>(user.Id, post)).IsSuccess)
                {
                    return StatusCodeReturn<PostReacts>
                                ._200_Success("Post react found successfully", postReact);
                }
                return await CheckToReactAsync<PostReacts>(user.Id, post);
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
                if ((await CheckToReactAsync<PostReacts>(userId, post)).IsSuccess)
                {
                    return StatusCodeReturn<PostReacts>
                                ._200_Success("Post react found successfully", postReact);
                }
                return await CheckToReactAsync<PostReacts>(userId, post);
            }
            return StatusCodeReturn<PostReacts>
                    ._404_NotFound("Post react not found");
        }

        public async Task<ApiResponse<IEnumerable<PostReacts>>> GetPostReactsByPostIdAsync(
            string postId, SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if ((await CheckToReactAsync<PostReacts>(user.Id, post)).IsSuccess)
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
            return await CheckToReactAsync<IEnumerable<PostReacts>>(user.Id, post);
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
            var postReact = await _postReactsRepository.GetByIdAsync(updatePostReactDto.Id);
            if (postReact != null)
            {
                var post = await _postRepository.GetPostByIdAsync(updatePostReactDto.PostId);
                if ((await CheckToReactAsync<PostReacts>(user.Id, post)).IsSuccess)
                {
                    if (postReact.UserId == user.Id)
                    {
                        var react = await _reactRepository.GetByIdAsync(updatePostReactDto.ReactId);
                        if (react != null)
                        {
                            postReact.PostReactId = updatePostReactDto.ReactId;
                            postReact = await _postReactsRepository.UpdateAsync(postReact);
                            return StatusCodeReturn<PostReacts>
                                ._200_Success("Post react updated successfully", postReact);
                        }
                        return StatusCodeReturn<PostReacts>
                        ._404_NotFound("React not found");
                    }
                    return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                }
                return await CheckToReactAsync<PostReacts>(user.Id, post);
            }
            return StatusCodeReturn<PostReacts>
                        ._404_NotFound("Post react not found");
        }

        private async Task<ApiResponse<PostReacts>> CheckPolicyAsync(string userId,
            string userWhoWantsToReactId, Post post)
        {
            var policy = await _policyRepository.GetByIdAsync(post.ReactPolicyId);
            if (policy != null)
            {
                if(userId != userWhoWantsToReactId)
                {
                    if (policy.PolicyType == "PRIVATE")
                    {
                        return StatusCodeReturn<PostReacts>
                            ._403_Forbidden();
                    }
                    else if (policy.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await _friendService.IsUserFriendAsync(userId,userWhoWantsToReactId);
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
                }
                return StatusCodeReturn<PostReacts>
                    ._200_Success("You can react", new PostReacts { });
            }
            return StatusCodeReturn<PostReacts>
                ._404_NotFound("Policy not found");
        }


        private async Task<ApiResponse<T>> CheckToReactAsync<T>(string userId, Post post)
        {
            if (post != null)
            {
                var isBlockedUser = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                userId, post.UserId);
                if (isBlockedUser == null)
                {
                    return StatusCodeReturn<T>
                    ._200_Success("Success");
                }
                return StatusCodeReturn<T>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<T>
                    ._404_NotFound("Post not found");
        }

    }
}

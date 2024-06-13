

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostCommentReplayRepository;
using SocialMedia.Repository.PostCommentsRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.GenericReturn;
using System.Collections.Generic;

namespace SocialMedia.Service.PostCommentReplayService
{
    public class PostCommentReplayService : IPostCommentReplayService
    {
        private readonly IPostCommentReplayRepository _postCommentReplayRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IPostCommentsRepository _postCommentsRepository;
        private readonly IUserPostsRepository _userPostsRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IFriendService _friendService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PostCommentReplayService(
            IPostCommentReplayRepository _postCommentReplayRepository,
            IBlockRepository _blockRepository,
            IPostCommentsRepository _postCommentsRepository, IUserPostsRepository _userPostsRepository,
            IPostRepository _postRepository, IPolicyRepository _policyRepository,
            IFriendService _friendService, IWebHostEnvironment _webHostEnvironment
            )
        {
            this._blockRepository = _blockRepository;
            this._postCommentReplayRepository = _postCommentReplayRepository;
            this._postCommentsRepository = _postCommentsRepository;
            this._userPostsRepository = _userPostsRepository;
            this._postRepository = _postRepository;
            this._policyRepository = _policyRepository;
            this._friendService = _friendService;
            this._webHostEnvironment = _webHostEnvironment;
        }
        public async Task<ApiResponse<PostCommentReplay>> AddCommentReplayAsync(
            AddPostCommentReplayDto addPostCommentReplayDto, SiteUser user)
        {
            var comment = await _postCommentsRepository.GetPostCommentByIdAsync(
                addPostCommentReplayDto.PostCommentId);
            if (comment != null)
            {
                var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                if (post != null)
                {
                    var userPost = await _userPostsRepository.GetUserPostByIdAsync(post.Id);
                    if (userPost != null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            user.Id, userPost.UserId);
                        if (isBlocked == null)
                        {
                            var checkPolicy = await CheckPolicyAsync(post, user);
                            if (checkPolicy.IsSuccess)
                            {
                                var newCommentReplay = await _postCommentReplayRepository
                                    .AddPostCommentReplayAsync(ConvertFromDto
                                    .ConvertFromPostCommentReplayDto_Add(addPostCommentReplayDto,
                                    user, SavePostImages(addPostCommentReplayDto.ReplayImage!)));
                                MakeUnnesseryObjectsInReplaysNull(newCommentReplay);
                                return StatusCodeReturn<PostCommentReplay>
                                    ._201_Created("Replayed successfully", newCommentReplay);
                            }
                            return StatusCodeReturn<PostCommentReplay>
                                ._403_Forbidden();
                        }
                        return StatusCodeReturn<PostCommentReplay>
                                ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("User post not found");
                }
                return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Comment not found");
        }

        public async Task<ApiResponse<PostCommentReplay>> AddReplayToReplayAsync(
            AddReplayToReplayCommentDto addReplayToReplayCommentDto, SiteUser user)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                addReplayToReplayCommentDto.CommentReplayId);
            if (replay != null)
            {
                var isAbleToReplay = await IsAbleToReplayToCommentAsync(user, replay);
                if (isAbleToReplay.IsSuccess)
                {
                    var newCommentReplay = await _postCommentReplayRepository.AddPostCommentReplayAsync(
                        ConvertFromDto.ConvertFromCommentReplayToReplayDto_Add(addReplayToReplayCommentDto,
                        user, SavePostImages(addReplayToReplayCommentDto.ReplayImage!),
                        isAbleToReplay.ResponseObject!.PostCommentId));
                    MakeUnnesseryObjectsInReplaysNull(newCommentReplay);
                    return StatusCodeReturn<PostCommentReplay>
                        ._201_Created("Replayed successfully", newCommentReplay);
                }
                return isAbleToReplay;
            }
            return StatusCodeReturn<PostCommentReplay>
                                    ._404_NotFound("Comment replay not found");
        }

        public async Task<ApiResponse<PostCommentReplay>> DeleteCommentReplayByIdAsync(
            string commentReplayById, SiteUser user)
        {
            var commentReplay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                commentReplayById);
            if (commentReplay != null)
            {
                var isAbleToDelete = await IsAbleToOperateCommentReplayAsync(user, commentReplay);
                if (isAbleToDelete.IsSuccess)
                {
                    await _postCommentReplayRepository.DeletePostCommentReplayByIdAsync(commentReplayById);
                    MakeUnnesseryObjectsInReplaysNull(commentReplay);
                    return StatusCodeReturn<PostCommentReplay>
                        ._200_Success("Comment replay deleted successfully", commentReplay);
                }
                return isAbleToDelete;
            }
            return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Comment replay not found");
        }

        public async Task<ApiResponse<PostCommentReplay>> DeleteReplayToReplayByIdAsync(
            string commentReplayToReplayById, SiteUser user)
        {
            var commentReplay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                commentReplayToReplayById);
            if (commentReplay != null)
            {
                var isAbleToDelete = await IsAbleToOperateCommentReplayAsync(user, commentReplay);
                if (isAbleToDelete.IsSuccess)
                {
                    await _postCommentReplayRepository.DeletePostCommentReplayByIdAsync(
                        commentReplayToReplayById);
                    MakeUnnesseryObjectsInReplaysNull(commentReplay);
                    return StatusCodeReturn<PostCommentReplay>
                        ._200_Success("Comment replay deleted successfully", commentReplay);
                }
                return isAbleToDelete;
            }
            return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Comment replay not found");
        }

        public async Task<ApiResponse<PostCommentReplay>> DeleteReplayImageAsync(
            string commentReplayToReplayById, SiteUser user)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                commentReplayToReplayById);
            if (replay != null)
            {
                var isAbleToDelete = await IsAbleToOperateCommentReplayAsync(user, replay);
                if (isAbleToDelete.IsSuccess)
                {
                    DeletePostImage(replay.ReplayImage!);
                    MakeUnnesseryObjectsInReplaysNull(replay);
                    return StatusCodeReturn<PostCommentReplay>
                        ._200_Success("Comment replay image deleted successfully");
                }
                return isAbleToDelete;
            }
            return StatusCodeReturn<PostCommentReplay>
                ._404_NotFound("Comment replay not found");
        }

        public async Task<ApiResponse<PostCommentReplay>> GetCommentReplayByIdAsync(
            string commentReplayById, SiteUser user)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(commentReplayById);
            if (replay != null)
            {
                var comment = await _postCommentsRepository.GetPostCommentByIdAsync(replay.PostCommentId);
                if (comment != null)
                {
                    var isAble = await CheckAbilityToGetCommentReplayAsync(comment, user);
                    if (isAble.IsSuccess)
                    {
                        MakeUnnesseryObjectsInReplaysNull(replay);
                        return StatusCodeReturn<PostCommentReplay>
                            ._200_Success("Comment replay found successfully", replay);
                    }
                    return StatusCodeReturn<PostCommentReplay>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostCommentReplay>
                                ._404_NotFound("Comment not found");
            }
            return StatusCodeReturn<PostCommentReplay>
                                ._404_NotFound("Comment replay not found");
        }


        public async Task<ApiResponse<PostCommentReplay>> GetReplayToReplayByIdAsync(
            string commentReplayToReplayById, SiteUser user)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                commentReplayToReplayById);
            if (replay != null)
            {
                var comment = await _postCommentsRepository.GetPostCommentByIdAsync(replay.PostCommentId);
                if (comment != null)
                {
                    var isAble = await CheckAbilityToGetCommentReplayAsync(comment, user);
                    if (isAble.IsSuccess)
                    {
                        MakeUnnesseryObjectsInReplaysNull(replay);
                        return StatusCodeReturn<PostCommentReplay>
                            ._200_Success("Comment replay found successfully", replay);
                    }
                    return StatusCodeReturn<PostCommentReplay>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Comment not found");
            }
            return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Comment replay not found");
        }

        public async Task<ApiResponse<PostCommentReplay>> GetCommentReplayByIdAsync(string commentReplayById)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                commentReplayById);
            if (replay != null)
            {
                var isAble = await CheckIfPostPolicyIsPublicAsync(replay);
                if (isAble.IsSuccess)
                {
                    MakeUnnesseryObjectsInReplaysNull(replay);
                    return StatusCodeReturn<PostCommentReplay>
                        ._200_Success("Replay found successfully", replay);
                }
                return isAble;
            }
            return StatusCodeReturn<PostCommentReplay>
                ._404_NotFound("Replay not found");
        }

        public async Task<ApiResponse<PostCommentReplay>> GetReplayToReplayByIdAsync(
            string commentReplayToReplayById)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                commentReplayToReplayById);
            if (replay != null)
            {
                var isAble = await CheckIfPostPolicyIsPublicAsync(replay);
                if (isAble.IsSuccess)
                {
                    MakeUnnesseryObjectsInReplaysNull(replay);
                    return StatusCodeReturn<PostCommentReplay>
                        ._200_Success("Replay found successfully", replay);
                }
                return isAble;
            }
            return StatusCodeReturn<PostCommentReplay>
                ._404_NotFound("Replay not found");
        }

        public async Task<ApiResponse<IEnumerable<PostCommentReplay>>> GetCommentReplaysByCommentIdAsync(
            string commentId)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync("public");
            if (policy != null)
            {
                var comment = await _postCommentsRepository.GetPostCommentByIdAsync(commentId);
                if (comment != null)
                {
                    var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                    if (post != null)
                    {
                        if (post.PostPolicyId == policy.Id)
                        {
                            var replays = await _postCommentReplayRepository.GetPostCommentReplaysAsync(
                                commentId);
                            MakeUnnesseryObjectsInReplaysNull(replays);
                            if (replays.ToList().Count == 0)
                            {
                                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                    ._200_Success("No replays found", replays);
                            }
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                    ._200_Success("Replays found successfully", replays);
                        }
                        return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Post not found");
                }
                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Comment not found");
            }
            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<IEnumerable<PostCommentReplay>>> GetCommentReplaysByCommentIdAsync(
            string commentId, SiteUser user)
        {
            var comment = await _postCommentsRepository.GetPostCommentByIdAsync(commentId);
            if (comment != null)
            {
                var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                if (post != null)
                {
                    var isAbleToGetReplays = await CheckPolicyToGetReplaysAsync(post, user, comment);
                    if (isAbleToGetReplays.IsSuccess)
                    {
                        var replays = await _postCommentReplayRepository.GetPostCommentReplaysAsync(
                            commentId);
                        MakeUnnesseryObjectsInReplaysNull(replays);
                        if (replays.ToList().Count == 0)
                        {
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                ._200_Success("No replays found", replays);
                        }
                        return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                ._200_Success("Replays found successfully", replays);
                    }
                    return isAbleToGetReplays;
                }
                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                    ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                    ._404_NotFound("Comment not found");


        }

        public async Task<ApiResponse<IEnumerable<PostCommentReplay>>> GetReplaysOfReplayAsync(
            string replayId)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync("public");
            if (policy != null)
            {
                var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(replayId);
                if (replay != null)
                {
                    var comment = await _postCommentsRepository.GetPostCommentByIdAsync(replay.PostCommentId);
                    if (comment != null)
                    {
                        var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                        if (post != null)
                        {
                            if (post.PostPolicyId == policy.Id)
                            {
                                var replays = await _postCommentReplayRepository.GetReplaysOfReplayAsync(
                                    replayId);
                                MakeUnnesseryObjectsInReplaysNull(replays);
                                if (replays.ToList().Count == 0)
                                {
                                    return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                        ._200_Success("No replays found", replays);
                                }
                                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                        ._200_Success("Replays found successfully", replays);
                            }
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                ._403_Forbidden();
                        }
                        return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._404_NotFound("Post not found");
                    }
                    return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._404_NotFound("Comment not found");
                }
                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._404_NotFound("Post replay not found");
            }
            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<IEnumerable<PostCommentReplay>>> GetReplaysOfReplayAsync(
            string replayId, SiteUser user)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(replayId);
            if (replay != null)
            {
                var comment = await _postCommentsRepository.GetPostCommentByIdAsync(replay.PostCommentId);
                if (comment != null)
                {
                    var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                    if (post != null)
                    {
                        var isAbleToGetReplays = await CheckPolicyToGetReplaysAsync(post, user, replay);
                        if (isAbleToGetReplays.IsSuccess)
                        {
                            var replays = await _postCommentReplayRepository.GetReplaysOfReplayAsync(
                                replayId);
                            MakeUnnesseryObjectsInReplaysNull(replays);
                            if (replays.ToList().Count == 0)
                            {
                                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                    ._200_Success("No replays found", replays);
                            }
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                    ._200_Success("Replays found successfully", replays);
                        }
                        return isAbleToGetReplays;
                    }
                    return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Post not found");
                }
                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Comment not found");
            }
            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Post replay not found");

        }



        public async Task<ApiResponse<PostCommentReplay>> UpdateCommentReplayAsync(
            UpdatePostCommentReplayDto updatePostCommentReplayDto, SiteUser user)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                                updatePostCommentReplayDto.Id);
            if (replay != null)
            {
                var isAbleToUpdate = await IsAbleToOperateCommentReplayAsync(user, replay);
                if (isAbleToUpdate.IsSuccess)
                {
                    replay.Replay = updatePostCommentReplayDto.Replay;
                    if (updatePostCommentReplayDto.ReplayImage != null)
                    {
                        DeletePostImage(replay.ReplayImage!);
                        replay.ReplayImage = SavePostImages(
                            updatePostCommentReplayDto.ReplayImage);
                    }
                    replay = await _postCommentReplayRepository
                        .UpdatePostCommentReplayAsync(replay);
                    MakeUnnesseryObjectsInReplaysNull(replay);
                    return StatusCodeReturn<PostCommentReplay>
                        ._200_Success("Replay updated successfully", replay);
                }
                return isAbleToUpdate;
            }
            return StatusCodeReturn<PostCommentReplay>
                ._404_NotFound("Replay not found");
        }

        public async Task<ApiResponse<PostCommentReplay>> UpdateReplayToReplayAsync(
            UpdateReplayToReplayCommentDto updateReplayToReplayCommentDto, SiteUser user)
        {
            var replay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                                updateReplayToReplayCommentDto.ReplayId);
            if (replay != null)
            {
                var isAbleToUpdate = await IsAbleToOperateCommentReplayAsync(user, replay);
                if (isAbleToUpdate.IsSuccess)
                {
                    replay.Replay = updateReplayToReplayCommentDto.Replay;
                    if (updateReplayToReplayCommentDto.ReplayImage != null)
                    {
                        DeletePostImage(replay.ReplayImage!);
                        replay.ReplayImage = SavePostImages(
                            updateReplayToReplayCommentDto.ReplayImage);
                    }
                    replay = await _postCommentReplayRepository
                        .UpdatePostCommentReplayAsync(replay);
                    MakeUnnesseryObjectsInReplaysNull(replay);
                    return StatusCodeReturn<PostCommentReplay>
                        ._200_Success("Replay updated successfully", replay);
                }
                return isAbleToUpdate;
            }
            return StatusCodeReturn<PostCommentReplay>
                ._404_NotFound("Replay not found");
        }



        #region Private

        private async Task<ApiResponse<PostCommentReplay>> CheckIfPostPolicyIsPublicAsync(
            PostCommentReplay replay)
        {
            var comment = await _postCommentsRepository.GetPostCommentByIdAsync(replay.PostCommentId);
            if (comment != null)
            {
                var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                if (post != null)
                {
                    var policy = await _policyRepository.GetPolicyByNameAsync("public");
                    if (policy != null)
                    {
                        if (post.PostPolicyId == policy.Id)
                        {
                            return StatusCodeReturn<PostCommentReplay>
                                ._200_Success("Able");
                        }
                        return StatusCodeReturn<PostCommentReplay>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostCommentReplay>
                        ._404_NotFound("Policy not found");
                }
                return StatusCodeReturn<PostCommentReplay>
                        ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostCommentReplay>
                        ._404_NotFound("Comment not found");
        }

        private async Task<ApiResponse<bool>> CheckAbilityToGetCommentReplayAsync(
            PostComment comment, SiteUser user)
        {
            var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(comment.PostId);
            if (userPost != null)
            {
                var isBlockedByPostPublisher = await _blockRepository
                    .GetBlockByUserIdAndBlockedUserIdAsync(userPost.UserId, user.Id);
                var isBlockedByCommentPublisher = await _blockRepository
                    .GetBlockByUserIdAndBlockedUserIdAsync(userPost.UserId, comment.UserId);
                if (isBlockedByCommentPublisher == null && isBlockedByPostPublisher == null)
                {
                    return StatusCodeReturn<bool>
                        ._200_Success("Able", true);
                }
                return StatusCodeReturn<bool>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<bool>
                ._404_NotFound("User post not found", false);
        }


        private async Task<ApiResponse<PostCommentReplay>> CheckPolicyAsync(Post post, SiteUser user)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(post.PostPolicyId);

            if (policy != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                if (userPost != null)
                {
                    var isAblePolicy = await CheckUserPolicyAsync(userPost.UserId, user.Id, policy);
                    if (isAblePolicy.IsSuccess)
                    {
                        return await CheckPostCommentPolicyAsync(post, user);
                    }
                    return StatusCodeReturn<PostCommentReplay>
                                                ._403_Forbidden();
                }
                return StatusCodeReturn<PostCommentReplay>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostCommentReplay>
                        ._404_NotFound("Policy not found");
        }

        private async Task<ApiResponse<IEnumerable<PostCommentReplay>>> CheckPolicyToGetReplaysAsync(
            Post post, SiteUser user, PostCommentReplay replay)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(post.PostPolicyId);

            if (policy != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                if (userPost != null)
                {
                    var isAblePolicy = await CheckUserPolicyAsync(user.Id, userPost.UserId, policy);
                    if (isAblePolicy.IsSuccess)
                    {
                        var isBlockedByPostPublisher = await _blockRepository
                                .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, userPost.UserId);
                        var isBlockedByCommentPublisher = await _blockRepository
                            .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, replay.UserId);
                        if (isBlockedByCommentPublisher == null && isBlockedByPostPublisher == null)
                        {
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._200_Success("You can get replays");
                        }
                    }
                    return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._403_Forbidden("Forbidden user policy");
                }
                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Policy not found");
        }

        private async Task<ApiResponse<bool>> CheckUserPolicyAsync(
                string userId, string friendId, Policy policy)
        {
            if(userId == friendId)
            {
                return StatusCodeReturn<bool>
                ._200_Success("Able", true);
            }
            else if (policy.PolicyType == "PRIVATE")
            {
                return StatusCodeReturn<bool>
                    ._403_Forbidden();
            }
            else if (policy.PolicyType == "FRIENDS ONLY")
            {
                var isFriend = await _friendService.IsUserFriendAsync(userId, friendId);
                if (isFriend == null || !isFriend.IsSuccess)
                {
                    return StatusCodeReturn<bool>
                    ._403_Forbidden("Friends only", false);
                }
            }
            else if (policy.PolicyType == "FRIENDS OF FRIENDS")
            {
                var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(
                    userId, friendId);
                if (isFriendOfFriend == null || !isFriendOfFriend.IsSuccess)
                {
                    return StatusCodeReturn<bool>
                    ._403_Forbidden("Friends of friends only", false);
                }
            }
            return StatusCodeReturn<bool>
                ._200_Success("Able", true);
        }


        private async Task<ApiResponse<IEnumerable<PostCommentReplay>>> CheckPolicyToGetReplaysAsync(
            Post post, SiteUser user, PostComment comment)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(post.PostPolicyId);

            if (policy != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                if (userPost != null)
                {
                    var isAblePolicy = await CheckUserPolicyAsync(user.Id, userPost.UserId, policy);
                    if (isAblePolicy.IsSuccess)
                    {
                        var isBlockedByPostPublisher = await _blockRepository
                                .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, userPost.UserId);
                        var isBlockedByCommentPublisher = await _blockRepository
                            .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, comment.UserId);
                        if (isBlockedByCommentPublisher == null && isBlockedByPostPublisher == null)
                        {
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._200_Success("You can get replays");
                        }
                    }
                    return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                                ._403_Forbidden();
                }
                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Policy not found");
        }

        private async Task<ApiResponse<PostCommentReplay>> CheckPostCommentPolicyAsync(
            Post post, SiteUser user)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(post.CommentPolicyId);
            if (policy != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                if (userPost != null)
                {
                    var isAblePolicy = await CheckUserPolicyAsync(user.Id, userPost.Id, policy);
                    if (isAblePolicy.IsSuccess)
                    {
                        return StatusCodeReturn<PostCommentReplay>
                    ._200_Success("You can replay to comment");
                    }
                    return StatusCodeReturn<PostCommentReplay>
                        ._403_Forbidden("Forbidden user policy");
                }
                return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Policy not found");
        }

        private string SavePostImages(IFormFile image)
        {
            if (image == null)
            {
                return null!;
            }
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\CommentReplays");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
                fileStream.Flush();
            }
            return uniqueFileName;
        }

        private bool DeletePostImage(string imageUrl)
        {
            if (imageUrl == null)
            {
                return false!;
            }
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\CommentReplays\");
            var file = Path.Combine(path, $"{imageUrl}");
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
                return true;
            }
            return false;
        }

        private async Task<ApiResponse<PostCommentReplay>> IsAbleToOperateCommentReplayAsync(
            SiteUser user, PostCommentReplay replay)
        {
            var comment = await _postCommentsRepository.GetPostCommentByIdAsync(replay.PostCommentId);
            if (comment != null)
            {
                var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                if (post != null)
                {
                    var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                    if (userPost != null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                                        user.Id, userPost.UserId);
                        if (isBlocked == null)
                        {
                            var policy = await _policyRepository.GetPolicyByIdAsync(post.PostPolicyId);
                            if (policy != null)
                            {
                                var checkPolicy = await CheckUserPolicyAsync(user.Id, userPost.UserId,
                                    policy);
                                if (checkPolicy.IsSuccess)
                                {
                                    if (replay.UserId == user.Id)
                                    {
                                        return StatusCodeReturn<PostCommentReplay>
                                        ._200_Success("You can update or delete comment replay",
                                                new PostCommentReplay { });
                                    }
                                    return StatusCodeReturn<PostCommentReplay>
                                                ._403_Forbidden();
                                }
                                return StatusCodeReturn<PostCommentReplay>
                                                ._403_Forbidden();
                            }
                            return StatusCodeReturn<PostCommentReplay>
                                                ._404_NotFound("Policy not found");
                        }
                        return StatusCodeReturn<PostCommentReplay>
                                                ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostCommentReplay>
                                            ._404_NotFound("User post not found");
                }
                return StatusCodeReturn<PostCommentReplay>
                                            ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostCommentReplay>
                ._404_NotFound("Replay not found");
        }

        private async Task<ApiResponse<PostCommentReplay>> IsAbleToReplayToCommentAsync(
            SiteUser user, PostCommentReplay replay)
        {
            var comment = await _postCommentsRepository.GetPostCommentByIdAsync(replay.PostCommentId);
            if (comment != null)
            {
                var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                if (post != null)
                {
                    var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                    if (userPost != null)
                    {
                        var isBlockedByPostPublisher = await _blockRepository
                            .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, userPost.UserId);
                        var isBlockedByCommentPublisher = await _blockRepository
                            .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, comment.UserId);
                        if (isBlockedByCommentPublisher == null && isBlockedByPostPublisher == null)
                        {
                            var checkPolicy = await CheckPolicyAsync(post, user);
                            if (checkPolicy.IsSuccess)
                            {
                                return StatusCodeReturn<PostCommentReplay>
                                    ._200_Success("You can replay to comment",
                                            new PostCommentReplay { PostCommentId = comment.Id});
                            }
                            return StatusCodeReturn<PostCommentReplay>
                                                ._403_Forbidden();
                        }
                        return StatusCodeReturn<PostCommentReplay>
                                                ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostCommentReplay>
                                            ._404_NotFound("User post not found");
                }
                return StatusCodeReturn<PostCommentReplay>
                                            ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostCommentReplay>
                ._404_NotFound("Replay not found");
        }


        private void MakeUnnesseryObjectsInReplaysNull(IEnumerable<PostCommentReplay> replays)
        {
            foreach (var r in replays)
            {
                r.User = null;
                r.PostCommentReplays = null;
                r.PostComment = null;
                r.PostCommentReplayChildReplay = null;
            }
        }

        private void MakeUnnesseryObjectsInReplaysNull(PostCommentReplay replay)
        {

            replay.User = null;
            replay.PostCommentReplays = null;
            replay.PostComment = null;
            replay.PostCommentReplayChildReplay = null;
            
        }

        #endregion

    }
}

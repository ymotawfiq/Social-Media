

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.CommentPolicyRepository;
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
        private readonly ICommentPolicyRepository _commentPolicyRepository;
        private readonly IPostCommentsRepository _postCommentsRepository;
        private readonly IUserPostsRepository _userPostsRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IFriendService _friendService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PostCommentReplayService(
            IPostCommentReplayRepository _postCommentReplayRepository,
            IBlockRepository _blockRepository, ICommentPolicyRepository _commentPolicyRepository,
            IPostCommentsRepository _postCommentsRepository, IUserPostsRepository _userPostsRepository,
            IPostRepository _postRepository, IPolicyRepository _policyRepository,
            IFriendService _friendService, IWebHostEnvironment _webHostEnvironment
            )
        {
            this._blockRepository = _blockRepository;
            this._commentPolicyRepository = _commentPolicyRepository;
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
                                newCommentReplay.User = null;
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
                    commentReplay.User = null;
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
                    commentReplay.User = null;
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
                    replay.ReplayImage = null;
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
            var commentReplay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                commentReplayById);
            if (commentReplay != null)
            {
                commentReplay.User = null;
                return StatusCodeReturn<PostCommentReplay>
                    ._200_Success("Comment replay found successfully", commentReplay);
            }
            return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Comment replay not found");
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
                        if (post.PolicyId == policy.Id)
                        {
                            var replays = await _postCommentReplayRepository.GetPostCommentReplaysAsync(
                                commentId);
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
                            if(post.PolicyId == policy.Id)
                            {
                                var replays = await _postCommentReplayRepository.GetReplaysOfReplayAsync(
                                    replayId);
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

        public async Task<ApiResponse<PostCommentReplay>> GetReplayToReplayByIdAsync(
            string commentReplayToReplayById, SiteUser user)
        {
            var commentReplay = await _postCommentReplayRepository.GetPostCommentReplayByIdAsync(
                commentReplayToReplayById);
            if (commentReplay != null)
            {
                commentReplay.User = null;
                return StatusCodeReturn<PostCommentReplay>
                    ._200_Success("Comment replay found successfully", commentReplay);
            }
            return StatusCodeReturn<PostCommentReplay>
                    ._404_NotFound("Comment replay not found");
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
                    replay.User = null;
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
                    replay.User = null;
                    return StatusCodeReturn<PostCommentReplay>
                        ._200_Success("Replay updated successfully", replay);
                }
                return isAbleToUpdate;
            }
            return StatusCodeReturn<PostCommentReplay>
                ._404_NotFound("Replay not found");
        }



        private async Task<ApiResponse<PostCommentReplay>> CheckPolicyAsync(Post post, SiteUser user)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(post.PolicyId);
            
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
            var policy = await _policyRepository.GetPolicyByIdAsync(post.PolicyId);

            if (policy != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                if (userPost != null)
                {
                    if (policy.PolicyType == "PRIVATE")
                    {
                        return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._403_Forbidden();
                    }
                    else if (policy.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await _friendService.IsUserFriendAsync(user.Id, userPost.UserId);
                        if (isFriend == null || !isFriend!.ResponseObject)
                        {
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._403_Forbidden("Friends only");
                        }
                    }
                    else if (policy.PolicyType == "FRIENDS OF FRIENDS")
                    {
                        var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(
                            user.Id, userPost.UserId);
                        if (isFriendOfFriend == null || !isFriendOfFriend!.ResponseObject)
                        {
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._403_Forbidden("Friends of friends only");
                        }
                    }
                    var isBlockedByPostPublisher = await _blockRepository
                        .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, userPost.UserId);
                    var isBlockedByCommentPublisher = await _blockRepository
                        .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, replay.UserId);
                    if (isBlockedByCommentPublisher == null && isBlockedByPostPublisher == null)
                    {
                        return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._200_Success("You can get replays");
                    }
                    //var isAblePolicy = await CheckUserPolicyAsync(user.Id, userPost.Id, policy);
                    //if (isAblePolicy.IsSuccess)
                    //{
                    //    var isBlockedByPostPublisher = await _blockRepository
                    //            .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, userPost.UserId);
                    //    var isBlockedByCommentPublisher = await _blockRepository
                    //        .GetBlockByUserIdAndBlockedUserIdAsync(user.Id, replay.UserId);
                    //    if (isBlockedByCommentPublisher == null && isBlockedByPostPublisher == null)
                    //    {
                    //        return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                    //        ._200_Success("You can get replays");
                    //    }
                    //}
                    //return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                    //        ._403_Forbidden("Forbidden user policy");
                }
                return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Policy not found");
        }

        private async Task<ApiResponse<IEnumerable<PostCommentReplay>>> CheckPolicyToGetReplaysAsync(
            Post post, SiteUser user, PostComment comment)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(post.PolicyId);

            if (policy != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                if (userPost != null)
                {
                    if (policy.PolicyType == "PRIVATE")
                    {
                        return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._403_Forbidden();
                    }
                    else if (policy.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await _friendService.IsUserFriendAsync(user.Id, userPost.UserId);
                        if (isFriend == null || !isFriend!.ResponseObject)
                        {
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._403_Forbidden("Friends only");
                        }
                    }
                    else if (policy.PolicyType == "FRIENDS OF FRIENDS")
                    {
                        var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(
                            user.Id, userPost.UserId);
                        if (isFriendOfFriend == null || !isFriendOfFriend!.ResponseObject)
                        {
                            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                            ._403_Forbidden("Friends of friends only");
                        }
                    }
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
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<IEnumerable<PostCommentReplay>>
                        ._404_NotFound("Policy not found");
        }

        private async Task<ApiResponse<PostCommentReplay>> CheckPostCommentPolicyAsync(
            Post post, SiteUser user)
        {
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByIdAsync(
                post.CommentPolicyId);
            if (commentPolicy != null)
            {
                var policy = await _policyRepository.GetPolicyByIdAsync(commentPolicy.PolicyId);
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
            return StatusCodeReturn<PostCommentReplay>
                        ._404_NotFound("Comment policy not found");
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
                            var policy = await _policyRepository.GetPolicyByIdAsync(post.PolicyId);
                            if (policy != null)
                            {
                                if (comment.UserId == user.Id)
                                {
                                    return StatusCodeReturn<PostCommentReplay>
                                    ._200_Success("You can update or delete comment replay",
                                            new PostCommentReplay { });
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


        private async Task<ApiResponse<bool>> CheckUserPolicyAsync(
            string userId, string friendId, Policy policy)
        {
            if (policy.PolicyType == "PRIVATE")
            {
                return StatusCodeReturn<bool>
                    ._403_Forbidden();
            }
            else if (policy.PolicyType == "FRIENDS ONLY")
            {
                var isFriend = await _friendService.IsUserFriendAsync(userId, friendId);
                if (isFriend == null || !isFriend!.ResponseObject)
                {
                    return StatusCodeReturn<bool>
                    ._403_Forbidden("Friends only", false);
                }
            }
            else if (policy.PolicyType == "FRIENDS OF FRIENDS")
            {
                var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(
                    userId, friendId);
                if (isFriendOfFriend == null || !isFriendOfFriend!.ResponseObject)
                {
                    return StatusCodeReturn<bool>
                    ._403_Forbidden("Friends of friends only", false);
                }
            }
            return StatusCodeReturn<bool>
                ._200_Success("Able", true);
        }


    }
}

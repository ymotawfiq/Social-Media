
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.BlockRepository;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PostCommentsRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Service.FriendsService;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.PostCommentService
{
    public class PostCommentService : IPostCommentService
    {
        private readonly IPostCommentsRepository _postCommentsRepository;
        private readonly IPostRepository _postRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPolicyRepository _policyRepository;
        private readonly IFriendService _friendService;
        private readonly UserManagerReturn _userManagerReturn;
        public PostCommentService(IPostCommentsRepository _postCommentsRepository,
            IPostRepository _postRepository, IWebHostEnvironment _webHostEnvironment, 
            IBlockRepository _blockRepository, IPolicyRepository _policyRepository, 
            IFriendService _friendService, UserManagerReturn _userManagerReturn)
        {
            this._postCommentsRepository = _postCommentsRepository;
            this._postRepository = _postRepository;
            this._blockRepository = _blockRepository;
            this._webHostEnvironment = _webHostEnvironment;
            this._policyRepository = _policyRepository;
            this._friendService = _friendService;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<PostComment>> AddPostCommentAsync(AddPostCommentDto addPostCommentDto,
            SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(addPostCommentDto.PostId);
            if (post != null)
            {
                var userPost = await _postRepository.GetPostByIdAsync(addPostCommentDto.PostId);
                if (userPost != null)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, userPost.UserId);
                    if (isBlocked == null)
                    {
                        var checkPolicy = await CheckPolicyAsync(userPost.UserId, user.Id, post.Id);
                        if (checkPolicy.IsSuccess)
                        {
                            var newPostComment = await _postCommentsRepository.AddAsync(
                            ConvertFromDto.ConvertFromPostCommentDto_Add(addPostCommentDto, user,
                            SaveCommentImages(addPostCommentDto.CommentImage!)));
                            newPostComment.User = _userManagerReturn.SetUserToReturn(user);
                            newPostComment.Post = post;
                            return StatusCodeReturn<PostComment>
                                ._201_Created("Post comment added successfully", newPostComment);
                        }
                        return checkPolicy;
                    }
                    return StatusCodeReturn<PostComment>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostComment>
                    ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostComment>
                        ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<PostComment>> AddCommentReplayAsync(
            AddCommentReplayDto replay, SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(replay.PostId);
            if (post != null)
            {
                var comment = await _postCommentsRepository.GetByIdAsync(replay.CommentId);
                if (comment != null)
                {
                    var isBlockedByPostCreator = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        post.UserId, user.Id);
                    var isBlockedByCommentWriter = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        comment.UserId, user.Id);
                    if (isBlockedByCommentWriter == null && isBlockedByPostCreator == null)
                    {
                        var checkPolicy = await CheckPolicyAsync(post.UserId, user.Id, post.Id);
                        if (checkPolicy.IsSuccess)
                        {
                            var commentReplay = await _postCommentsRepository.AddAsync(
                                new PostComment
                                {
                                    Comment = replay.Comment,
                                    CommentId = replay.CommentId,
                                    Id = Guid.NewGuid().ToString(),
                                    PostId = replay.PostId,
                                    UserId = user.Id
                                });
                            commentReplay.User = _userManagerReturn.SetUserToReturn(user);
                            commentReplay.Post = post;
                            return StatusCodeReturn<PostComment>
                                ._201_Created("Replayed successfully", commentReplay);
                        }
                        return checkPolicy;
                    }
                    return StatusCodeReturn<PostComment>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostComment>
                    ._404_NotFound("Comment not found");
            }
            return StatusCodeReturn<PostComment>
                    ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<PostComment>> DeletePostCommentByIdAsync(string postCommentId,
            SiteUser user)
        {
            var postComment = await _postCommentsRepository.GetByIdAsync(postCommentId);
            if (postComment != null)
            {
                var userPost = await _postRepository.GetPostByIdAsync(postComment.PostId);
                if (userPost != null)
                {
                    if (postComment.UserId == user.Id || userPost.UserId == user.Id)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            userPost.UserId, user.Id);
                        if (isBlocked == null)
                        {
                            DeleteCommentImage(postComment.CommentImage!);
                            await _postCommentsRepository.DeleteByIdAsync(postComment.Id);
                            postComment.User = _userManagerReturn.SetUserToReturn(user);
                            postComment.Post = await _postRepository.GetPostByIdAsync(postComment.PostId);
                            return StatusCodeReturn<PostComment>
                                ._200_Success("Post comment deleted successfully", postComment);
                        }
                        return StatusCodeReturn<PostComment>
                        ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostComment>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostComment>
                ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostComment>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComment>> DeletePostCommentByPostIdAndUserIdAsync(string postId,
            SiteUser user)
        {
            var postComment = await _postCommentsRepository.GetPostCommentByPostIdAndUserIdAsync(
                postId, user.Id);
            if (postComment != null)
            {
                var userPost = await _postRepository.GetPostByIdAsync(postComment.PostId);
                if (userPost != null)
                {
                    if (postComment.UserId == user.Id || userPost.UserId == user.Id)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            userPost.UserId, user.Id);
                        if (isBlocked == null)
                        {
                            DeleteCommentImage(postComment.CommentImage!);
                            await _postCommentsRepository.DeleteByIdAsync(postComment.Id);
                            postComment.User = _userManagerReturn.SetUserToReturn(user);
                            postComment.Post = await _postRepository.GetPostByIdAsync(postComment.PostId);
                            return StatusCodeReturn<PostComment>
                                ._200_Success("Post comment deleted successfully", postComment);
                        }
                        return StatusCodeReturn<PostComment>
                        ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostComment>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostComment>
                ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostComment>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComment>> DeletePostCommentImageAsync(string postId,
            SiteUser user)
        {
            var postComment = await _postCommentsRepository.GetPostCommentByPostIdAndUserIdAsync(
                postId, user.Id);
            if (postComment != null)
            {
                if(postComment.UserId == user.Id)
                {
                    var userPost = await _postRepository.GetPostByIdAsync(postComment.PostId);
                    if (userPost != null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            user.Id, userPost.UserId);
                        if (isBlocked == null)
                        {
                            postComment = await _postCommentsRepository.DeletePostCommentImageAsync(
                                postId, user.Id);
                            postComment.User = _userManagerReturn.SetUserToReturn(user);
                            postComment.Post = await _postRepository.GetPostByIdAsync(postComment.PostId);
                            return StatusCodeReturn<PostComment>
                                ._200_Success("Post comment image deleted successfully", postComment);
                        }
                        return StatusCodeReturn<PostComment>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostComment>
                    ._404_NotFound("User post not found");
                }
                return StatusCodeReturn<PostComment>
                            ._403_Forbidden();
            }
            return StatusCodeReturn<PostComment>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComment>> DeletePostCommentImageAsync(string postCommentId)
        {
            var postComment = await _postCommentsRepository.GetByIdAsync(postCommentId);
            if (postComment != null)
            {
                DeleteCommentImage(postComment.CommentImage!);
                await _postCommentsRepository.DeletePostCommentImageAsync(postCommentId);
                postComment.User = _userManagerReturn.SetUserToReturn(await _userManagerReturn
                    .GetUserByUserNameOrEmailOrIdAsync(postComment.UserId));
                postComment.Post = await _postRepository.GetPostByIdAsync(postComment.PostId);
                return StatusCodeReturn<PostComment>
                    ._200_Success("Post comment image deleted successfully", postComment);
            }
            return StatusCodeReturn<PostComment>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComment>> GetPostCommentByIdAsync(string postCommentId)
        {
            var postComment = await _postCommentsRepository.GetByIdAsync(postCommentId);
            postComment.User = null;
            if (postComment != null)
            {
                postComment.User = _userManagerReturn.SetUserToReturn(await _userManagerReturn
                    .GetUserByUserNameOrEmailOrIdAsync(postComment.UserId));
                postComment.Post = await _postRepository.GetPostByIdAsync(postComment.PostId);
                return StatusCodeReturn<PostComment>
                    ._200_Success("Post comment found successfully", postComment);
            }
            return StatusCodeReturn<PostComment>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComment>> GetPostCommentByIdAsync(string postCommentId,
            SiteUser user)
        {
            var postComment = await _postCommentsRepository.GetByIdAsync(postCommentId);
            if (postComment != null)
            {
                var userPost = await _postRepository.GetPostByIdAsync(postComment.PostId);
                if (userPost != null)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, userPost.UserId);
                    if (isBlocked == null)
                    {
                        postComment.User = _userManagerReturn.SetUserToReturn(user);
                        postComment.Post = await _postRepository.GetPostByIdAsync(postComment.PostId);
                        return StatusCodeReturn<PostComment>
                            ._200_Success("Post comment found successfully", postComment);
                    }
                    return StatusCodeReturn<PostComment>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostComment>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostComment>
                        ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComment>> GetPostCommentByPostIdAndUserIdAsync(string postId,
            SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post != null)
            {
                var userPost = await _postRepository.GetPostByIdAsync(postId);
                if (userPost != null)
                {
                    var postComment = await _postCommentsRepository.GetPostCommentByPostIdAndUserIdAsync(
                    postId, user.Id);
                    if (postComment != null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            user.Id, userPost.UserId);
                        if (isBlocked == null)
                        {
                            postComment.User = _userManagerReturn.SetUserToReturn(user);
                            postComment.Post = await _postRepository.GetPostByIdAsync(postComment.PostId);
                            return StatusCodeReturn<PostComment>
                                ._200_Success("Post comment found successfully", postComment);
                        }
                        return StatusCodeReturn<PostComment>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostComment>
                        ._404_NotFound("Post comment not found");
                }
                return StatusCodeReturn<PostComment>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostComment>
                        ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<IEnumerable<PostComment>>> GetPostCommentsByPostIdAsync(string postId)
        {
            var postComments = await _postCommentsRepository.GetPostCommentsByPostIdAsync(postId);
            if (postComments.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostComment>>
                    ._200_Success("No post comments found", postComments);
            }
            foreach(var p in postComments)
            {
                p.User = null;
            }
            return StatusCodeReturn<IEnumerable<PostComment>>
                    ._200_Success("Post comments found successfully", postComments);
        }

        public async Task<ApiResponse<IEnumerable<PostComment>>> GetPostCommentsByPostIdAsync(string postId,
            SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post != null)
            {
                var userPost = await _postRepository.GetPostByIdAsync(postId);
                if (userPost != null)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, userPost.UserId);
                    if (isBlocked == null)
                    {
                        var postComments = await _postCommentsRepository.GetPostCommentsByPostIdAsync(postId);
                        if (postComments.ToList().Count == 0)
                        {
                            return StatusCodeReturn<IEnumerable<PostComment>>
                                ._200_Success("No post comments found", postComments);
                        }
                        foreach (var p in postComments)
                        {
                            p.User = null;
                        }
                        return StatusCodeReturn<IEnumerable<PostComment>>
                                ._200_Success("Post comments found successfully", postComments);
                    }
                    return StatusCodeReturn<IEnumerable<PostComment>>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<IEnumerable<PostComment>>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<IEnumerable<PostComment>>
                        ._404_NotFound("Post not found");
        }


        public async Task<ApiResponse<IEnumerable<PostComment>>> GetPostCommentsByPostIdAndUserIdAsync(
            string postId, string userId)
        {
            var postComments = await _postCommentsRepository.GetPostCommentsByPostIdAndUserIdAsync(
                postId, userId);
            if (postComments.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostComment>>
                    ._200_Success("No post comments found", postComments);
            }
            foreach (var p in postComments)
            {
                p.User = null;
            }
            return StatusCodeReturn<IEnumerable<PostComment>>
                    ._200_Success("Post comments found successfully", postComments);
        }

        public async Task<ApiResponse<PostComment>> UpdatePostCommentAsync(
            UpdatePostCommentDto updatePostCommentDto, SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(updatePostCommentDto.PostId);
            if (post != null)
            {
                var postComment = await _postCommentsRepository.GetPostCommentByPostIdAndUserIdAsync(
                    updatePostCommentDto.PostId, user.Id);
                if (postComment != null)
                {
                    var userPost = await _postRepository.GetPostByIdAsync(updatePostCommentDto.PostId);
                    if (userPost != null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            user.Id, userPost.UserId);
                        if (isBlocked == null)
                        {
                            if(user.Id == postComment.UserId)
                            {
                                if (postComment.CommentImage != null || postComment.CommentImage == null)
                                {
                                    if (updatePostCommentDto.CommentImage != null)
                                    {
                                        DeleteCommentImage(postComment.CommentImage!);
                                        postComment.CommentImage = SaveCommentImages(
                                            updatePostCommentDto.CommentImage);
                                    }
                                }
                                postComment = await _postCommentsRepository.UpdateAsync(
                                    postComment);
                                postComment.User = _userManagerReturn.SetUserToReturn(user);
                                postComment.Post = await _postRepository.GetPostByIdAsync(postComment.PostId);
                                return StatusCodeReturn<PostComment>
                                    ._200_Success("Post comment updated successfully", postComment);
                            }
                            return StatusCodeReturn<PostComment>
                                ._403_Forbidden();
                        }
                        return StatusCodeReturn<PostComment>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostComment>
                        ._404_NotFound("User post not found");
                }
                return StatusCodeReturn<PostComment>
                    ._403_Forbidden("Post comment already exists");
            }
            return StatusCodeReturn<PostComment>
                        ._404_NotFound("Post not found");
        }

        private async Task<ApiResponse<PostComment>> CheckPolicyAsync(string userId, 
            string userWhoWantsToCommentId, string postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post != null)
            {
                var policy = await _policyRepository.GetByIdAsync(post.CommentPolicyId);
                if (policy != null)
                {
                    if(policy.PolicyType == "PRIVATE")
                    {
                        return StatusCodeReturn<PostComment>
                            ._403_Forbidden();
                    }
                    else if(policy.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await _friendService.IsUserFriendAsync(userId,
                            userWhoWantsToCommentId);
                        if (isFriend == null || !isFriend!.ResponseObject)
                        {
                            return StatusCodeReturn<PostComment>
                            ._403_Forbidden("Friends only");
                        }
                    }
                    else if (policy.PolicyType == "FRIENDS OF FRIENDS")
                    {
                        var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(userId,
                            userWhoWantsToCommentId);
                        if (isFriendOfFriend == null || !isFriendOfFriend!.ResponseObject)
                        {
                            return StatusCodeReturn<PostComment>
                            ._403_Forbidden("friends of friends only");
                        }
                    }
                    return StatusCodeReturn<PostComment>
                        ._200_Success("You can comment", new PostComment { });
                }
                return StatusCodeReturn<PostComment>
                    ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<PostComment>
                        ._404_NotFound("Post not found");
        }

        private string SaveCommentImages(IFormFile image)
        {
            if (image == null)
            {
                return null!;
            }
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\Comment_Images");
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

        private bool DeleteCommentImage(string imageUrl)
        {
            if (imageUrl == null)
            {
                return false;
            }
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\Comment_Images\");
            var file = Path.Combine(path, $"{imageUrl}");
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
                return true;
            }
            return false;
        }


    }
}

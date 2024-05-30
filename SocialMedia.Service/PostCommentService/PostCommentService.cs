
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.PostCommentsRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.PostCommentService
{
    public class PostCommentService : IPostCommentService
    {
        private readonly IPostCommentsRepository _postCommentsRepository;
        private readonly IPostRepository _postRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IUserPostsRepository _userPostsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PostCommentService(IPostCommentsRepository _postCommentsRepository,
            IPostRepository _postRepository, IWebHostEnvironment _webHostEnvironment, 
            IBlockRepository _blockRepository,
            IUserPostsRepository _userPostsRepository)
        {
            this._postCommentsRepository = _postCommentsRepository;
            this._postRepository = _postRepository;
            this._blockRepository = _blockRepository;
            this._userPostsRepository = _userPostsRepository;
            this._webHostEnvironment = _webHostEnvironment;
        }
        public async Task<ApiResponse<PostComments>> AddPostCommentAsync(AddPostCommentDto addPostCommentDto,
            SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(addPostCommentDto.PostId);
            if (post != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(
                    addPostCommentDto.PostId);
                if (userPost != null)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, userPost.UserId);
                    if (isBlocked == null)
                    {
                        var newPostComment = await _postCommentsRepository.AddPostCommentAsync(
                            ConvertFromDto.ConvertFromPostCommentDto_Add(addPostCommentDto, user,
                            SaveCommentImages(addPostCommentDto.CommentImage!)));
                        newPostComment.User = null;
                        return StatusCodeReturn<PostComments>
                            ._201_Created("Post comment added successfully", newPostComment);
                    }
                    return StatusCodeReturn<PostComments>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostComments>
                    ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostComments>
                        ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<PostComments>> DeletePostCommentByIdAsync(string postCommentId,
            SiteUser user)
        {
            var postComment = await _postCommentsRepository.GetPostCommentByIdAsync(postCommentId);
            if (postComment != null)
            {
                if(postComment.UserId == user.Id)
                {
                    DeleteCommentImage(postComment.CommentImage!);
                    await _postCommentsRepository.DeletePostCommentByIdAsync(postComment.Id);
                    postComment.User = null;
                    return StatusCodeReturn<PostComments>
                        ._200_Success("Post comment deleted successfully", postComment);
                }
                return StatusCodeReturn<PostComments>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<PostComments>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComments>> DeletePostCommentByPostIdAndUserIdAsync(string postId,
            SiteUser user)
        {
            var postComment = await _postCommentsRepository.GetPostCommentByPostIdAndUserIdAsync(
                postId, user.Id);
            if (postComment != null)
            {
                DeleteCommentImage(postComment.CommentImage!);
                await _postCommentsRepository.DeletePostCommentByIdAsync(postComment.Id);
                postComment.User = null;
                return StatusCodeReturn<PostComments>
                    ._200_Success("Post comment deleted successfully", postComment);
            }
            return StatusCodeReturn<PostComments>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComments>> DeletePostCommentImageAsync(string postId,
            SiteUser user)
        {
            var postComment = await _postCommentsRepository.GetPostCommentByPostIdAndUserIdAsync(
                postId, user.Id);
            if (postComment != null)
            {
                postComment = await _postCommentsRepository.DeletePostCommentImageAsync(postId, user.Id);
                postComment.User = null;
                return StatusCodeReturn<PostComments>
                    ._200_Success("Post comment image deleted successfully", postComment);
            }
            return StatusCodeReturn<PostComments>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComments>> DeletePostCommentImageAsync(string postCommentId)
        {
            var postComment = await _postCommentsRepository.GetPostCommentByIdAsync(postCommentId);
            if (postComment != null)
            {
                DeleteCommentImage(postComment.CommentImage!);
                await _postCommentsRepository.DeletePostCommentImageAsync(postCommentId);
                postComment.User = null;
                return StatusCodeReturn<PostComments>
                    ._200_Success("Post comment image deleted successfully", postComment);
            }
            return StatusCodeReturn<PostComments>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComments>> GetPostCommentByIdAsync(string postCommentId)
        {
            var postComment = await _postCommentsRepository.GetPostCommentByIdAsync(postCommentId);
            postComment.User = null;
            if (postComment != null)
            {
                return StatusCodeReturn<PostComments>
                    ._200_Success("Post comment found successfully", postComment);
            }
            return StatusCodeReturn<PostComments>
                ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComments>> GetPostCommentByIdAsync(string postCommentId,
            SiteUser user)
        {
            var postComment = await _postCommentsRepository.GetPostCommentByIdAsync(postCommentId);
            if (postComment != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(postComment.PostId);
                if (userPost != null)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, userPost.UserId);
                    if (isBlocked == null)
                    {
                        postComment.User = null;
                        return StatusCodeReturn<PostComments>
                            ._200_Success("Post comment found successfully", postComment);
                    }
                    return StatusCodeReturn<PostComments>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PostComments>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostComments>
                        ._404_NotFound("Post comment not found");
        }

        public async Task<ApiResponse<PostComments>> GetPostCommentByPostIdAndUserIdAsync(string postId,
            SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(postId);
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
                            postComment.User = null;
                            return StatusCodeReturn<PostComments>
                                ._200_Success("Post comment found successfully", postComment);
                        }
                        return StatusCodeReturn<PostComments>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostComments>
                        ._404_NotFound("Post comment not found");
                }
                return StatusCodeReturn<PostComments>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<PostComments>
                        ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<IEnumerable<PostComments>>> GetPostCommentsByPostIdAsync(string postId)
        {
            var postComments = await _postCommentsRepository.GetPostCommentsByPostIdAsync(postId);
            if (postComments.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostComments>>
                    ._200_Success("No post comments found", postComments);
            }
            foreach(var p in postComments)
            {
                p.User = null;
            }
            return StatusCodeReturn<IEnumerable<PostComments>>
                    ._200_Success("Post comments found successfully", postComments);
        }

        public async Task<ApiResponse<IEnumerable<PostComments>>> GetPostCommentsByPostIdAsync(string postId,
            SiteUser user)
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
                        var postComments = await _postCommentsRepository.GetPostCommentsByPostIdAsync(postId);
                        if (postComments.ToList().Count == 0)
                        {
                            return StatusCodeReturn<IEnumerable<PostComments>>
                                ._200_Success("No post comments found", postComments);
                        }
                        foreach (var p in postComments)
                        {
                            p.User = null;
                        }
                        return StatusCodeReturn<IEnumerable<PostComments>>
                                ._200_Success("Post comments found successfully", postComments);
                    }
                    return StatusCodeReturn<IEnumerable<PostComments>>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<IEnumerable<PostComments>>
                        ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<IEnumerable<PostComments>>
                        ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<PostComments>> UpdatePostCommentAsync(
            UpdatePostCommentDto updatePostCommentDto, SiteUser user)
        {
            var post = await _postRepository.GetPostByIdAsync(updatePostCommentDto.PostId);
            if (post != null)
            {
                var postComment = await _postCommentsRepository.GetPostCommentByPostIdAndUserIdAsync(
                    updatePostCommentDto.PostId, user.Id);
                if (postComment != null)
                {
                    var userPost = await _userPostsRepository.GetUserPostByPostIdAsync(
                        updatePostCommentDto.PostId);
                    if (userPost != null)
                    {
                        var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                            user.Id, userPost.UserId);
                        if (isBlocked == null)
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
                            postComment = await _postCommentsRepository.UpdatePostCommentAsync(postComment);
                            postComment.User = null;
                            return StatusCodeReturn<PostComments>
                                ._200_Success("Post comment updated successfully", postComment);
                        }
                        return StatusCodeReturn<PostComments>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<PostComments>
                        ._404_NotFound("User post not found");
                }
                return StatusCodeReturn<PostComments>
                    ._403_Forbidden("Post comment already exists");
            }
            return StatusCodeReturn<PostComments>
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

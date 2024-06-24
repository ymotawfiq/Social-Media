

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Repository.SavePostsRepository;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.UserSavedPostsFoldersService;

namespace SocialMedia.Api.Service.SavedPostsService
{
    public class SavedPostsService : ISavedPostsService
    {
        private readonly ISavePostsRepository _savePostsRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserSavedPostsFolderService _userSavedPostsFolderService;
        private readonly UserManagerReturn _userManagerReturn;
        public SavedPostsService(ISavePostsRepository _savePostsRepository, IPostRepository _postRepository,
            IUserSavedPostsFolderService _userSavedPostsFolderService, UserManagerReturn _userManagerReturn)
        {
            this._savePostsRepository = _savePostsRepository;
            this._postRepository = _postRepository;
            this._userSavedPostsFolderService = _userSavedPostsFolderService;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<SavedPosts>> SavePostAsync(SiteUser user, SavePostDto savePostDto)
        {
            var post = await _postRepository.GetPostByIdAsync(savePostDto.PostId);
            if (post != null)
            {
                var folder = await _userSavedPostsFolderService.GetUserSavedPostsFoldersByFolderIdAsync(
                    user, savePostDto.FolderId);
                if (folder != null && folder.ResponseObject != null)
                {
                    var existPost = await _savePostsRepository.GetSavedPostAsync(user, post.Id,
                        folder.ResponseObject.Id);
                    if (existPost != null)
                    {
                        return StatusCodeReturn<SavedPosts>
                            ._403_Forbidden("Post already saved in the same folder");
                    }
                    var newSavedPost = await _savePostsRepository.AddAsync(
                        new SavedPosts
                        {
                            FolderId = folder.ResponseObject.Id,
                            Id = Guid.NewGuid().ToString(),
                            PostId = post.Id,
                            UserId = user.Id,
                            Post = post,
                            User = _userManagerReturn.SetUserToReturn(user),
                            UserSavedPostsFolder = folder.ResponseObject
                        }
                        );
                    return StatusCodeReturn<SavedPosts>
                        ._201_Created("Post saved successfully", newSavedPost);
                }
                return StatusCodeReturn<SavedPosts>
                    ._404_NotFound("Folder not found");
            }
            return StatusCodeReturn<SavedPosts>
                    ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<SavedPosts>> UnSavePostAsync(SiteUser user, string postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post != null)
            {
                var existPost = await _savePostsRepository.GetSavedPostAsync(user, post.Id);
                if (existPost != null)
                {
                    await _savePostsRepository.UnSavePostAsync(user, postId);
                    return StatusCodeReturn<SavedPosts>
                        ._200_Success("Post unsaved successfully", null!);
                }

                return StatusCodeReturn<SavedPosts>
                    ._404_NotFound("Post not found in saved posts");
            }
            return StatusCodeReturn<SavedPosts>
                    ._404_NotFound("Post not found");
        }
    }
}

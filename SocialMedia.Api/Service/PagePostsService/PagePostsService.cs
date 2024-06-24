

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PagePostsRepository;
using SocialMedia.Api.Repository.PageRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PostService;

namespace SocialMedia.Api.Service.PagePostsService
{
    public class PagePostsService : IPagePostsService
    {
        private readonly IPagePostsRepository _pagePostsRepository;
        private readonly IPostService _postService;
        private readonly IPageRepository _pageRepository;
        private readonly IPostRepository _postRepository;
        public PagePostsService(IPagePostsRepository _pagePostsRepository, IPostService _postService,
            IPageRepository _pageRepository, IPostRepository _postRepository)
        {
            this._pagePostsRepository = _pagePostsRepository;
            this._postService = _postService;
            this._pageRepository = _pageRepository;
            this._postRepository = _postRepository;
        }
        public async Task<ApiResponse<object>> AddPagePostAsync(
            AddPagePostDto addPagePostDto, SiteUser user)
        {
            var page = await _pageRepository.GetByIdAsync(addPagePostDto.PageId);
            if (page != null)
            {
                if(page.CreatorId == user.Id)
                {
                    var postDto = await _postService.AddPostAsync(user, new AddPostDto
                    {
                        Images = addPagePostDto.Images,
                        PostContent = addPagePostDto.PostContent
                    });
                    if (postDto.IsSuccess && postDto.ResponseObject != null && postDto != null)
                    {
                        var newPagePost = await _pagePostsRepository.AddAsync(new PagePost
                        {
                            Id = Guid.NewGuid().ToString(),
                            PageId = addPagePostDto.PageId,
                            PostId = postDto.ResponseObject.Post.Id
                        });
                        newPagePost.Page = page;
                        newPagePost.Post = postDto.ResponseObject.Post;
                        return StatusCodeReturn<object>
                            ._201_Created("Page post created successfully", newPagePost);
                    }
                }
                return StatusCodeReturn<object>
                    ._403_Forbidden("Unauthorized to post in this page");
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Page not found");
        }

        public async Task<ApiResponse<object>> DeletePagePostByIdAsync(string pagePostId, SiteUser user)
        {
            var pagePost = await _pagePostsRepository.GetByIdAsync(pagePostId);
            if (pagePost != null)
            {
                var userPost = await _postService.GetPostByIdAsync(user, pagePost.PostId);
                if (userPost != null && userPost.ResponseObject != null)
                {
                    await _postService.DeletePostAsync(user, pagePost.PostId);
                    pagePost.Page = await _pageRepository.GetByIdAsync(pagePost.PageId);
                    pagePost.Post = await _postRepository.GetPostByIdAsync(pagePost.PostId);
                    return StatusCodeReturn<object>
                        ._200_Success("Page post deleted successfully", pagePost);
                }
                return StatusCodeReturn<object>
                    ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Page post not found");
        }

        public async Task<ApiResponse<object>> DeletePagePostByPostIdAsync(string postId, SiteUser user)
        {
            var userPost = await _postService.GetPostByIdAsync(user, postId);
            if (userPost != null && userPost.ResponseObject != null)
            {
                await _postService.DeletePostAsync(user, postId);
                return StatusCodeReturn<object>
                    ._200_Success("Page post deleted successfully");
            }
            return StatusCodeReturn<object>
                ._404_NotFound("User post not found");
        }

        public async Task<ApiResponse<object>> GetPagePostByIdAsync(string pagePostId)
        {
            var pagePost = await _pagePostsRepository.GetByIdAsync(pagePostId);
            if (pagePost != null)
            {
                pagePost.Page = await _pageRepository.GetByIdAsync(pagePost.PageId);
                pagePost.Post = await _postRepository.GetPostByIdAsync(pagePost.PostId);
                return StatusCodeReturn<object>
                    ._200_Success("Page post found successfully", pagePost);
            }
            return StatusCodeReturn<object>
                    ._404_NotFound("Page post not found");
        }
    }
}

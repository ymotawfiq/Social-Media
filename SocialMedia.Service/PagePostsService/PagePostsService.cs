

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.PagePostsRepository;
using SocialMedia.Repository.PageRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PostService;

namespace SocialMedia.Service.PagePostsService
{
    public class PagePostsService : IPagePostsService
    {
        private readonly IPagePostsRepository _pagePostsRepository;
        private readonly IPostService _postService;
        private readonly IPageRepository _pageRepository;
        private readonly IUserPostsRepository _userPostsRepository;
        public PagePostsService(IPagePostsRepository _pagePostsRepository, IPostService _postService,
            IPageRepository _pageRepository, IUserPostsRepository _userPostsRepository)
        {
            this._pagePostsRepository = _pagePostsRepository;
            this._postService = _postService;
            this._pageRepository = _pageRepository;
            this._userPostsRepository = _userPostsRepository;
        }
        public async Task<ApiResponse<object>> AddPagePostAsync(
            AddPagePostDto addPagePostDto, SiteUser user)
        {
            var page = await _pageRepository.GetPageByIdAsync(addPagePostDto.PageId);
            if (page != null)
            {
                var postDto = await _postService.AddPostAsync(user, new AddPostDto
                {
                    Images = addPagePostDto.Images,
                    PostContent = addPagePostDto.PostContent
                });
                if (postDto.IsSuccess && postDto.ResponseObject != null && postDto != null)
                {
                    var newPagePost = await _pagePostsRepository.AddPagePostAsync(new Data.Models.PagePosts
                    {
                        Id = Guid.NewGuid().ToString(),
                        PageId = addPagePostDto.PageId,
                        PostId = postDto.ResponseObject.PostId
                    });
                    newPagePost.Page = null;
                    newPagePost.Post = null;
                    return StatusCodeReturn<object>
                        ._201_Created("Page post created successfully", newPagePost);
                }
                return StatusCodeReturn<object>
                    ._406_NotAcceptable();
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Page not found");
        }

        public async Task<ApiResponse<object>> DeletePagePostByIdAsync(string pagePostId, SiteUser user)
        {
            var pagePost = await _pagePostsRepository.GetPagePostByIdAsync(pagePostId);
            if (pagePost != null)
            {
                var userPost = await _userPostsRepository.GetUserPostByUserAndPostIdAsync(
                    user.Id, pagePost.PostId);
                if (userPost != null)
                {
                    await _pagePostsRepository.DeletePagePostByIdAsync(pagePostId);
                    await _postService.DeletePostAsync(user, pagePost.PostId);
                    return StatusCodeReturn<object>
                        ._200_Success("Page post deleted successfully");
                }
                return StatusCodeReturn<object>
                    ._404_NotFound("User post not found");
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Page post not found");
        }

        public Task<ApiResponse<object>> GetPagePostByIdAsync(string pagePostId)
        {
            throw new NotImplementedException();
        }
    }
}


using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.PageRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.PageService
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        public PageService(IPageRepository _pageRepository)
        {
            this._pageRepository = _pageRepository;
        }
        public async Task<ApiResponse<Page>> AddPageAsync(AddPageDto addPageDto, SiteUser user)
        {
            var newPage = await _pageRepository.AddPageAsync(ConvertFromDto.ConvertFromPageDto_Add(
                addPageDto, user));
            return StatusCodeReturn<Page>
                ._201_Created("Page created successfully", newPage);
        }

        public async Task<ApiResponse<Page>> DeletePageByIdAsync(string pageId, SiteUser user)
        {
            var page = await _pageRepository.GetPageByIdAsync(pageId);
            if (page != null)
            {
                if(page.CreatorId == user.Id)
                {
                    await _pageRepository.DeletePageByIdAsync(pageId);
                    return StatusCodeReturn<Page>
                        ._200_Success("Page deleted successfully", page);
                }
                return StatusCodeReturn<Page>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<Page>
                    ._404_NotFound("Page not found");
        }

        public async Task<ApiResponse<Page>> GetPageByIdAsync(string pageId)
        {
            var page = await _pageRepository.GetPageByIdAsync(pageId);
            if (page != null)
            {
                return StatusCodeReturn<Page>
                    ._200_Success("Page found successfully", page);
            }
            return StatusCodeReturn<Page>
                ._404_NotFound("Page not found");
        }

        public async Task<ApiResponse<IEnumerable<Page>>> GetPagesByUserIdAsync(string userId)
        {
            var pages = await _pageRepository.GetPagesByUserIdAsync(userId);
            if (pages.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<Page>>
                    ._200_Success("No pages found", pages);
            }
            return StatusCodeReturn<IEnumerable<Page>>
                    ._200_Success("Pages found successfully", pages);
        }

        public async Task<ApiResponse<Page>> UpdatePageAsync(UpdatePageDto updatePageDto, SiteUser user)
        {
            var page = await _pageRepository.GetPageByIdAsync(updatePageDto.Id);
            if (page != null)
            {
                if(user.Id == page.CreatorId)
                {
                    var updatedPage = await _pageRepository.UpdatePageAsync(ConvertFromDto
                        .ConvertFromPageDto_Update(updatePageDto));
                    return StatusCodeReturn<Page>
                        ._200_Success("Page updated successfully", updatedPage);
                }
                return StatusCodeReturn<Page>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<Page>
                    ._404_NotFound("Page not found");
        }

    }
}

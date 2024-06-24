
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PageRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.PageService
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public PageService(IPageRepository _pageRepository, UserManagerReturn _userManagerReturn)
        {
            this._pageRepository = _pageRepository;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<Page>> AddPageAsync(AddPageDto addPageDto, SiteUser user)
        {
            var newPage = await _pageRepository.AddAsync(ConvertFromDto.ConvertFromPageDto_Add(
                addPageDto, user));
            newPage.Creator = _userManagerReturn.SetUserToReturn(user);
            return StatusCodeReturn<Page>
                ._201_Created("Page created successfully", newPage);
        }

        public async Task<ApiResponse<Page>> DeletePageByIdAsync(string pageId, SiteUser user)
        {
            var page = await _pageRepository.GetByIdAsync(pageId);
            if (page != null)
            {
                if(page.CreatorId == user.Id)
                {
                    await _pageRepository.DeleteByIdAsync(pageId);
                    page.Creator = _userManagerReturn.SetUserToReturn(user);
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
            var page = await _pageRepository.GetByIdAsync(pageId);
            if (page != null)
            {
                page.Creator = _userManagerReturn.SetUserToReturn(await _userManagerReturn
                    .GetUserByUserNameOrEmailOrIdAsync(page.CreatorId));
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
            var page = await _pageRepository.GetByIdAsync(updatePageDto.Id);
            if (page != null)
            {
                if(user.Id == page.CreatorId)
                {
                    var updatedPage = await _pageRepository.UpdateAsync(ConvertFromDto
                        .ConvertFromPageDto_Update(updatePageDto));
                    updatedPage.Creator = _userManagerReturn.SetUserToReturn(user);
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

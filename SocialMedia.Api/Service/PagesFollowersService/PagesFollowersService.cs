
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PageRepository;
using SocialMedia.Api.Repository.PagesFollowersRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.PagesFollowersService
{
    public class PagesFollowersService : IPagesFollowersService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IPagesFollowersRepository _pagesFollowersRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public PagesFollowersService(IPageRepository _pageRepository,
            IPagesFollowersRepository _pagesFollowersRepository, UserManagerReturn _userManagerReturn)
        {
            this._pageRepository = _pageRepository;
            this._pagesFollowersRepository = _pagesFollowersRepository;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<object>> FollowPageAsync(FollowPageDto followPageDto, SiteUser user)
        {
            var page = await _pageRepository.GetByIdAsync(followPageDto.PageId);
            if (page != null)
            {
                var pageFollower = await _pagesFollowersRepository.GetPageFollowerByPageIdAndFollowerIdAsync(
                    followPageDto.PageId, user.Id);
                if (pageFollower == null)
                {
                    var followPage = await _pagesFollowersRepository.AddAsync(
                        ConvertFromDto.ConvertFromFollowPageDto_Add(followPageDto, user));
                    followPage.User = _userManagerReturn.SetUserToReturn(user);
                    followPage.Page = page;
                    return StatusCodeReturn<object>
                        ._201_Created("Followed successfully", followPage);
                }
                return StatusCodeReturn<object>
                    ._403_Forbidden("You already following this page");
            }
            return StatusCodeReturn<object>
                ._404_NotFound("page you want to follow not found");
        }

        public async Task<ApiResponse<object>> FollowPageAsync(FollowPageUserDto followPageUserDto)
        {
            var page = await _pageRepository.GetByIdAsync(followPageUserDto.PageId);
            if (page != null)
            {
                var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                    followPageUserDto.UserIdOrUserNameOrEmail);
                if (user != null)
                {
                    followPageUserDto.UserIdOrUserNameOrEmail = user.Id;
                    var pageFollower = await _pagesFollowersRepository.GetPageFollowerByPageIdAndFollowerIdAsync(
                        followPageUserDto.PageId, followPageUserDto.UserIdOrUserNameOrEmail);
                    if (pageFollower == null)
                    {
                        var followPage = await _pagesFollowersRepository.AddAsync(
                            ConvertFromDto.ConvertFromFollowPageUserDto_Add(followPageUserDto));
                        followPage.User = _userManagerReturn.SetUserToReturn(user);
                        followPage.Page = page;
                        return StatusCodeReturn<object>
                            ._201_Created("Followed successfully", followPage);
                    }
                    return StatusCodeReturn<object>
                    ._403_Forbidden("You already following this page");
                }
                return StatusCodeReturn<object>
                ._404_NotFound("User not found");
            }
            return StatusCodeReturn<object>
                ._404_NotFound("page you want to follow not found");
        }

        public async Task<ApiResponse<object>> GetPageFollowerAsync(string pageId, SiteUser user)
        {
            var page = await _pageRepository.GetByIdAsync(pageId);
            if (page != null)
            {
                var pageFollower = await _pagesFollowersRepository.GetPageFollowerByPageIdAndFollowerIdAsync(
                pageId, user.Id);
                if (pageFollower != null)
                {
                    pageFollower.User = _userManagerReturn.SetUserToReturn(user);
                    pageFollower.Page = page;
                    return StatusCodeReturn<object>
                        ._200_Success("Page follower found successfully", pageFollower);
                }
                return StatusCodeReturn<object>
                    ._404_NotFound("Page follower not found");
            }
            return StatusCodeReturn<object>
                    ._404_NotFound("Page not found");
        }

        public async Task<ApiResponse<object>> GetPageFollowerAsync(string pageFollowersId)
        {
            var pageFollower = await _pagesFollowersRepository.GetByIdAsync(pageFollowersId);
            if (pageFollower != null)
            {
                return StatusCodeReturn<object>
                    ._200_Success("Page follower found successfully", pageFollower);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Page follower not found");
        }

        public async Task<ApiResponse<object>> GetPageFollowersByPageIdAsync(string pageId)
        {
            var followers = await _pagesFollowersRepository.GetPageFollowersAsync(pageId);
            if (followers.ToList().Count == 0)
            {
                return StatusCodeReturn<object>
                    ._200_Success("No followers found for this page", followers);
            }
            return StatusCodeReturn<object>
                    ._200_Success("Followers for this page found successfully", followers);
        }

        public async Task<ApiResponse<object>> UnFollowPageAsync(
            UnFollowPageDto unFollowPageDto, SiteUser user)
        {
            var page = await _pageRepository.GetByIdAsync(unFollowPageDto.PageId);
            if (page != null)
            {
                var pageFollower = await _pagesFollowersRepository.GetPageFollowerByPageIdAndFollowerIdAsync(
                unFollowPageDto.PageId, user.Id);
                if (pageFollower != null)
                {
                    await _pagesFollowersRepository.UnfollowPageByPageIdAsync(unFollowPageDto.PageId, user.Id);
                    pageFollower.User = _userManagerReturn.SetUserToReturn(user);
                    pageFollower.Page = page;
                    return StatusCodeReturn<object>
                        ._200_Success("Unfollowed successfully", pageFollower);
                }
                return StatusCodeReturn<object>
                    ._404_NotFound("Page follower not found");
            }
            return StatusCodeReturn<object>
                    ._404_NotFound("Page not found");
        }

        public async Task<ApiResponse<object>> UnFollowPageAsync(string pageFollowersId, SiteUser user)
        {
            var pageFollower = await _pagesFollowersRepository.GetByIdAsync(pageFollowersId);
            if (pageFollower != null)
            {
                if(pageFollower.FollowerId == user.Id)
                {
                    await _pagesFollowersRepository.DeleteByIdAsync(pageFollowersId);
                    return StatusCodeReturn<object>
                        ._200_Success("Unfollowed successfully", pageFollower);
                }
                return StatusCodeReturn<object>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Page follower not found");
        }


    }
}

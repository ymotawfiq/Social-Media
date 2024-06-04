

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FollowerRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.FollowerService
{
    public class FollowerService : IFollowerService
    {
        private readonly IFollowerRepository _followerRepository;
        private readonly UserManager<SiteUser> _userManager;
        private readonly UserManagerReturn _userManagerReturn;
        public FollowerService(IFollowerRepository _followerRepository,
            UserManager<SiteUser> _userManager, UserManagerReturn _userManagerReturn)
        {
            this._followerRepository = _followerRepository;
            this._userManager = _userManager;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<Follower>> FollowAsync(FollowDto followDto, SiteUser user)
        {
            try
            {
                var followedPerson = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                    followDto.UserIdOrUserNameOrEmail);
                if (followedPerson != null)
                {
                    var isFollowing = await _followerRepository.GetFollowingByUserIdAndFollowerIdAsync(
                    user.Id, followedPerson.Id);
                    if (isFollowing == null)
                    {
                        followDto.UserIdOrUserNameOrEmail = followedPerson.Id;
                        var follow = await _followerRepository.FollowAsync(
                            ConvertFromDto.ConvertFromFollowerDto_Add(followDto, user));
                        return StatusCodeReturn<Follower>
                            ._200_Success("Followed successfully");
                    }
                    return StatusCodeReturn<Follower>
                        ._403_Forbidden("You already following this person");
                }

                return StatusCodeReturn<Follower>
                        ._404_NotFound("User you want to follow not found");

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<Follower>> FollowAsync(SiteUser user, SiteUser follower)
        {
            var follow = await _followerRepository.FollowAsync(new Follower
            {
                Id = Guid.NewGuid().ToString(),
                FollowerId = follower.Id,
                UserId = user.Id
            }); ;
            return StatusCodeReturn<Follower>
                ._200_Success("Followed successfully", follow);
        }

        public async Task<ApiResponse<IEnumerable<Follower>>> GetAllFollowers(string userId)
        {
            var followers = await _followerRepository.GetAllFollowers(userId);
            if (followers.ToList().Count==0)
            {
                return StatusCodeReturn<IEnumerable<Follower>>
                    ._200_Success("No followers found", followers);
            }
            return StatusCodeReturn<IEnumerable<Follower>>
                    ._200_Success("Followers found successfully", followers);
        }

        public async Task<ApiResponse<Follower>> UnfollowAsync(UnFollowDto unFollowDto, SiteUser user)
        {
            var followedPerson = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                unFollowDto.UserIdOrUserNameOrEmail);
            if (followedPerson != null)
            {
                var isFollowed = await _followerRepository.GetFollowingByUserIdAndFollowerIdAsync(
                user.Id, followedPerson.Id);
                if (isFollowed != null)
                {
                    var unfollow = await _followerRepository.UnfollowAsync(user.Id, followedPerson.Id);
                    return StatusCodeReturn<Follower>
                        ._200_Success("Unfollowed successfully", unfollow);
                }
                return StatusCodeReturn<Follower>
                    ._403_Forbidden("You are not following this person");
            }

            return StatusCodeReturn<Follower>
                         ._404_NotFound("User you want to unfollow not found");

        }
    }
}

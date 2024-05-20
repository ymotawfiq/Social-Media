

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
        public FollowerService(IFollowerRepository _followerRepository,
            UserManager<SiteUser> _userManager)
        {
            this._followerRepository = _followerRepository;
            this._userManager = _userManager;
        }
        public async Task<ApiResponse<Follower>> FollowAsync(FollowerDto followersDto)
        {
            try
            {
                var follower = await _userManager.FindByIdAsync(followersDto.FollowerId);
                var user = await _userManager.FindByIdAsync(followersDto.UserId);
                var isFollowing = await _followerRepository.GetFollowingByUserIdAndFollowerIdAsync(
                    user!.Id, follower!.Id);
                if (isFollowing != null)
                {
                    return StatusCodeReturn<Follower>
                        ._400_BadRequest("You already following this person");
                }
                var follow = await _followerRepository.FollowAsync(
                    ConvertFromDto.ConvertFromFollowerDto_Add(followersDto));
                return StatusCodeReturn<Follower>
                    ._200_Success("Followed successfully");
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task<ApiResponse<Follower>> UnfollowAsync(string userId, string followerId)
        {

            if(userId == followerId)
            {
                return StatusCodeReturn<Follower>
                    ._403_Forbidden("You can't follow yourself");
            }
            var isFollowed = await _followerRepository.GetFollowingByUserIdAndFollowerIdAsync(userId, followerId);
            if (isFollowed == null)
            {
                return StatusCodeReturn<Follower>
                    ._400_BadRequest("You are not following this person");
            }
            var unfollow = await _followerRepository.UnfollowAsync(userId, followerId);
            return StatusCodeReturn<Follower>
                ._200_Success("Unfollowed successfully", unfollow);
        }
    }
}

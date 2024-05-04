

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FollowerRepository;

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
                    return new ApiResponse<Follower>
                    {
                        IsSuccess = false,
                        Message = "You already following this person",
                        StatusCode = 400
                    };
                }
                var follow = await _followerRepository.FollowAsync(
                    ConvertFromDto.ConvertFromFollowerDto_Add(followersDto));
                return new ApiResponse<Follower>
                {
                    IsSuccess = true,
                    Message = "Followed successfully",
                    StatusCode = 200
                };
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
                return new ApiResponse<IEnumerable<Follower>>
                {
                    IsSuccess = true,
                    Message = "No followers found",
                    StatusCode = 200
                };
            }
            return new ApiResponse<IEnumerable<Follower>>
            {
                IsSuccess = true,
                Message = "Followers found successfully",
                StatusCode = 200,
                ResponseObject = followers
            };
        }

        public async Task<ApiResponse<Follower>> UnfollowAsync(string userId, string followerId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var follower = await _userManager.FindByIdAsync(followerId);

            if(userId == followerId)
            {
                return new ApiResponse<Follower>
                {
                    StatusCode = 403,
                    IsSuccess = false,
                    Message = "Forbidden"
                };
            }
            var isFollowed = await _followerRepository.GetFollowingByUserIdAndFollowerIdAsync(userId, followerId);
            if (isFollowed == null)
            {
                return new ApiResponse<Follower>
                {
                    IsSuccess = false,
                    Message = "You are not following this person",
                    StatusCode = 400
                };
            }
            var unfollow = await _followerRepository.UnfollowAsync(userId, followerId);
            return new ApiResponse<Follower>
            {
                IsSuccess = true,
                Message = "Unfollowed successfully",
                StatusCode = 200,
                ResponseObject = unfollow
            };
        }
    }
}

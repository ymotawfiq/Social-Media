

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
                if (user == null)
                {
                    return new ApiResponse<Follower>
                    {
                        IsSuccess = false,
                        Message = "User you want to follow not found",
                        StatusCode = 404
                    };
                }
                if (follower == null)
                {
                    return new ApiResponse<Follower>
                    {
                        IsSuccess = false,
                        Message = "Follower not found",
                        StatusCode = 404
                    };
                }
                if (followersDto.UserId == followersDto.FollowerId)
                {
                    return new ApiResponse<Follower>
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Message = "You can't unfollow yourself"
                    };
                }
                var follow = await _followerRepository.FollowAsync(
                    ConvertFromDto.ConvertFromFollowerDto_Add(followersDto));
                return new ApiResponse<Follower>
                {
                    IsSuccess = true,
                    Message = "Followed successfully",
                    StatusCode = 201
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<IEnumerable<Follower>>> GetAllFollowers(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<IEnumerable<Follower>>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            var followers = await _followerRepository.GetAllFollowers(userId);
            if (followers == null)
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
            if (user == null)
            {
                return new ApiResponse<Follower>
                {
                    IsSuccess = false, 
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            if (follower == null)
            {
                return new ApiResponse<Follower>
                {
                    IsSuccess = false,
                    Message = "Follower not found",
                    StatusCode = 404
                };
            }
            if(userId == followerId)
            {
                return new ApiResponse<Follower>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "You can't unfollow yourself"
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

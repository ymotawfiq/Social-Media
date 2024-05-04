

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;

namespace SocialMedia.Service.FriendsService
{
    public class FriendService : IFriendService
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IFriendRequestRepository _friendRequestRepository;
        public FriendService(IFriendsRepository _friendsRepository,
            UserManager<SiteUser> _userManager, IFriendRequestRepository _friendRequestRepository)
        {
            this._friendsRepository = _friendsRepository;
            this._userManager = _userManager;
            this._friendRequestRepository = _friendRequestRepository;
        }
        public async Task<ApiResponse<Friend>> AddFriendAsync(FriendDto friendsDto)
        {
            var existFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(friendsDto.UserId,
                friendsDto.FriendId);
            if (existFriend != null)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "You are already friends",
                    StatusCode = 400
                };
            }
            var newFriend = await _friendsRepository.AddFriendAsync(
                ConvertFromDto.ConvertFromFriendtDto_Add(friendsDto));
            return new ApiResponse<Friend>
            {
                IsSuccess = true,
                Message = "Friend added successfully to your friend list",
                StatusCode = 201,
                ResponseObject = newFriend
            };
        }

        public async Task<ApiResponse<Friend>> DeleteFriendAsync(string userId, string friendId)
        {
            var isYourFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
            if (isYourFriend == null)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "Friend not in your friend list",
                    StatusCode = 404
                };
            }
            var deletedFriend = await _friendsRepository.DeleteFriendAsync(userId, friendId);
            if (deletedFriend == null)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "Can't delete friend",
                    StatusCode = 400
                };
            }
            deletedFriend.User = null;
            return new ApiResponse<Friend>
            {
                IsSuccess = true,
                Message = "Friend deleted successfully",
                StatusCode = 200,
                ResponseObject = deletedFriend
            };
        }

        public async Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(string userId)
        {
            var friends = await _friendsRepository.GetAllUserFriendsAsync(userId);
            
            if (friends.ToList().Count==0)
            {
                return new ApiResponse<IEnumerable<Friend>>
                {
                    IsSuccess = true,
                    Message = "No friends found",
                    StatusCode = 200
                };
            }
            return new ApiResponse<IEnumerable<Friend>>
            {
                IsSuccess = true,
                Message = "Friends found successfully",
                StatusCode = 200,
                ResponseObject = friends
            };
        }
    }
}

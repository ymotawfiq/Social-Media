

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
            var user = await _userManager.FindByIdAsync(friendsDto.UserId);
            var friend = await _userManager.FindByIdAsync(friendsDto.FriendId);
            if (user == null)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            if (friend == null)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "Friend not found",
                    StatusCode = 404
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
            var user = await _userManager.FindByIdAsync(userId);
            var friend = await _userManager.FindByIdAsync(friendId);
            if (user == null)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            if (friend == null)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "Friend not found",
                    StatusCode = 404
                };
            }

            if (user == friend)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "Friend and user id can't be same",
                    StatusCode = 400
                };
            }
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
            var friendRequest = await _friendRequestRepository.GetFriendRequestByUserAndPersonIdAsync(friendId, userId);
            var deletedFriendRequest = await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequest.Id);
            if (deletedFriendRequest == null)
            {
                return new ApiResponse<Friend>
                {
                    IsSuccess = false,
                    Message = "Can't delete friend",
                    StatusCode = 400
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
            
            if (friends == null)
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

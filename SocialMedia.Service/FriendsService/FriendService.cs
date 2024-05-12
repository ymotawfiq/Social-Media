

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendListPolicyRepository;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Service.FriendListPolicyService;
using System.Linq;

namespace SocialMedia.Service.FriendsService
{
    public class FriendService : IFriendService
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFriendListPolicyService _friendListPolicyService;
        public FriendService(IFriendsRepository _friendsRepository,
            UserManager<SiteUser> _userManager, IFriendRequestRepository _friendRequestRepository,
            IFriendListPolicyService _friendListPolicyService)
        {
            this._friendsRepository = _friendsRepository;
            this._userManager = _userManager;
            this._friendRequestRepository = _friendRequestRepository;
            this._friendListPolicyService = _friendListPolicyService;
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
            var userFriendList = await _friendsRepository.GetAllUserFriendsAsync(friendsDto.UserId);
            if (userFriendList == null || userFriendList.ToList().Count == 0)
            {
                await _friendListPolicyService.AddFriendListPolicyAsync(
                    new AddFriendListPolicyDto
                    {
                        PolicyIdOrName = "PUBLIC",
                        UserIdOrNameOrEmail = friendsDto.UserId
                    }
                    );
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

        public async Task<ApiResponse<bool>> IsUserFriendAsync(string userId, string friendId)
        {
            var check = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
            if (check == null)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Not friend",
                    StatusCode = 200,
                    ResponseObject = false
                };
            }
            return new ApiResponse<bool>
            {
                IsSuccess = true,
                Message = "Friend",
                StatusCode = 200,
                ResponseObject = true
            };
        }

        public async Task<ApiResponse<bool>> IsUserFriendOfFriendAsync(string userId, string friendId)
        {
            var friendsOfFriends = (await _friendsRepository.GetUserFriendsOfFriendsAsync(userId)).ToList();
            var friend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
            if (friend != null)
            {
                foreach (var f in friendsOfFriends)
                {
                    if (f.Contains(friend))
                    {
                        return new ApiResponse<bool>
                        {
                            IsSuccess = true,
                            Message = "Friend of friend",
                            StatusCode = 200,
                            ResponseObject = true
                        };
                    }
                }
            }
            
            return new ApiResponse<bool>
            {
                IsSuccess = true,
                Message = "Not friend of friend",
                StatusCode = 200,
                ResponseObject = false
            };
        }
    }
}

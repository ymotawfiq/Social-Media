

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
using SocialMedia.Service.GenericReturn;
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
                return StatusCodeReturn<Friend>
                    ._400_BadRequest("You are already friends");
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
            return StatusCodeReturn<Friend>
                ._201_Created("Friend added successfully to your friend list");
        }

        public async Task<ApiResponse<Friend>> DeleteFriendAsync(string userId, string friendId)
        {
            var isYourFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
            if (isYourFriend == null)
            {
                return StatusCodeReturn<Friend>
                    ._404_NotFound("Friend not in your friend list");
            }
            var deletedFriend = await _friendsRepository.DeleteFriendAsync(userId, friendId);
            if (deletedFriend == null)
            {
                return StatusCodeReturn<Friend>
                    ._500_ServerError("Can't delete friend");
            }
            deletedFriend.User = null;
            return StatusCodeReturn<Friend>
                ._200_Success("Friend deleted successfully", deletedFriend);
        }

        public async Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(string userId)
        {
            var friends = await _friendsRepository.GetAllUserFriendsAsync(userId);
            
            if (friends.ToList().Count==0)
            {
                return StatusCodeReturn<IEnumerable<Friend>>
                    ._200_Success("No friends found");
            }
            return StatusCodeReturn<IEnumerable<Friend>>
                    ._200_Success("Friends found successfully", friends);
        }

        public async Task<ApiResponse<bool>> IsUserFriendAsync(string userId, string friendId)
        {
            var check = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
            if (check == null)
            {
                return StatusCodeReturn<bool>
                    ._200_Success("Not friend");
            }
            return StatusCodeReturn<bool>
                    ._200_Success("Friend");
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
                        return StatusCodeReturn<bool>
                    ._200_Success("Friend of friend");
                    }
                }
            }

            return StatusCodeReturn<bool>
                    ._200_Success("Not friend of friend");
        }
    }
}

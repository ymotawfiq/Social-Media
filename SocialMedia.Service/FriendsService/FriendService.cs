

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Service.FriendListPolicyService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.FriendsService
{
    public class FriendService : IFriendService
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly IFriendListPolicyService _friendListPolicyService;
        private readonly IBlockRepository _blockRepository;
        private readonly IPolicyRepository _policyRepository;
        public FriendService(IFriendsRepository _friendsRepository,
            IFriendListPolicyService _friendListPolicyService, IBlockRepository _blockRepository,
            IPolicyRepository _policyRepository)
        {
            this._friendsRepository = _friendsRepository;
            this._friendListPolicyService = _friendListPolicyService;
            this._blockRepository = _blockRepository;
            this._policyRepository = _policyRepository;
        }
        public async Task<ApiResponse<Friend>> AddFriendAsync(
            AddFriendDto addFriendDto)
        {
            var existFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(addFriendDto.UserId,
                addFriendDto.FriendId);
            if (existFriend == null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    addFriendDto.FriendId, addFriendDto.UserId);
                if (isBlocked == null)
                {
                    var newFriend = await _friendsRepository.AddFriendAsync(
                    ConvertFromDto.ConvertFromFriendtDto_Add(addFriendDto));
                    newFriend.User = null;
                    return StatusCodeReturn<Friend>
                        ._201_Created("Friend added successfully to your friend list", newFriend);
                }
                return StatusCodeReturn<Friend>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<Friend>
                    ._403_Forbidden("You are already friends");
        }

        public async Task<ApiResponse<Friend>> DeleteFriendAsync(string userId, string friendId)
        {
            if (userId != friendId)
            {
                var isYourFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
                if (isYourFriend != null)
                {
                    await _friendsRepository.DeleteFriendAsync(userId, friendId);
                    return StatusCodeReturn<Friend>
                        ._200_Success("Friend deleted successfully", isYourFriend);
                }
                return StatusCodeReturn<Friend>
                        ._404_NotFound("Friend not in your friend list");
            }
            return StatusCodeReturn<Friend>
                ._403_Forbidden();
        }

        public async Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(SiteUser user, 
            SiteUser user1)
        {
            var friends = await _friendsRepository.GetAllUserFriendsAsync(user.Id);
            foreach(var friend in friends)
            {
                friend.User = null;
            }
            var freindListPolicy = await _friendListPolicyService.GetFriendListPolicyAsync(
                user.FriendListPolicyId!);
            if (freindListPolicy != null && freindListPolicy.ResponseObject != null)
            {
                var policy = await _policyRepository.GetPolicyByIdAsync(
                    freindListPolicy.ResponseObject.PolicyId);
                if (policy != null)
                {
                    if(user.Id == user1.Id || policy.PolicyType == "PUBLIC")
                    {
                        if (friends.ToList().Count == 0)
                        {
                            return StatusCodeReturn<IEnumerable<Friend>>
                                ._200_Success("No friends found");
                        }
                        return StatusCodeReturn<IEnumerable<Friend>>
                                ._200_Success("Friends found successfully", friends);
                    }
                    else if(policy.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await IsUserFriendAsync(user.Id, user1.Id);
                        if(isFriend==null || !isFriend.ResponseObject)
                        {
                            return StatusCodeReturn<IEnumerable<Friend>>
                                ._403_Forbidden();
                        }
                    }
                    else if (policy.PolicyType == "FRIENDS OF FRIENDS")
                    {
                        var isFriend = await IsUserFriendOfFriendAsync(user.Id, user1.Id);
                        if (isFriend == null || !isFriend.ResponseObject)
                        {
                            return StatusCodeReturn<IEnumerable<Friend>>
                                ._403_Forbidden();
                        }
                    }
                }
                return StatusCodeReturn<IEnumerable<Friend>>
                    ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<IEnumerable<Friend>>
                    ._404_NotFound("Friend list policy not found");
        }

        public async Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(SiteUser user)
        {
            var friends = await _friendsRepository.GetAllUserFriendsAsync(user.Id);
            if (friends.ToList().Count == 0)
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
                    ._404_NotFound("Not friend", false);
            }
            return StatusCodeReturn<bool>
                    ._200_Success("Friend", true);
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
                            ._200_Success("Friend of friend", true);
                    }
                }
            }

            return StatusCodeReturn<bool>
                    ._404_NotFound("Not friend of friend", false);
        }



    }
}

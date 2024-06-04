

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.FriendRequestService
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFriendService _friendService;
        private readonly IFriendsRepository _friendsRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IBlockRepository _blockRepository;
        public FriendRequestService(IFriendRequestRepository _friendRequestRepository
            , IFriendService _friendService,
            IFriendsRepository friendsRepository, UserManagerReturn _userManagerReturn,
            IBlockRepository _blockRepository)
        {
            this._friendRequestRepository = _friendRequestRepository;
            this._friendService = _friendService;
            _friendsRepository = friendsRepository;
            this._userManagerReturn = _userManagerReturn;
            this._blockRepository = _blockRepository;
        }

        public async Task<ApiResponse<FriendRequest>> AddFriendRequestAsync(
            AddFriendRequestDto addFriendRequestDto, SiteUser user)
        {
            var askToFriendUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                addFriendRequestDto.PersonIdOrUserNameOrEmail);
            if (askToFriendUser != null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    user.Id, askToFriendUser.Id);
                if(isBlocked == null)
                {
                    var isYouFriends = await _friendsRepository.GetFriendByUserAndFriendIdAsync(
                    user.Id, askToFriendUser.Id);
                    if (isYouFriends == null)
                    {
                        var isFriendRequestSent = await _friendRequestRepository
                            .GetFriendRequestByUserAndPersonIdAsync(user.Id, askToFriendUser.Id);
                        if (isFriendRequestSent == null)
                        {
                            var newFriendRequest = await _friendRequestRepository.AddFriendRequestAsync(
                                    ConvertFromDto.ConvertFromFriendRequestDto_Add(addFriendRequestDto, user));
                            newFriendRequest.User = null;
                            return StatusCodeReturn<FriendRequest>
                                ._200_Success("Friend request send successfully", newFriendRequest);
                        }
                        return StatusCodeReturn<FriendRequest>
                            ._403_Forbidden("Friend request already sent before");
                    }
                    return StatusCodeReturn<FriendRequest>
                    ._403_Forbidden("You are friends");
                }
                return StatusCodeReturn<FriendRequest>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<FriendRequest>
                        ._404_NotFound("User you want to send friend request not found");
        }

        public async Task<ApiResponse<FriendRequest>> DeleteFriendRequestByAsync(
            SiteUser user, string friendRequestId)
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
            if (friendRequest != null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    friendRequest.UserWhoReceivedId, friendRequest.UserWhoSendId);
                if (isBlocked == null)
                {
                    if (friendRequest.UserWhoSendId == user.Id
                    || friendRequest.UserWhoReceivedId == user.Id)
                    {
                        await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequestId);
                        return StatusCodeReturn<FriendRequest>
                                ._200_Success("Friend request deleted successfully");
                    }
                    return StatusCodeReturn<FriendRequest>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<FriendRequest>
                        ._403_Forbidden();
            }

            return StatusCodeReturn<FriendRequest>
                ._404_NotFound("Friend request not found");
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsAsync()
        {
            var friendRequsts = await _friendRequestRepository.GetAllFriendRequestsAsync();
            if (friendRequsts.ToList().Count==0)
            {
                return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found");
            }

            return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found", friendRequsts);
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsByUserIdAsync(
            string userId)
        {
            var userFriendRequsts = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(userId);
            if (userFriendRequsts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found");
            }

            return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found", userFriendRequsts);
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsByUserNameAsync(
            SiteUser user)
        {
            var userFriendRequsts = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(user.Id);
            if (userFriendRequsts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found");
            }

            return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found", userFriendRequsts);
        }

        public async Task<ApiResponse<FriendRequest>> GetFriendRequestByIdAsync(
            string friendRequestId, SiteUser user)
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
            if (friendRequest != null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    friendRequest.UserWhoSendId, friendRequest.UserWhoReceivedId);
                if (isBlocked == null)
                {
                    if (friendRequest.UserWhoSendId == user.Id
                    || friendRequest.UserWhoReceivedId == user.Id)
                    {
                        return StatusCodeReturn<FriendRequest>
                                ._200_Success("Friend request found successfully");
                    }
                    return StatusCodeReturn<FriendRequest>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<FriendRequest>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<FriendRequest>
                ._404_NotFound("Friend request not found");
        }

        public async Task<ApiResponse<FriendRequest>> UpdateFriendRequestAsync(
            UpdateFriendRequestDto updateFriendRequestDto)
        {     
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(
                updateFriendRequestDto.FriendRequestId);
            if (friendRequest != null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    friendRequest.UserWhoReceivedId, friendRequest.UserWhoSendId);
                if (isBlocked == null)
                {
                    if (updateFriendRequestDto.IsAccepted)
                    {
                        await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequest.Id);
                        await _friendService.AddFriendAsync(
                            new AddFriendDto
                            {
                                FriendId = friendRequest.UserWhoReceivedId,
                                UserId = friendRequest.UserWhoSendId
                            });
                        return StatusCodeReturn<FriendRequest>
                            ._200_Success("Friend request accepted successfully");
                    }
                    return StatusCodeReturn<FriendRequest>
                            ._200_Success("If you want to delete friend request use delete friend request");
                }
                return StatusCodeReturn<FriendRequest>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<FriendRequest>
                ._404_NotFound("Friend request not found");
        }
    }
}



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
            var check = await CheckAbilityToSendRequestAsync<FriendRequest>(addFriendRequestDto, user);
            if (check.IsSuccess)
            {
                addFriendRequestDto.PersonIdOrUserNameOrEmail = (await _userManagerReturn
                    .GetUserByUserNameOrEmailOrIdAsync(addFriendRequestDto.PersonIdOrUserNameOrEmail)).Id;
                var friendRequest = await _friendRequestRepository.AddAsync(
                    ConvertFromDto.ConvertFromFriendRequestDto_Add(addFriendRequestDto, user));
                return StatusCodeReturn<FriendRequest>
                    ._201_Created("Friend request sent ssuccessfully", friendRequest);
            }
            return check;
        }

        public async Task<ApiResponse<FriendRequest>> DeleteFriendRequestAsync(
            SiteUser user, string friendRequestId)
        {
            var friendRequest = await _friendRequestRepository.GetByIdAsync(friendRequestId);
            if (friendRequest != null)
            {
                if (friendRequest.UserWhoSendId == user.Id
                || friendRequest.UserWhoReceivedId == user.Id)
                {
                    await _friendRequestRepository.DeleteByIdAsync(friendRequestId);
                    return StatusCodeReturn<FriendRequest>
                            ._200_Success("Friend request deleted successfully");
                }
                return StatusCodeReturn<FriendRequest>
                    ._403_Forbidden();
            }

            return StatusCodeReturn<FriendRequest>
                ._404_NotFound("Friend request not found");
        }

        public async Task<ApiResponse<FriendRequest>> DeleteFriendRequestAsync(SiteUser sender, SiteUser receiver)
        {
            var friendRequest = await _friendRequestRepository.GetByUserAndPersonIdAsync(
                sender.Id, receiver.Id);
            if (friendRequest != null)
            {
                await _friendRequestRepository.DeleteByIdAsync(friendRequest.Id);
                return StatusCodeReturn<FriendRequest>
                        ._200_Success("Friend request deleted successfully");
            }
            return StatusCodeReturn<FriendRequest>
                ._404_NotFound("Friend request not found");
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsAsync()
        {
            var friendRequsts = await _friendRequestRepository.GetAllAsync();
            if (friendRequsts.ToList().Count==0)
            {
                return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found");
            }

            return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("Friend requests found successfully", friendRequsts);
        }

        public async Task<ApiResponse<FriendRequest>> GetFriendRequestByIdAsync(
            string friendRequestId, SiteUser user)
        {
            var friendRequest = await _friendRequestRepository.GetByIdAsync(friendRequestId);
            if (friendRequest != null)
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
                ._404_NotFound("Friend request not found");
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetReceivedFriendRequestsByUserIdAsync(
            string userId)
        {
            var requests = await _friendRequestRepository.GetReceivedFriendRequestsByUserIdAsync(userId);
            if (requests.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests received");
            }

            return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("Received friend requests found successfully", requests);
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetSentFriendRequestsByUserIdAsync(
            string userId)
        {
            var requests = await _friendRequestRepository.GetSentFriendRequestsByUserIdAsync(userId);
            if (requests.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests sent");
            }

            return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("Sent friend requests found successfully", requests);
        }

        public async Task<ApiResponse<FriendRequest>> UpdateFriendRequestAsync(
            UpdateFriendRequestDto updateFriendRequestDto, SiteUser user)
        {     
            var friendRequest = await _friendRequestRepository.GetByIdAsync(
                updateFriendRequestDto.FriendRequestId);
            if (friendRequest != null)
            {
                if(user.Id == friendRequest.UserWhoReceivedId)
                {
                    if (updateFriendRequestDto.IsAccepted)
                    {
                        await _friendRequestRepository.DeleteByIdAsync(friendRequest.Id);
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


        private async Task<ApiResponse<T>> CheckAbilityToSendRequestAsync<T>(
                AddFriendRequestDto addFriendRequestDto, SiteUser user)
        {
            var askToFriendUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                addFriendRequestDto.PersonIdOrUserNameOrEmail);
            if (askToFriendUser != null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    user.Id, askToFriendUser.Id);
                if (isBlocked == null)
                {
                    var isYouFriends = await _friendsRepository.GetByUserAndFriendIdAsync(
                    user.Id, askToFriendUser.Id);
                    if (isYouFriends == null)
                    {
                        var isFriendRequestSentBefore = await _friendRequestRepository
                            .GetByUserAndPersonIdAsync(user.Id, askToFriendUser.Id);
                        if (isFriendRequestSentBefore == null)
                        {
                            if (user.Id != askToFriendUser.Id)
                            {
                                return StatusCodeReturn<T>
                                ._200_Success("Success");
                            }
                            return StatusCodeReturn<T>
                            ._403_Forbidden();
                        }
                        return StatusCodeReturn<T>
                            ._403_Forbidden("Friend request already sent before");
                    }
                    return StatusCodeReturn<T>
                    ._403_Forbidden("You are friends");
                }
                return StatusCodeReturn<T>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<T>
                        ._404_NotFound("User you want to send friend request not found");
        }

    }
}

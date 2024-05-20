

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.FriendRequestService
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IFriendService _friendService;
        private readonly IFriendsRepository _friendsRepository;
        public FriendRequestService(IFriendRequestRepository _friendRequestRepository,
            UserManager<SiteUser> _userManager, IFriendService _friendService,
            IFriendsRepository friendsRepository)
        {
            this._friendRequestRepository = _friendRequestRepository;
            this._userManager = _userManager;
            this._friendService = _friendService;
            _friendsRepository = friendsRepository; 
        }

        public async Task<ApiResponse<FriendRequest>> AddFriendRequestAsync(FriendRequestDto friendRequestDto)
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestByUserAndPersonIdAsync(
                friendRequestDto.UserId, friendRequestDto.PersonId);
            if (friendRequest != null && friendRequest.IsAccepted == false)
            {
                return StatusCodeReturn<FriendRequest>
                    ._400_BadRequest("Friend request already sent or check your friend list");
            }
            var isYouFriends = await _friendsRepository.GetFriendByUserAndFriendIdAsync(
                friendRequestDto.UserId, friendRequestDto.PersonId);
            if (isYouFriends == null)
            {
                var newFriendRequest = await _friendRequestRepository.AddFriendRequestAsync(
                ConvertFromDto.ConvertFromFriendRequestDto_Add(friendRequestDto));
                newFriendRequest.User = null;
                return StatusCodeReturn<FriendRequest>
                    ._200_Success("Friend request send successfully", newFriendRequest);
            }
            return StatusCodeReturn<FriendRequest>
                ._400_BadRequest("You are friends");

        }

        public async Task<ApiResponse<FriendRequest>> DeleteFriendRequestByAsync
            (SiteUser user, Guid friendRequestId)
        {
            var friendRequests = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(user.Id);
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
            if (!friendRequests.ToList().Contains(friendRequest)
                && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return StatusCodeReturn<FriendRequest>
                ._400_BadRequest("You haven't sent friend request to this user");
            }
            await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequestId);
            return StatusCodeReturn<FriendRequest>
                    ._200_Success("Friend request deleted successfully");
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

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> 
            GetAllFriendRequestsByUserIdAsync(string userId)
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

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> 
            GetAllFriendRequestsByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userFriendRequsts = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(user.Id);
            if (userFriendRequsts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found");
            }

            return StatusCodeReturn<IEnumerable<FriendRequest>>
                    ._200_Success("No friend requests found", userFriendRequsts);
        }

        public async Task<ApiResponse<FriendRequest>> GetFriendRequestByIdAsync(Guid friendRequestId)
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
            if (friendRequest == null)
            {
                return StatusCodeReturn<FriendRequest>
                    ._404_NotFound("Friend request not found");
            }
            return StatusCodeReturn<FriendRequest>
                ._200_Success("Friend request found successfully", friendRequest);
        }

        public async Task<ApiResponse<FriendRequest>> UpdateFriendRequestAsync(FriendRequestDto friendRequestDto)
        {     
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(
                new Guid(friendRequestDto.Id!));
            if (friendRequest.IsAccepted == false)
            {
                if (friendRequestDto.IsAccepted == true)
                {
                    var deletedFriendRequest = await _friendRequestRepository
                        .DeleteFriendRequestByAsync(friendRequest.Id);
                    if (deletedFriendRequest == null)
                    {
                        return StatusCodeReturn<FriendRequest>
                            ._500_ServerError("Can't accept friend request");
                    }
                    var friendDto = new FriendDto
                    {
                        FriendId = friendRequestDto.UserId,
                        UserId = friendRequestDto.PersonId
                    };
                    
                    var newFriend = await _friendService.AddFriendAsync(friendDto);
                    if (newFriend == null)
                    {
                        return StatusCodeReturn<FriendRequest>
                            ._400_BadRequest("Friend request can't be accepted");
                    }

                    return StatusCodeReturn<FriendRequest>
                        ._200_Success("Friend request accepted successfully");
                }
                else
                {
                    var deletedFriendRequest = 
                        await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequest.Id);
                    if (deletedFriendRequest != null)
                    {
                        return StatusCodeReturn<FriendRequest>
                        ._200_Success("Friend request deleted successfully");
                    }
                }
            }
            return StatusCodeReturn<FriendRequest>
                ._500_ServerError("Can't accept friend request");
            
        }
    }
}

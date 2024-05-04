

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Service.FriendsService;

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
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Friend request already sent or check your friend list",
                    StatusCode = 400,
                };
            }
            var isYouFriends = await _friendsRepository.GetFriendByUserAndFriendIdAsync(
                friendRequestDto.UserId, friendRequestDto.PersonId);
            if (isYouFriends == null)
            {
                var newFriendRequest = await _friendRequestRepository.AddFriendRequestAsync(
                ConvertFromDto.ConvertFromFriendRequestDto_Add(friendRequestDto));
                newFriendRequest.User = null;
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = true,
                    Message = "Friend request send successfully",
                    StatusCode = 201,
                    ResponseObject = newFriendRequest
                };
            }
            return new ApiResponse<FriendRequest>
            {
                IsSuccess = false,
                Message = "You are friends",
                StatusCode = 400,
            };

        }

        public async Task<ApiResponse<FriendRequest>> DeleteFriendRequestByAsync
            (SiteUser user, Guid friendRequestId)
        {
            var friendRequests = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(user.Id);
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
            if (!friendRequests.ToList().Contains(friendRequest)
                && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = true,
                    Message = "You haven't sent friend request to this user",
                    StatusCode = 400,
                };
            }
            await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequestId);
            return new ApiResponse<FriendRequest>
            {
                IsSuccess = true,
                Message = "Friend request deleted successfully",
                StatusCode = 200,
            };
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsAsync()
        {
            var friendRequsts = await _friendRequestRepository.GetAllFriendRequestsAsync();
            if (friendRequsts.ToList().Count==0)
            {
                return new ApiResponse<IEnumerable<FriendRequest>>
                {
                    IsSuccess = true,
                    Message = "No friend requests found",
                    StatusCode = 200
                };
            }

            return new ApiResponse<IEnumerable<FriendRequest>>
            {
                IsSuccess = true,
                Message = "Friend requests found successfully",
                StatusCode = 200,
                ResponseObject = friendRequsts
            };
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> 
            GetAllFriendRequestsByUserIdAsync(string userId)
        {
            var userFriendRequsts = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(userId);
            if (userFriendRequsts.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<FriendRequest>>
                {
                    IsSuccess = true,
                    Message = "No friend requests found",
                    StatusCode = 200
                };
            }

            return new ApiResponse<IEnumerable<FriendRequest>>
            {
                IsSuccess = true,
                Message = "Friend requests found successfully",
                StatusCode = 200,
                ResponseObject = userFriendRequsts
            };
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> 
            GetAllFriendRequestsByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userFriendRequsts = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(user.Id);
            if (userFriendRequsts.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<FriendRequest>>
                {
                    IsSuccess = true,
                    Message = "No friend requests found",
                    StatusCode = 200
                };
            }

            return new ApiResponse<IEnumerable<FriendRequest>>
            {
                IsSuccess = true,
                Message = "Friend requests found successfully",
                StatusCode = 200,
                ResponseObject = userFriendRequsts
            };
        }

        public async Task<ApiResponse<FriendRequest>> GetFriendRequestByIdAsync(Guid friendRequestId)
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
            if (friendRequest == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Friend request not found",
                    StatusCode = 404
                };
            }
            return new ApiResponse<FriendRequest>
            {
                IsSuccess = true,
                Message = "Friend request found successfully",
                StatusCode = 200,
                ResponseObject = friendRequest
            };
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
                        return new ApiResponse<FriendRequest>
                        {
                            IsSuccess = false,
                            Message = "Can't accept driend request",
                            StatusCode = 500
                        };
                    }
                    var friendDto = new FriendDto
                    {
                        FriendId = friendRequestDto.UserId,
                        UserId = friendRequestDto.PersonId
                    };
                    
                    var newFriend = await _friendService.AddFriendAsync(friendDto);
                    if (newFriend == null)
                    {
                        return new ApiResponse<FriendRequest>
                        {
                            IsSuccess = false,
                            Message = "Friend request can't be accepted",
                            StatusCode = 400,
                        };
                    }
                    
                    return new ApiResponse<FriendRequest>
                    {
                        IsSuccess = true,
                        Message = "Friend request accepted successfully",
                        StatusCode = 201,
                    };
                }
                else
                {
                    var deletedFriendRequest = 
                        await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequest.Id);
                    if (deletedFriendRequest != null)
                    {
                        return new ApiResponse<FriendRequest>
                        {
                            StatusCode = 200,
                            Message = "Friend request deleted successfully",
                            IsSuccess = false
                        };
                    }
                }
            }
            return new ApiResponse<FriendRequest>
            {
                StatusCode = 500,
                Message = "Can't accept friend request",
                IsSuccess = false
            };
            
        }
    }
}



using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.ChatRequestRepository;
using SocialMedia.Api.Service.BlockService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.UserChatService;

namespace SocialMedia.Api.Service.ChatRequestService
{
    public class ChatRequestService : IChatRequestService
    {
        private readonly IChatRequestRepository _chatRequestRepository;
        private readonly IUserChatService _userChatService;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IBlockService _blockService;
        public ChatRequestService(IChatRequestRepository _chatRequestRepository,
            UserManagerReturn _userManagerReturn, IBlockService _blockService,
            IUserChatService _userChatService)
        {
            this._chatRequestRepository = _chatRequestRepository;
            this._userManagerReturn = _userManagerReturn;
            this._blockService = _blockService;
            this._userChatService = _userChatService;
        }

        public async Task<ApiResponse<ChatRequest>> AcceptChatRequestAsync(string requestId, SiteUser user)
        {
            var chatRequest = await _chatRequestRepository.GetByIdAsync(requestId);
            if (chatRequest != null)
            {
                if(chatRequest.UserWhoReceivedRequestId == user.Id)
                {
                    var acceptRequest = await _userChatService.AddUserChatAsync(new AddUserChatDto
                    {
                        UserIdOrNameOrEmail = chatRequest.UserWhoSentRequestId
                    }, user);
                    if (acceptRequest.IsSuccess)
                    {
                        await DeleteChatRequestByIdAsync(requestId, user);
                        return StatusCodeReturn<ChatRequest>
                            ._200_Success("Chat request accepted successfully");
                    }
                    return StatusCodeReturn<ChatRequest>
                        ._500_ServerError(acceptRequest.Message);
                }
                return StatusCodeReturn<ChatRequest>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<ChatRequest>
                    ._404_NotFound("Chat request not found");
        }

        public async Task<ApiResponse<ChatRequest>> AddChatRequestAsync(AddChatRequestDto addChatRequestDto,
            SiteUser user)
        {
            var receivedUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                addChatRequestDto.UserIdOrNameOrEmail);
            if(receivedUser != null)
            {
                var isBlocked = await _blockService.GetBlockByUserIdAndBlockedUserIdAsync(
                    receivedUser.Id, user.Id);
                if (isBlocked.ResponseObject == null)
                {
                    var isSentBefore = await _chatRequestRepository.GetChatRequestAsync(user, receivedUser);
                    if (isSentBefore == null)
                    {
                        addChatRequestDto.UserIdOrNameOrEmail = receivedUser.Id;
                        var newChatRequest = await _chatRequestRepository.AddAsync(ConvertFromDto
                            .ConvertFromChatRequestDto_Add(addChatRequestDto, user));
                        return StatusCodeReturn<ChatRequest>
                            ._201_Created("Chat request sent successfully", newChatRequest);
                    }
                    return StatusCodeReturn<ChatRequest>
                        ._403_Forbidden("Chat request already sent before");
                }
                return StatusCodeReturn<ChatRequest>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<ChatRequest>
                    ._404_NotFound("User you want to send chat request not found");
        }

        public async Task<ApiResponse<ChatRequest>> DeleteChatRequestByIdAsync(string id, SiteUser user)
        {
            var chatRequest = await GetChatRequestByIdAsync(id, user);
            if (chatRequest.ResponseObject != null && chatRequest.IsSuccess)
            {
                await _chatRequestRepository.DeleteByIdAsync(id);
                return StatusCodeReturn<ChatRequest>
                        ._200_Success("Chat request deleted successfully", chatRequest.ResponseObject);
            }
            return chatRequest;
        }

        public async Task<ApiResponse<ChatRequest>> GetChatRequestByIdAsync(string id, SiteUser user)
        {
            var chatRequest = await _chatRequestRepository.GetByIdAsync(id);
            if (chatRequest != null)
            {
                if(chatRequest.UserWhoReceivedRequestId == user.Id
                    || chatRequest.UserWhoSentRequestId == user.Id)
                {
                    return StatusCodeReturn<ChatRequest>
                        ._200_Success("Chat request found successfully", chatRequest);
                }
                return StatusCodeReturn<ChatRequest>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<ChatRequest>
                    ._404_NotFound("Chat request not found");
        }

        public async Task<ApiResponse<ChatRequest>> GetChatRequestByUserAsync(SiteUser user1, SiteUser user2)
        {
            var chatRequest = await _chatRequestRepository.GetChatRequestAsync(user1, user2);
            return await GetChatRequestByIdAsync(chatRequest.Id, user2);
        }

        public async Task<ApiResponse<IEnumerable<ChatRequest>>> GetReceivedChatRequestsAsync(SiteUser user)
        {
            var chatRequests = await _chatRequestRepository.GetReceivedChatRequestsAsync(user);
            if (chatRequests.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<ChatRequest>>
                    ._200_Success("No chat requests found", chatRequests);
            }
            return StatusCodeReturn<IEnumerable<ChatRequest>>
                    ._200_Success("Chat requests found successfully", chatRequests);
        }

        public async Task<ApiResponse<IEnumerable<ChatRequest>>> GetSentChatRequestsAsync(SiteUser user)
        {
            var chatRequests = await _chatRequestRepository.GetSentChatRequestsAsync(user);
            if (chatRequests.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<ChatRequest>>
                    ._200_Success("No chat requests found", chatRequests);
            }
            return StatusCodeReturn<IEnumerable<ChatRequest>>
                    ._200_Success("Chat requests found successfully", chatRequests);
        }
    }
}



using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.ChatRequestRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.ChatRequestService
{
    public class ChatRequestService : IChatRequestService
    {
        private readonly IChatRequestRepository _chatRequestRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public ChatRequestService(IChatRequestRepository _chatRequestRepository,
            UserManagerReturn _userManagerReturn)
        {
            this._chatRequestRepository = _chatRequestRepository;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<ChatRequest>> AddChatRequestAsync(AddChatRequestDto addChatRequestDto,
            SiteUser user)
        {
            var receivedUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                addChatRequestDto.UserIdOrNameOrEmail);
            if(receivedUser != null)
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
                if(chatRequest.UserWhoReceivedRequestId==user.Id
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

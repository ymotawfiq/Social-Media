using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.ChatRepository;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;

namespace SocialMedia.Api.Service.ChatService
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IPolicyService _policyService;
        public ChatService(IChatRepository _chatRepository, IPolicyService _policyService)
        {
            this._chatRepository = _chatRepository;
            this._policyService = _policyService;
        }

        public async Task<ApiResponse<Chat>> AddNonPrivateChatAsync(
            AddChatDto addChatDto, SiteUser user)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync("public");
            if (policy.ResponseObject != null)
            {
                if (policy.ResponseObject.PolicyType != "PRIVATE")
                {
                    var newChat = await _chatRepository.AddAsync(ConvertFromDto
                    .ConvertFromNonPrivateChatDto_Add(addChatDto, policy.ResponseObject, user));
                    return StatusCodeReturn<Chat>
                        ._201_Created("Chat created successfully", newChat);
                }
                return StatusCodeReturn<Chat>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<Chat>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Chat>> AddPrivateChatAsync(AddChatDto addChatDto,
            SiteUser user)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync("private");
            if (policy.ResponseObject != null)
            {
                if (policy.ResponseObject.PolicyType == "PRIVATE")
                {
                    var newChat = await _chatRepository.AddAsync(ConvertFromDto
                    .ConvertFromPrivateChatDto_Add(addChatDto, policy.ResponseObject));
                    return StatusCodeReturn<Chat>
                        ._201_Created("Chat created successfully", newChat);
                }
                return StatusCodeReturn<Chat>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<Chat>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Chat>> GetChatAsync(string chatId)
        {
            return StatusCodeReturn<Chat>
                ._200_Success("", await _chatRepository.GetByIdAsync(chatId));
        }
    }
}

using Microsoft.CodeAnalysis.Elfie.Serialization;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.ChatMemberRepository;
using SocialMedia.Api.Repository.ChatMessageRepository;
using SocialMedia.Api.Repository.ChatRepository;
using SocialMedia.Api.Repository.MessageReactRepository;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PrivateChatRepository;
using SocialMedia.Api.Repository.ReactRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.MessageReactService
{
    public class MessageReactService : IMessageReactService
    {
        private readonly IMessageReactRepository _messageReactRepository;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IChatMemberRepository _chatMemberRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IReactRepository _reactRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public MessageReactService(IMessageReactRepository _messageReactRepository,
            IChatMessageRepository _chatMessageRepository, IPrivateChatRepository _privateChatRepository,
            IChatMemberRepository _chatMemberRepository, IChatRepository _chatRepository,
            IPolicyRepository _policyRepository, IReactRepository _reactRepository,
            UserManagerReturn _userManagerReturn)
        {
            this._chatMessageRepository = _chatMessageRepository;
            this._messageReactRepository = _messageReactRepository;
            this._privateChatRepository = _privateChatRepository;
            this._chatMemberRepository = _chatMemberRepository;
            this._chatRepository = _chatRepository;
            this._policyRepository = _policyRepository;
            this._reactRepository = _reactRepository;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<IEnumerable<MessageReact>>> GetReactsByMessageIdAsync(string messageId,
            SiteUser user)
        {
            var messageReacts = await _messageReactRepository.GetMessageReactsByMessageIdAsync(messageId);
            var message = await _chatMessageRepository.GetByIdAsync(messageId);
            if (message != null)
            {
                if ((await IsChatMemberAsync<IEnumerable<MessageReact>>(message.ChatId, user)).IsSuccess)
                {
                    if (messageReacts.ToList().Count == 0)
                    {
                        return StatusCodeReturn<IEnumerable<MessageReact>>
                        ._200_Success("No message reacts found", messageReacts);
                    }
                    return StatusCodeReturn<IEnumerable<MessageReact>>
                        ._200_Success("Message reacts found successfully", messageReacts);
                }
                return await IsChatMemberAsync<IEnumerable<MessageReact>>(message.ChatId, user);
            }
            return StatusCodeReturn<IEnumerable<MessageReact>>
                        ._404_NotFound("Message not found");
        }

        public async Task<ApiResponse<MessageReact>> ReactToMessageAsync(AddMessageReactDto addMessageReactDto,
            SiteUser user)
        {
            var chatMessage = await _chatMessageRepository.GetByIdAsync(addMessageReactDto.MessageId);
            if (chatMessage != null)
            {
                var react = await _reactRepository.GetByIdAsync(addMessageReactDto.ReactId);
                if (react != null)
                {
                    var chat = await _chatRepository.GetByIdAsync(chatMessage.ChatId);
                    if ((await IsChatMemberAsync<MessageReact>(chatMessage.ChatId, user)).IsSuccess)
                    {
                        if ((await IsAbleToModifyPrivateMessageAsync<MessageReact>(chat, user)).IsSuccess)
                        {
                            var messageReact = await _messageReactRepository.AddAsync(
                                ConvertFromDto.ConvertAddMessageReactDto_Add(addMessageReactDto, user));
                            messageReact.React = react;
                            messageReact.User = _userManagerReturn.SetUserToReturn(user);
                            return StatusCodeReturn<MessageReact>
                                ._201_Created("React to message successfully", messageReact);
                        }
                        return await IsAbleToModifyPrivateMessageAsync<MessageReact>(chat, user);
                    }
                    return await IsChatMemberAsync<MessageReact>(chatMessage.ChatId, user);
                }
                return StatusCodeReturn<MessageReact>
                    ._404_NotFound("React not found");
            }
            return StatusCodeReturn<MessageReact>
                    ._404_NotFound("Message not found");
        }

        public async Task<ApiResponse<MessageReact>> UnReactToMessageByMessageIdAsync(string messageId,
            SiteUser user)
        {
            var chatMessage = await _chatMessageRepository.GetByIdAsync(messageId);
            if (chatMessage != null)
            {
                var messageReact = await _messageReactRepository.GetMessageReactByMessageAndUserIdAsync(
                    messageId, user.Id);
                if (messageReact != null)
                {
                    var chat = await _chatRepository.GetByIdAsync(chatMessage.ChatId);
                    if ((await IsAbleToModifyPrivateMessageAsync<MessageReact>(chat, user)).IsSuccess)
                    {
                        await _messageReactRepository.DeleteByIdAsync(messageReact.Id);
                        messageReact.React = await _reactRepository.GetByIdAsync(messageReact.ReactId);
                        messageReact.User = _userManagerReturn.SetUserToReturn(user);
                        return StatusCodeReturn<MessageReact>
                            ._200_Success("Un reacted to message successfully", messageReact);
                    }
                    return await IsAbleToModifyPrivateMessageAsync<MessageReact>(chat, user);
                }
                return StatusCodeReturn<MessageReact>
                    ._404_NotFound("Message react not found");
            }
            return StatusCodeReturn<MessageReact>
                    ._404_NotFound("Message not found");
        }

        public async Task<ApiResponse<MessageReact>> UnReactToMessageByMessageReactIdAsync(
            string messageReactId, SiteUser user)
        {
            var messageReact = await _messageReactRepository.GetByIdAsync(messageReactId);
            return await UnReactToMessageByMessageIdAsync(messageReact.MessageId, user);
        }

        private async Task<ApiResponse<T>> IsChatMemberAsync<T>(string chatId, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                var isPrivateChatMember = await _privateChatRepository.GetByMemberAndChatIdAsync(
                    chatId, user.Id);
                var isGroupChatMember = await _chatMemberRepository.GetByMemberAndChatIdAsync(
                    chatId, user.Id);
                if (isGroupChatMember != null || isPrivateChatMember != null)
                {
                    return StatusCodeReturn<T>
                    ._200_Success("Allowed");
                }
                return StatusCodeReturn<T>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<T>
                    ._404_NotFound("Chat not found");
        }

        private async Task<ApiResponse<T>> IsAbleToModifyPrivateMessageAsync<T>(
                Chat chat, SiteUser user)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync("private");
            var privateChatMember = await _privateChatRepository.GetByMemberAndChatIdAsync(chat.Id, user.Id);
            if (policy != null)
            {
                if (policy.Id == chat.Id)
                {
                    if (privateChatMember != null)
                    {
                        if (privateChatMember.IsBlocked)
                        {
                            return StatusCodeReturn<T>
                                ._403_Forbidden();
                        }
                        return StatusCodeReturn<T>
                                ._200_Success("Allowed");
                    }
                    return StatusCodeReturn<T>
                                ._404_NotFound("Not chat member");
                }
                return StatusCodeReturn<T>
                                ._200_Success("Allowed");
            }
            return StatusCodeReturn<T>
                                ._404_NotFound("Private policy not found");
        }


    }
}

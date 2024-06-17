

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.ChatMessageRepository;
using SocialMedia.Repository.MessageReactRepository;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Repository.UserChatRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.MessageReactService
{
    public class MessageReactService : IMessageReactService
    {
        private readonly IMessageReactRepository _messageReactRepository;
        private readonly IReactRepository _reactRepository;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IUserChatRepository _userChatRepository;
        public MessageReactService(IMessageReactRepository _messageReactRepository, 
            IReactRepository _reactRepository, IChatMessageRepository _chatMessageRepository,
            IUserChatRepository _userChatRepository)
        {
            this._chatMessageRepository = _chatMessageRepository;
            this._messageReactRepository = _messageReactRepository;
            this._reactRepository = _reactRepository;
            this._userChatRepository = _userChatRepository;
        }
        public async Task<ApiResponse<MessageReact>> AddReactToMessageAsync(
            AddMessageReactDto addMessageReactDto, SiteUser user)
        {
            if((await IsAbleToReactAsync(addMessageReactDto, user)).IsSuccess)
            {
                var messageReact = await _messageReactRepository.AddAsync(ConvertFromDto
                    .ConvertFromMessageReactDto_Add(addMessageReactDto, user));
                return StatusCodeReturn<MessageReact>
                    ._201_Created("Reacted to message successfully", messageReact);
            }
            return await IsAbleToReactAsync(addMessageReactDto, user);
        }



        public async Task<ApiResponse<MessageReact>> DeleteReactToMessageAsync(
            string messageReactId, SiteUser user)
        {
            var messageReact = await GetReactToMessageAsync(messageReactId, user);
            if (messageReact.IsSuccess && messageReact.ResponseObject != null)
            {
                await _messageReactRepository.DeleteByIdAsync(messageReactId);
                return StatusCodeReturn<MessageReact>
                    ._200_Success("Message react deleted successfully", messageReact.ResponseObject);
            }
            return messageReact;
        }

        public async Task<ApiResponse<IEnumerable<MessageReact>>> GetMessageReactsAsync(
            string messageId, SiteUser user)
        {
            var reacts = await _messageReactRepository.GetMessageReactsAsync(messageId);
            var message = await _chatMessageRepository.GetByIdAsync(messageId);
            if (message != null)
            {
                var chat = await _userChatRepository.GetByIdAsync(message.ChatId);
                if(user.Id == chat.User1Id || user.Id == chat.User2Id)
                {
                    if (reacts.ToList().Count == 0)
                    {
                        return StatusCodeReturn<IEnumerable<MessageReact>>
                            ._200_Success("No reacts found", reacts);
                    }
                    return StatusCodeReturn<IEnumerable<MessageReact>>
                            ._200_Success("Reacts found successfully", reacts);
                }
                return StatusCodeReturn<IEnumerable<MessageReact>>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<IEnumerable<MessageReact>>
                    ._404_NotFound("Message not found");
        }

        public async Task<ApiResponse<MessageReact>> GetReactToMessageAsync(
            string messageReactId, SiteUser user)
        {
            var messageReact = await _messageReactRepository.GetByIdAsync(messageReactId);
            if (messageReact != null)
            {
                var message = await _chatMessageRepository.GetByIdAsync(messageReact.MessageId);
                var chat = await _userChatRepository.GetByIdAsync(message.ChatId);
                if (chat.User1Id == user.Id || chat.User2Id == user.Id)
                {
                    return StatusCodeReturn<MessageReact>
                        ._200_Success("Message react found successfully", messageReact);
                }
                return StatusCodeReturn<MessageReact>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<MessageReact>
                ._404_NotFound("Message react not found");
        }

        public async Task<ApiResponse<MessageReact>> UpdateReactToMessageAsync(
            UpdateMessageReactDto updateMessageReactDto, SiteUser user)
        {
            var messageReact = await _messageReactRepository.GetByIdAsync(updateMessageReactDto.Id);
            if (messageReact != null)
            {
                var react = await _reactRepository.GetByIdAsync(updateMessageReactDto.ReactId);
                if (react != null)
                {
                    if(messageReact.ReactedUserId == user.Id)
                    {
                        if(react.Id != messageReact.ReactId)
                        {
                            messageReact.ReactId = react.Id;
                            var updatedMessageReact = await _messageReactRepository.UpdateAsync(messageReact);
                            return StatusCodeReturn<MessageReact>
                                ._200_Success("Message react updated successfully", updatedMessageReact);
                        }
                        return StatusCodeReturn<MessageReact>
                        ._403_Forbidden("Unable to update to same react");
                    }
                    return StatusCodeReturn<MessageReact>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<MessageReact>
                    ._404_NotFound("React not found");
            }
            return StatusCodeReturn<MessageReact>
                    ._404_NotFound("Message react not found");
        }


        private async Task<ApiResponse<MessageReact>> IsAbleToReactAsync(
                AddMessageReactDto addMessageReactDto, SiteUser user)
        {
            var message = await _chatMessageRepository.GetByIdAsync(addMessageReactDto.MessageId);
            if (message != null)
            {
                var react = await _reactRepository.GetByIdAsync(addMessageReactDto.ReactId);
                if (react != null)
                {
                    var chat = await _userChatRepository.GetByIdAsync(message.ChatId);
                    if (chat != null)
                    {
                        if (chat.User1Id == user.Id || chat.User2Id == user.Id)
                        {
                            return StatusCodeReturn<MessageReact>
                                ._200_Success("Success");
                        }
                        return StatusCodeReturn<MessageReact>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<MessageReact>
                            ._404_NotFound("Chat not found");
                }
                return StatusCodeReturn<MessageReact>
                            ._404_NotFound("React not found");
            }
            return StatusCodeReturn<MessageReact>
                            ._404_NotFound("Message not found");
        }

    }
}

using Microsoft.AspNetCore.Hosting;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.ChatMemberRepository;
using SocialMedia.Api.Repository.ChatMessageRepository;
using SocialMedia.Api.Repository.ChatRepository;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PrivateChatRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.ChatMessageService
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IChatMemberRepository _chatMemberRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPolicyRepository _policyRepository;
        public ChatMessageService(IChatMessageRepository _chatMessageRepository, 
            IChatRepository _chatRepository, IWebHostEnvironment _webHostEnvironment, 
            IPrivateChatRepository _privateChatRepository, IChatMemberRepository _chatMemberRepository,
            IPolicyRepository _policyRepository)
        {
            this._chatRepository = _chatRepository;
            this._chatMessageRepository = _chatMessageRepository;
            this._chatMemberRepository = _chatMemberRepository;
            this._privateChatRepository = _privateChatRepository;
            this._webHostEnvironment = _webHostEnvironment;
            this._policyRepository = _policyRepository;
        }
        public async Task<ApiResponse<IEnumerable<ChatMessage>>> GetChatMessagesAsync(string chatId,
            SiteUser user)
        {
            var messages = await _chatMessageRepository.GetMessagesByChatIdAsync(chatId);
            if ((await IsChatMemberAsync<IEnumerable<ChatMessage>>(chatId, user)).IsSuccess)
            {
                if (messages.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<ChatMessage>>
                            ._200_Success("No messages found", messages);
                }
                return StatusCodeReturn<IEnumerable<ChatMessage>>
                            ._200_Success("Messages found successfully", messages);
            }
            return await IsChatMemberAsync<IEnumerable<ChatMessage>>(chatId, user);
        }

        public async Task<ApiResponse<ChatMessage>> SendMessageAsync(AddChatMessageDto addChatMessageDto,
            SiteUser user)
        {
            if((await IsAbleToSendMessageAsync(addChatMessageDto, user)).IsSuccess)
            {
                var chat = await _chatRepository.GetByIdAsync(addChatMessageDto.ChatId);
                if((await IsAbleToModifyPrivateMessageAsync(chat, user)).IsSuccess)
                {
                    var newMessage = await _chatMessageRepository.AddAsync(ConvertFromDto
                        .ConvertAddChatMessageDto_Add(addChatMessageDto,
                        SaveMessageImage(addChatMessageDto.Photo!), user));
                    return StatusCodeReturn<ChatMessage>
                        ._201_Created("Message sent successfully", newMessage);
                }
                return await IsAbleToModifyPrivateMessageAsync(chat, user);
            }
            return await IsAbleToSendMessageAsync(addChatMessageDto, user);
        }

        public async Task<ApiResponse<ChatMessage>> UnSendMessageAsync(string messageId, SiteUser user)
        {
            var message = await _chatMessageRepository.GetByIdAsync(messageId);
            if (message != null)
            {
                var chat = await _chatRepository.GetByIdAsync(message.ChatId);
                if ((await IsAbleToModifyPrivateMessageAsync(chat, user)).IsSuccess)
                {
                    if (message.SenderId == user.Id)
                    {
                        if (message.Photo != null)
                        {
                            DeleteMessageImage(message.Photo);
                        }
                        await _chatMessageRepository.DeleteByIdAsync(messageId);
                        return StatusCodeReturn<ChatMessage>
                            ._200_Success("Message unsent successfully", message);
                    }
                    return StatusCodeReturn<ChatMessage>
                        ._403_Forbidden();
                }
                return await IsAbleToModifyPrivateMessageAsync(chat, user);
            }
            return StatusCodeReturn<ChatMessage>
                    ._404_NotFound("Message not found");
        }

        public async Task<ApiResponse<ChatMessage>> UnpdateMessageAsync(
            UpdateChatMessageDto updateChatMessageDto, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(updateChatMessageDto.ChatId);
            if (chat != null)
            {
                var message = await _chatMessageRepository.GetByIdAsync(updateChatMessageDto.MessageId);
                if (message != null)
                {
                    if ((await IsAbleToModifyPrivateMessageAsync(chat, user)).IsSuccess)
                    {
                        if (message.Photo == null && message.SenderId == user.Id)
                        {
                            if (message.SentAt.AddMinutes(5).Minute < DateTime.Now.Minute
                                && message.SentAt.Hour == DateTime.Now.Hour
                                && message.SentAt.Day == DateTime.Now.Day)
                            {
                                message.Message = updateChatMessageDto.Message;
                                message.UpdatedAt = DateTime.Now;
                                await _chatMessageRepository.UpdateAsync(message);
                            }
                            return StatusCodeReturn<ChatMessage>
                                    ._403_Forbidden("You can update message only in 5 minutes after sending");
                        }
                        return StatusCodeReturn<ChatMessage>
                                    ._403_Forbidden();
                    }
                    return await IsAbleToModifyPrivateMessageAsync(chat, user);
                }
                return StatusCodeReturn<ChatMessage>
                        ._404_NotFound("Message not found");
            }
            return StatusCodeReturn<ChatMessage>
                ._404_NotFound("Chat not found");
        }

        private async Task<ApiResponse<ChatMessage>> IsAbleToModifyPrivateMessageAsync(
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
                            return StatusCodeReturn<ChatMessage>
                                ._403_Forbidden();
                        }
                        return StatusCodeReturn<ChatMessage>
                                ._200_Success("Allowed");
                    }
                    return StatusCodeReturn<ChatMessage>
                                ._404_NotFound("Not chat member");
                }
                return StatusCodeReturn<ChatMessage>
                                ._200_Success("Allowed");
            }
            return StatusCodeReturn<ChatMessage>
                                ._404_NotFound("Private policy not found");
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

        private async Task<ApiResponse<ChatMessage>> IsAbleToSendMessageAsync(
            AddChatMessageDto addChatMessageDto, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(addChatMessageDto.ChatId);
            if (chat != null)
            {
                if (addChatMessageDto.Message == null)
                {
                    if (addChatMessageDto.Photo == null)
                    {
                        return StatusCodeReturn<ChatMessage>
                            ._403_Forbidden("You must enter message or photo");
                    }
                    return StatusCodeReturn<ChatMessage>
                        ._200_Success("allowed");
                }
                else if (addChatMessageDto.Photo == null)
                {
                    if (addChatMessageDto.Message == null)
                    {
                        return StatusCodeReturn<ChatMessage>
                            ._403_Forbidden("You must enter message or photo");
                    }
                    return StatusCodeReturn<ChatMessage>
                        ._200_Success("allowed");
                }
                if ((await IsChatMemberAsync<ChatMessage>(addChatMessageDto.ChatId, user)).IsSuccess)
                {
                    return StatusCodeReturn<ChatMessage>
                        ._200_Success("allowed");
                }
                return await IsChatMemberAsync<ChatMessage>(addChatMessageDto.ChatId, user);
            }
            return StatusCodeReturn<ChatMessage>
                    ._404_NotFound("Chat not found");
        }

        private string SaveMessageImage(IFormFile image)
        {
            if (image == null)
            {
                return null!;
            }
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\Message_Images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
                fileStream.Flush();
            }
            return uniqueFileName;
        }

        private bool DeleteMessageImage(string imageUrl)
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\Message_Images\");
            var file = Path.Combine(path, $"{imageUrl}");
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
                return true;
            }
            return false;
        }

        
    }
}

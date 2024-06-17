
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.ChatMessageRepository;
using SocialMedia.Repository.ChatRequestRepository;
using SocialMedia.Repository.UserChatRepository;
using SocialMedia.Service.BlockService;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.ChatMessageService
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IUserChatRepository _userChatRepository;
        private readonly IChatRequestRepository _chatRequestRepository;
        private readonly IBlockService _blockService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ChatMessageService(IChatMessageRepository _chatMessageRepository,
            IUserChatRepository _userChatRepository, IChatRequestRepository _chatRequestRepository,
            IBlockService _blockService,
            IWebHostEnvironment _webHostEnvironment)
        {
            this._blockService = _blockService;
            this._chatMessageRepository = _chatMessageRepository;
            this._chatRequestRepository = _chatRequestRepository;
            this._userChatRepository = _userChatRepository;
            this._webHostEnvironment = _webHostEnvironment;
        }

        public async Task<ApiResponse<ChatMessage>> ReplayToMessageAsync(
            AddChatMessageReplayDto addChatMessageReplayDto, SiteUser user)
        {
            var message = await _chatMessageRepository.GetByIdAsync(addChatMessageReplayDto.MessageId);
            if (message != null)
            {
                var chat = await _userChatRepository.GetByIdAsync(message.ChatId);
                if (chat != null)
                {
                    if (chat.User1Id == user.Id || chat.User2Id == user.Id)
                    {
                        var replay = await _chatMessageRepository.AddAsync(ConvertFromDto
                        .ConvertFromChatMessageReplayDto_Add(addChatMessageReplayDto, user, chat,
                        SaveChatImages(addChatMessageReplayDto.Photo!)));
                        return StatusCodeReturn<ChatMessage>
                            ._201_Created("Replayed successfully", replay);
                    }
                    return StatusCodeReturn<ChatMessage>
                    ._403_Forbidden();
                }
                return StatusCodeReturn<ChatMessage>
                ._404_NotFound("Chat not found");
            }
            return StatusCodeReturn<ChatMessage>
                ._404_NotFound("Message not found");
        }

        public async Task<ApiResponse<ChatMessage>> DeleteMessageAsync(string messageId, string chatId,
            SiteUser user)
        {
            var message = await GetMessageAsync(messageId, chatId, user);
            if(message.IsSuccess && message.ResponseObject != null)
            {
                if (message.ResponseObject.Photo != null)
                {
                    DeleteChatImage(message.ResponseObject.Photo);
                }
                await _chatMessageRepository.DeleteByIdAsync(message.ResponseObject.Id);
                return StatusCodeReturn<ChatMessage>
                            ._200_Success("Message deleted successfully", message.ResponseObject);
            }
            return message;
        }

        public async Task<ApiResponse<ChatMessage>> GetMessageAsync(string messageId, string chatId,
            SiteUser user)
        {
            var chat = await _userChatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                var message = await _chatMessageRepository.GetByIdAsync(messageId);
                if (message != null)
                {
                    if(message.SenderId == chat.User1Id || message.SenderId == chat.User2Id)
                    {
                        return StatusCodeReturn<ChatMessage>
                            ._200_Success("Message found successfully", message);
                    }
                    return StatusCodeReturn<ChatMessage>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<ChatMessage>
                        ._404_NotFound("Message not found");
            }
            return StatusCodeReturn<ChatMessage>
                        ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<IEnumerable<ChatMessage>>> GetUserMessagesAsync(SiteUser user,
            string chatId)
        {
            var chat = await _userChatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                var messages = await _chatMessageRepository.GetUserSentMessagesAsync(user, chatId);
                if (messages.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<ChatMessage>>
                        ._200_Success("You didn't send messages yet", messages);
                }
                return StatusCodeReturn<IEnumerable<ChatMessage>>
                        ._200_Success("Messages found successfully", messages);
            }
            return StatusCodeReturn<IEnumerable<ChatMessage>>
                ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<ChatMessage>> SendMessageAsync(AddChatMessageDto addChatMessageDto,
            SiteUser user)
        {
            var userChat = await _userChatRepository.GetByIdAsync(addChatMessageDto.ChatId);
            if (userChat != null)
            {
                if(!(await IsBlockedAsync(userChat, user)).IsSuccess)
                {
                    return await SaveMessageAsync(addChatMessageDto, user);
                }
                return StatusCodeReturn<ChatMessage>
                    ._403_Forbidden();
            }
            var chatRequest = await _chatRequestRepository.GetByIdAsync(addChatMessageDto.ChatId);
            if (chatRequest != null && !chatRequest.IsAccepted)
            {
                return StatusCodeReturn<ChatMessage>
                    ._403_Forbidden("Wait untill user accept your chat request");
            }
            return StatusCodeReturn<ChatMessage>
                ._404_NotFound("No chats found send chat request to user before sending messages");
        }

        public async Task<ApiResponse<ChatMessage>> UpdateMessageAsync(
            UpdateChatMessageDto updateChatMessageDto, SiteUser user)
        {
            var userChat = await _userChatRepository.GetByIdAsync(updateChatMessageDto.ChatId);
            if (userChat != null)
            {
                var message = await _chatMessageRepository.GetByIdAsync(updateChatMessageDto.MessageId);
                if (message != null)
                {
                    if (message.SentAt.AddMinutes(5).Minute < DateTime.Now.Minute)
                    {
                        if(message.Photo == null)
                        {
                            message.Message = updateChatMessageDto.Message;
                            var updatedMessage = await _chatMessageRepository.UpdateAsync(message);
                            return StatusCodeReturn<ChatMessage>
                                ._200_Success("Message updated successfully", updatedMessage);
                        }
                        return StatusCodeReturn<ChatMessage>
                        ._403_Forbidden("You can't update photo with text");
                    }
                    return StatusCodeReturn<ChatMessage>
                        ._403_Forbidden("You can update only in 5 minutes of sending message");
                }
                return StatusCodeReturn<ChatMessage>
                ._404_NotFound("Message not found");
            }
            return StatusCodeReturn<ChatMessage>
                ._404_NotFound("Chat not found");
        }

        private async Task<ApiResponse<ChatMessage>> IsBlockedAsync(UserChat userChat, SiteUser user)
        {
            string receiverId = "";
            if(userChat.User1Id == user.Id)
            {
                receiverId = userChat.User2Id;
            }
            else
            {
                receiverId = userChat.User1Id;
            }
            var isBlocked = await _blockService.GetBlockByUserIdAndBlockedUserIdAsync(user.Id, receiverId);
            if (isBlocked.ResponseObject == null)
            {
                return StatusCodeReturn<ChatMessage>
                    ._404_NotFound("User not in your block list");
            }
            return StatusCodeReturn<ChatMessage>
                    ._200_Success("User in your block list");
        }

        private async Task<ApiResponse<ChatMessage>> SaveMessageAsync(AddChatMessageDto addChatMessageDto,
            SiteUser user)
        {
            var newMessage = await _chatMessageRepository.AddAsync(ConvertFromDto
                    .ConvertFromChatMessageDto_Add(addChatMessageDto, user,
                    SaveChatImages(addChatMessageDto.Photo!)));
            return StatusCodeReturn<ChatMessage>
                ._201_Created("Message saved successfully", newMessage);
        }

        private string SaveChatImages(IFormFile image)
        {
            if (image == null)
            {
                return null!;
            }
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\Chat_Images");
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

        private bool DeleteChatImage(string imageUrl)
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\Chat_Images\");
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

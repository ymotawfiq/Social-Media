

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.ArchievedChatRepository;
using SocialMedia.Repository.UserChatRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.ArchievedChatService
{
    public class ArchievedChatService : IArchievedChatService
    {
        private readonly IArchievedChatRepository _archievedChatRepository;
        private readonly IUserChatRepository _userChatRepository;
        public ArchievedChatService(IArchievedChatRepository _archievedChatRepository,
            IUserChatRepository _userChatRepository)
        {
            this._archievedChatRepository = _archievedChatRepository;
            this._userChatRepository = _userChatRepository;
        }
        public async Task<ApiResponse<ArchievedChat>> ArchieveChatAsync(ArchieveChatDto archieveChatDto,
            SiteUser user)
        {
            var chat = await _userChatRepository.GetByIdAsync(archieveChatDto.ChatId);
            if (chat != null)
            {
                var archievedChat = await _archievedChatRepository.GetByChatAndUserIdAsync(chat.Id, user.Id);
                if (archievedChat == null)
                {
                    var newArchievedChat = await _archievedChatRepository.AddAsync(ConvertFromDto
                        .ConvertFromArchievedChatDto_Add(archieveChatDto, user));
                    return StatusCodeReturn<ArchievedChat>
                        ._201_Created("Chat archieved successfully", newArchievedChat);
                }
                return StatusCodeReturn<ArchievedChat>
                        ._403_Forbidden("Chat already in archieve");
            }
            return StatusCodeReturn<ArchievedChat>
                        ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<ArchievedChat>> GetArchieveChatByChatIdAsync(string chatId,
            SiteUser user)
        {
            var archievedChat = await _archievedChatRepository.GetByChatAndUserIdAsync(chatId, user.Id);
            if (archievedChat != null)
            {
                return StatusCodeReturn<ArchievedChat>
                    ._200_Success("Archieved chat found successfully", archievedChat);
            }
            return StatusCodeReturn<ArchievedChat>
                        ._404_NotFound("Archieved chat not found");
        }

        public async Task<ApiResponse<ArchievedChat>> GetArchieveChatByIdAsync(string archievedChatId,
            SiteUser user)
        {
            var archievedChat = await _archievedChatRepository.GetByIdAsync(archievedChatId);
            if (archievedChat != null)
            {
                if(archievedChat.UserId == user.Id)
                {
                    return StatusCodeReturn<ArchievedChat>
                    ._200_Success("Archieved chat found successfully", archievedChat);
                }
                return StatusCodeReturn<ArchievedChat>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<ArchievedChat>
                        ._404_NotFound("Archieved chat not found");
        }

        public async Task<ApiResponse<IEnumerable<ArchievedChat>>> GetUserArchieveChatsAsync(SiteUser user)
        {
            var chats = await _archievedChatRepository.GetUserArchievedChatsAsync(user.Id);
            if (chats.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<ArchievedChat>>
                    ._200_Success("Arcieve is empty", chats);
            }
            return StatusCodeReturn<IEnumerable<ArchievedChat>>
                    ._200_Success("Arcieved chats found successfully", chats);
        }

        public async Task<ApiResponse<ArchievedChat>> UnArchieveChatByChatIdAsync(string chatId, SiteUser user)
        {
            var archievedChat = await _archievedChatRepository.GetByChatAndUserIdAsync(chatId, user.Id);
            if (archievedChat != null)
            {
                await _archievedChatRepository.DeleteByIdAsync(archievedChat.Id);
                return StatusCodeReturn<ArchievedChat>
                ._200_Success("Chat unarchieved successfully", archievedChat);
            }
            return StatusCodeReturn<ArchievedChat>
                        ._404_NotFound("Archieved chat not found");
        }

        public async Task<ApiResponse<ArchievedChat>> UnArchieveChatByIdAsync(string archievedChatId,
            SiteUser user)
        {
            var archievedChat = await _archievedChatRepository.GetByIdAsync(archievedChatId);
            if (archievedChat != null)
            {
                if (archievedChat.UserId == user.Id)
                {
                    await _archievedChatRepository.DeleteByIdAsync(archievedChat.Id);
                    return StatusCodeReturn<ArchievedChat>
                    ._200_Success("Chat unarchieved successfully", archievedChat);
                }
                return StatusCodeReturn<ArchievedChat>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<ArchievedChat>
                        ._404_NotFound("Archieved chat not found");
        }
    }
}

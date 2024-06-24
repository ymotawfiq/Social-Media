using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.ArchievedChatRepository;
using SocialMedia.Api.Repository.ChatMemberRepository;
using SocialMedia.Api.Repository.ChatRepository;
using SocialMedia.Api.Repository.PrivateChatRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.ArchievedChatService
{
    public class ArchievedChatService : IArchievedChatService
    {
        private readonly IArchievedChatRepository _archievedChatRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IChatMemberRepository _chatMemberRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public ArchievedChatService(IArchievedChatRepository _archievedChatRepository,
            IChatRepository _chatRepository, IPrivateChatRepository _privateChatRepository,
            IChatMemberRepository _chatMemberRepository, UserManagerReturn _userManagerReturn)
        {
            this._chatRepository = _chatRepository;
            this._archievedChatRepository = _archievedChatRepository;
            this._chatMemberRepository = _chatMemberRepository;
            this._privateChatRepository = _privateChatRepository;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<ArchievedChat>> ArchieveChatAsync(string chatId, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                var privateChat = await _privateChatRepository.GetByMemberAndChatIdAsync(chatId, user.Id);
                var groupChat = await _chatMemberRepository.GetByMemberAndChatIdAsync(chatId, user.Id);
                if (privateChat != null || groupChat != null)
                {
                    var existArchievedChat = await _archievedChatRepository.GetByUserAndChatIdAsync(
                        user.Id, chatId);
                    if (existArchievedChat == null)
                    {
                        var archievedChat = await _archievedChatRepository.AddAsync(new ArchievedChat
                        {
                            ChatId = chatId,
                            UserId = user.Id,
                            Id = Guid.NewGuid().ToString(),
                            Chat = chat,
                            User = user
                        });
                        return StatusCodeReturn<ArchievedChat>
                            ._201_Created("Archieved successfully", archievedChat);
                    }
                    return StatusCodeReturn<ArchievedChat>
                            ._403_Forbidden("Already archieved");
                }
                return StatusCodeReturn<ArchievedChat>
                            ._403_Forbidden();
            }
            return StatusCodeReturn<ArchievedChat>
                            ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<IEnumerable<ArchievedChat>>> GetUserArchieveChatsAsync(SiteUser user)
        {
            var archievedChats = await _archievedChatRepository.GetAllByUserIdAsync(user.Id);
            if (archievedChats.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<ArchievedChat>>
                    ._200_Success("No archieved chats found", archievedChats);
            }
            return StatusCodeReturn<IEnumerable<ArchievedChat>>
                    ._200_Success("Archieved chats found successfully", archievedChats);
        }

        public async Task<ApiResponse<ArchievedChat>> UnArchieveChatByArchievedChatIdAsync(
            string archievedChatId, SiteUser user)
        {
            var archievedChat = await _archievedChatRepository.GetByIdAsync(archievedChatId);
            return await UnArchieveChatByChatIdAsync(archievedChat.ChatId, user);
        }

        public async Task<ApiResponse<ArchievedChat>> UnArchieveChatByChatIdAsync(string chatId, SiteUser user)
        {
            var archievedChat = await _archievedChatRepository.GetByUserAndChatIdAsync(user.Id, chatId);
            if (archievedChat != null)
            {
                archievedChat.Chat = await _chatRepository.GetByIdAsync(chatId);
                archievedChat.User = _userManagerReturn.SetUserToReturn(user);
                await _archievedChatRepository.DeleteByIdAsync(archievedChat.Id);
                return StatusCodeReturn<ArchievedChat>
                            ._200_Success("Unarchieved successfully", archievedChat);
            }
            return StatusCodeReturn<ArchievedChat>
                            ._404_NotFound("Archieved chat not found");
        }

    }
}

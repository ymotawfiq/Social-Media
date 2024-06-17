

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.ChatMessageRepository;
using SocialMedia.Repository.UserChatRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.UserChatService
{
    public class UserChatService : IUserChatService
    {
        private readonly IUserChatRepository _userChatRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IChatMessageRepository _chatMessageRepository;
        public UserChatService(IUserChatRepository _userChatRepository, UserManagerReturn _userManagerReturn,
            IChatMessageRepository _chatMessageRepository)
        {
            this._userChatRepository = _userChatRepository;
            this._userManagerReturn = _userManagerReturn;
            this._chatMessageRepository = _chatMessageRepository;
        }
        public async Task<ApiResponse<UserChat>> AddUserChatAsync(
            AddUserChatDto addUserChatDto, SiteUser user)
        {
            var user2 = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                addUserChatDto.UserIdOrNameOrEmail);
            if (user2 != null)
            {
                var userChat = await _userChatRepository.GetByUser1AndUser2Async(user.Id, user2.Id);
                if (userChat == null)
                {
                    addUserChatDto.UserIdOrNameOrEmail = user2.Id;
                    var newChat = await _userChatRepository.AddAsync(ConvertFromDto
                        .ConvertFromUserChatDto_Add(addUserChatDto, user));
                    return StatusCodeReturn<UserChat>
                        ._200_Success("Chat created successfully", newChat);
                }
                return StatusCodeReturn<UserChat>
                    ._403_Forbidden("Chat already exists with this user");
            }
            return StatusCodeReturn<UserChat>
                ._404_NotFound("User you want to add chat with not found");
        }

        public async Task<ApiResponse<UserChat>> DeleteUserChatByIdAsync(string chatId, SiteUser user)
        {
            var chat = await GetUserChatByIdAsync(chatId, user);
            if (chat.IsSuccess && chat.ResponseObject != null)
            {
                if(await _chatMessageRepository.IsChatEmptyAsync(user, chatId))
                {
                    await _userChatRepository.DeleteByIdAsync(chatId);
                    return StatusCodeReturn<UserChat>
                        ._200_Success("Chat deleted successfully", chat.ResponseObject);
                }
                return StatusCodeReturn<UserChat>
                    ._403_Forbidden("Unable to delete chat because it is not empty");
            }
            return chat;
        }

        public async Task<ApiResponse<UserChat>> DeleteUserChatByUserAsync(string user1Id, string user2Id)
        {
            var chat = await _userChatRepository.GetByUser1AndUser2Async(user2Id, user1Id);
            var anyUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(user1Id);
            if (chat != null)
            {
                return await DeleteUserChatByIdAsync(chat.Id, anyUser);
            }
            return StatusCodeReturn<UserChat>
                    ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<UserChat>> GetUserChatByIdAsync(string chatId, SiteUser user)
        {
            var chat = await _userChatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                if (chat.User1Id == user.Id || chat.User2Id == user.Id)
                {
                    return StatusCodeReturn<UserChat>
                        ._200_Success("Chat deleted successfully", chat);
                }
                return StatusCodeReturn<UserChat>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<UserChat>
                    ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<IEnumerable<UserChat>>> GetUserChatsAsync(SiteUser user)
        {
            var chats = await _userChatRepository.GetUserChatsAsync(user.Id);
            if (chats.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<UserChat>>
                    ._200_Success("No chats found", chats);
            }
            return StatusCodeReturn<IEnumerable<UserChat>>
                    ._200_Success("Chats found successfully", chats);
        }
    }
}

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.ChatManagerService
{
    public interface IChatManagerService
    {
        Task<ApiResponse<Chat>> AddGroupChatAsync(AddChatDto addChatDto, SiteUser user);
        Task<ApiResponse<Chat>> GetGroupChatByIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<Chat>> UpdateGroupChatByIdAsync(UpdateChatDto updateChatDto, SiteUser user);
        Task<ApiResponse<Chat>> DeleteGroupChatByIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<IEnumerable<Chat>>> GetGroupChatsCreatedByUserAsync(SiteUser user);


        Task<ApiResponse<IEnumerable<PrivateChat>>> GetReceivedPrivateChatRequestsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PrivateChat>>> GetSentPrivateChatRequestsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PrivateChat>>> GetPrivateChatsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<PrivateChat>>> GetNotAcceptedPrivateChatRequestsAsync(SiteUser user);
        Task<ApiResponse<PrivateChat>> AcceptPrivateChatRequestAsync(string chatId, SiteUser user);
        Task<ApiResponse<PrivateChat>> AddPrivateChatRequestAsync(string userIdOrNameOrEmail, SiteUser user1);
        Task<ApiResponse<PrivateChat>> UnSendPrivateChatRequestAsync(string chatMemberId, SiteUser user);
        Task<ApiResponse<PrivateChat>> BlockChatByChatIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<PrivateChat>> BlockChatByPrivateChatIdAsync(string privateChatId, SiteUser user);
        Task<ApiResponse<PrivateChat>> UnBlockChatByChatIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<PrivateChat>> UnBlockChatByPrivateChatIdAsync(string privateChatId, SiteUser user);


        Task<ApiResponse<ChatMember>> AddChatMemberAsync(AddChatMemberDto addChatMemberDto, SiteUser user);
        Task<ApiResponse<ChatMember>> AddChatJoinRequestAsync(string chatId, SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMember>>> GetNotAcceptedGroupChatRequestsAsync(SiteUser user);
        Task<ApiResponse<ChatMember>> AcceptChatJoinRequestAsync(string chatMemberId,SiteUser user);
        Task<ApiResponse<ChatMember>> DeleteChatMemberAsync(string chatMemberId, SiteUser admin);
        Task<ApiResponse<ChatMember>> UnSendGroupChatRequestAsync(string chatMemberId, SiteUser user);
        Task<ApiResponse<ChatMember>> GetChatMemberAsync(string chatMemberId, SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMember>>> GetChatMembersAsync(string chatId, SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMember>>> GetChatJoinRequestsAsync(string chatId, SiteUser user);


        Task<ApiResponse<IEnumerable<ChatMemberRole>>> GetMemberRolesByMemberIdAsync(string chatMemberId,
            SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMemberRole>>> GetMemberRolesByChatIdAsync(string chatId,
            SiteUser user);
        Task<ApiResponse<ChatMemberRole>> AssigntMemberToRoleAsync(string chatMemberId,
            string roleIdOrName, SiteUser user);
        Task<ApiResponse<ChatMemberRole>> DeleteMemberFromRoleAsync(ChatMemberRoleDto chatMemberRoleDto,
            SiteUser user);
        Task<ApiResponse<ChatMemberRole>> DeleteMemberFromRoleAsync(string chatMemberRoleId, SiteUser user);
        Task<ApiResponse<ChatMemberRole>> IsInRoleRoleAsync(string chatId, string roleIdOrName, SiteUser user);

    }
}

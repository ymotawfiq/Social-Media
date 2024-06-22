using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.ChatManagerService
{
    public interface IChatManagerService
    {
        Task<ApiResponse<ChatMember>> AddChatMemberAsync(AddChatMemberDto addChatMemberDto,
            SiteUser user);
        Task<ApiResponse<Chat>> AddGroupChatAsync(AddChatDto addChatDto, SiteUser user);
        Task<ApiResponse<ChatMember>> AddChatJoinRequestAsync(string chatId, SiteUser user);
        Task<ApiResponse<ChatMember>> AddPrivateChatRequestAsync(
            string userIdOrNameOrEmail, SiteUser user1);
        Task<ApiResponse<IEnumerable<ChatMember>>> GetPrivateChatsAsync(SiteUser user);
        Task<ApiResponse<ChatMember>> AcceptPrivateChatRequestAsync(string chatId, 
            SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMember>>> GetPrivateChatRequestsAsync(SiteUser user);
        Task<ApiResponse<ChatMember>> AcceptChatJoinRequestAsync(string chatMemberId,
            SiteUser user);
        Task<ApiResponse<ChatMember>> DeleteChatMemberAsync(string chatMemberId,SiteUser admin);
        Task<ApiResponse<ChatMember>> GetChatMemberAsync(string chatMemberId, SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMemberRole>>> GetMemberRolesByMemberIdAsync(
            string chatMemberId, SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMemberRole>>> GetMemberRolesByChatIdAsync(
            string chatId, SiteUser user);
        Task<ApiResponse<ChatMemberRole>> AssigntMemberToRoleAsync(string chatMemberId,
            string roleIdOrName, SiteUser user);
        Task<ApiResponse<ChatMemberRole>> DeleteMemberFromRoleAsync(
            ChatMemberRoleDto chatMemberRoleDto, SiteUser user);
        Task<ApiResponse<ChatMemberRole>> DeleteMemberFromRoleAsync(string chatMemberRoleId,
            SiteUser user);
        Task<ApiResponse<ChatMemberRole>> IsInRoleRoleAsync(string chatId, string roleIdOrName,
            SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMember>>> GetChatMembersAsync(string chatId, 
            SiteUser user);

        Task<ApiResponse<IEnumerable<ChatMember>>> GetChatJoinRequestsAsync(string chatId,
                SiteUser user);

    }
}

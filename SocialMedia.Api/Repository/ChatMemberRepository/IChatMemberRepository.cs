using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ChatMemberRepository
{
    public interface IChatMemberRepository : ICrud<ChatMember>
    {
        Task<IEnumerable<ChatMember>> GetChatMembersAsync(string chatId);
        Task<IEnumerable<ChatMember>> GetGroupChatJoinRequestsAsync(string chatId);
        Task<ChatMember> GetByMemberAndChatIdAsync(string chatId, string memberId);
        Task<IEnumerable<ChatMember>> GetNotAcceptedGroupChatRequestsAsync(string userId);


    }
}

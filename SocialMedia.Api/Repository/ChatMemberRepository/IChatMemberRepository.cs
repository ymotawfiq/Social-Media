using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ChatMemberRepository
{
    public interface IChatMemberRepository : ICrud<ChatMember>
    {
        Task<IEnumerable<ChatMember>> GetChatMembersAsync(string chatId);
        Task<IEnumerable<ChatMember>> GetPrivateChatRequestsAsync(string member2Id);
        Task<IEnumerable<ChatMember>> GetGroupChatJoinRequestsAsync(string chatId);
        Task<ChatMember> GetByMemberAndChatIdAsync(string chatId, string memberId);
        Task<bool> AddRangeAsync(List<ChatMember> members);
        Task<ChatMember> GetByMember1AndMember2IdAsync(string member1Id, string member2Id);
        public async Task<IEnumerable<ChatMember>> GetPrivateChatsAsync(string userId)


    }
}

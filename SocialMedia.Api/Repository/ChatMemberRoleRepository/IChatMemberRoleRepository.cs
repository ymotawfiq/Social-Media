using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ChatMemberRoleRepository
{
    public interface IChatMemberRoleRepository : ICrud<ChatMemberRole>
    {
        Task<IEnumerable<ChatMemberRole>> GetMemberRolesAsync(string chatMemberId);
        Task<ChatMemberRole> GetByChatAndRoleIdAsync(string chatMemberId, string roleId);
    }
}

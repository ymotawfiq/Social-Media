using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ChatRepository
{
    public interface IChatRepository : ICrud<Chat>
    {
        Task<IEnumerable<Chat>> GetUserCreatedChatsAsync(string userId);
    }
}

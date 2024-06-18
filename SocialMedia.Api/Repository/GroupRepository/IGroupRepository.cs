


using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.GroupRepository
{
    public interface IGroupRepository : ICrud<Group>
    {
        Task<IEnumerable<Group>> GetAllGroupsByUserIdAsync(string userId);
    }
}

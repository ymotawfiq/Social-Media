


using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.GroupRepository
{
    public interface IGroupRepository : ICrud<Group>
    {
        Task<IEnumerable<Group>> GetAllGroupsByUserIdAsync(string userId);
    }
}

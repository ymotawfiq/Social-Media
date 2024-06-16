

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.GroupRoleRepository
{
    public interface IGroupRoleRepository : ICrud<GroupRole>
    {
        Task<GroupRole> GetGroupRoleByRoleNameAsync(string groupRoleName);
        Task<GroupRole> DeleteGroupRoleByRoleNameAsync(string groupRoleName);
    }
}



using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.GroupRoleRepository
{
    public interface IGroupRoleRepository : ICrud<GroupRole>
    {
        Task<GroupRole> GetGroupRoleByRoleNameAsync(string groupRoleName);
        Task<GroupRole> DeleteGroupRoleByRoleNameAsync(string groupRoleName);
    }
}

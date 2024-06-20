

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.RoleRepository
{
    public interface IRoleRepository : ICrud<Role>
    {
        Task<Role> GetRoleByRoleNameAsync(string RoleName);
        Task<Role> DeleteRoleByRoleNameAsync(string RoleName);
    }
}

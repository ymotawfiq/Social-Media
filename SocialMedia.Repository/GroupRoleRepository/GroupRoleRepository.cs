


using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupRoleRepository
{
    public class GroupRoleRepository : IGroupRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupRoleRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<GroupRole> AddGroupRoleAsync(GroupRole groupRole)
        {
            try
            {
                groupRole.RoleName = groupRole.RoleName.ToUpper();
                await _dbContext.GroupRoles.AddAsync(groupRole);
                await SaveChangesAsync();
                return groupRole;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupRole> DeleteGroupRoleByIdAsync(string groupRoleId)
        {
            try
            {
                var groupRole = await GetGroupRoleByIdAsync(groupRoleId);
                _dbContext.GroupRoles.Remove(groupRole);
                await SaveChangesAsync();
                return groupRole;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupRole> DeleteGroupRoleByRoleNameAsync(string groupRoleName)
        {
            try
            {
                groupRoleName = groupRoleName.ToUpper();
                var groupRole = await GetGroupRoleByRoleNameAsync(groupRoleName);
                _dbContext.GroupRoles.Remove(groupRole);
                await SaveChangesAsync();
                return groupRole;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupRole> GetGroupRoleByIdAsync(string groupRoleId)
        {
            try
            {
                return (await _dbContext.GroupRoles.Where(e => e.Id == groupRoleId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupRole> GetGroupRoleByRoleNameAsync(string groupRoleName)
        {
            try
            {
                groupRoleName = groupRoleName.ToUpper();
                return (await _dbContext.GroupRoles.Where(e => e.RoleName == groupRoleName)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GroupRole>> GetGroupRolesAsync()
        {
            try
            {
                return await _dbContext.GroupRoles.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<GroupRole> UpdateGroupRoleAsync(GroupRole groupRole)
        {
            try
            {
                groupRole.RoleName = groupRole.RoleName.ToUpper();
                var existGroupRole = await GetGroupRoleByIdAsync(groupRole.Id);
                existGroupRole.RoleName = groupRole.RoleName;
                await SaveChangesAsync();
                return existGroupRole;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}




using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.GroupRoleRepository
{
    public class GroupRoleRepository : IGroupRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupRoleRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<GroupRole> AddAsync(GroupRole t)
        {
            try
            {
                t.RoleName = t.RoleName.ToUpper();
                await _dbContext.GroupRoles.AddAsync(t);
                await SaveChangesAsync();
                return new GroupRole
                {
                    Id = t.Id,
                    RoleName = t.RoleName
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<GroupRole> DeleteByIdAsync(string id)
        {
            try
            {
                var groupRole = await GetByIdAsync(id);
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

        public async Task<IEnumerable<GroupRole>> GetAllAsync()
        {
            try
            {
                return await _dbContext.GroupRoles.Select(e => new GroupRole
                {
                    RoleName = e.RoleName,
                    Id = e.Id
                }).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupRole> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.GroupRoles.Select(e => new GroupRole
                {
                    RoleName = e.RoleName,
                    Id = e.Id
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
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
                return (await _dbContext.GroupRoles.Select(e=>new GroupRole
                {
                    RoleName = e.RoleName,
                    Id = e.Id
                }).Where(e => e.RoleName == groupRoleName)
                    .FirstOrDefaultAsync())!;
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

        public async Task<GroupRole> UpdateAsync(GroupRole t)
        {
            try
            {
                t.RoleName = t.RoleName.ToUpper();
                var existGroupRole = await GetByIdAsync(t.Id);
                existGroupRole.RoleName = t.RoleName;
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

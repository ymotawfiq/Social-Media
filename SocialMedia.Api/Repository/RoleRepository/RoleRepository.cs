


using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoleRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Role> AddAsync(Role t)
        {
            try
            {
                t.RoleName = t.RoleName.ToUpper();
                await _dbContext.Role.AddAsync(t);
                await SaveChangesAsync();
                return new Role
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


        public async Task<Role> DeleteByIdAsync(string id)
        {
            try
            {
                var groupRole = await GetByIdAsync(id);
                _dbContext.Role.Remove(groupRole);
                await SaveChangesAsync();
                return groupRole;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Role> DeleteRoleByRoleNameAsync(string RoleName)
        {
            try
            {
                RoleName = RoleName.ToUpper();
                var groupRole = await GetRoleByRoleNameAsync(RoleName);
                _dbContext.Role.Remove(groupRole);
                await SaveChangesAsync();
                return groupRole;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Role.Select(e => new Role
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

        public async Task<Role> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.Role.Select(e => new Role
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


        public async Task<Role> GetRoleByRoleNameAsync(string RoleName)
        {
            try
            {
                RoleName = RoleName.ToUpper();
                return (await _dbContext.Role.Select(e=>new Role
                {
                    RoleName = e.RoleName,
                    Id = e.Id
                }).Where(e => e.RoleName == RoleName)
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

        public async Task<Role> UpdateAsync(Role t)
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

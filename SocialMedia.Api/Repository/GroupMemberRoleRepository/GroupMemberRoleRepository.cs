

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.GroupMemberRoleRepository
{
    public class GroupMemberRoleRepository : IGroupMemberRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupMemberRoleRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<GroupMemberRole> AddAsync(GroupMemberRole t)
        {
            try
            {
                await _dbContext.GroupMemberRoles.AddAsync(t);
                await SaveChangesAsync();
                return new GroupMemberRole
                {
                    Id = t.Id,
                    GroupMemberId = t.GroupMemberId,
                    RoleId = t.RoleId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<GroupMemberRole> DeleteByIdAsync(string id)
        {
            try
            {
                var groupMember = await GetByIdAsync(id);
                _dbContext.GroupMemberRoles.Remove(groupMember);
                await SaveChangesAsync();
                return groupMember;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupMemberRole> DeleteGroupMemberRoleAsync(string groupMemberId, string roleId)
        {
            try
            {
                var groupMember = await GetGroupMemberRoleAsync(groupMemberId, roleId);
                _dbContext.GroupMemberRoles.Remove(groupMember);
                await SaveChangesAsync();
                return groupMember;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<GroupMemberRole>> GetAllAsync()
        {
            return await _dbContext.GroupMemberRoles.Select(e=>new GroupMemberRole
            {
                Id = e.Id,
                GroupMemberId = e.GroupMemberId,
                RoleId = e.RoleId
            }).ToListAsync();
        }

        public async Task<GroupMemberRole> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.GroupMemberRoles.Select(e => new GroupMemberRole
                {
                    RoleId = e.RoleId,
                    Id = e.Id,
                    GroupMemberId = e.GroupMemberId
                }).Where(e => e.Id == id)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupMemberRole> GetGroupMemberRoleAsync(string groupMemberId, string roleId)
        {
            try
            {
                return (await _dbContext.GroupMemberRoles.Select(e=>new GroupMemberRole
                {
                    RoleId = e.RoleId,
                    Id = e.Id,
                    GroupMemberId = e.GroupMemberId
                }).Where(e => e.RoleId == roleId)
                    .Where(e=>e.GroupMemberId==groupMemberId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GroupMemberRole>> GetMemberRolesAsync(string groupMemberId)
        {
            try
            {
                return from r in await _dbContext.GroupMemberRoles.ToListAsync()
                       where r.GroupMemberId == groupMemberId
                       select (new GroupMemberRole
                       {
                           GroupMemberId = r.GroupMemberId,
                           Id = r.Id,
                           RoleId = r.RoleId
                       });
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

        public async Task<GroupMemberRole> UpdateAsync(GroupMemberRole t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}



using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupMemberRoleRepository
{
    public class GroupMemberRoleRepository : IGroupMemberRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupMemberRoleRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<GroupMemberRole> AddGroupMemberRoleAsync(GroupMemberRole groupMemberRole)
        {
            try
            {
                await _dbContext.GroupMemberRoles.AddAsync(groupMemberRole);
                await SaveChangesAsync();
                return new GroupMemberRole
                {
                    Id = groupMemberRole.Id,
                    GroupMemberId = groupMemberRole.GroupMemberId,
                    RoleId = groupMemberRole.RoleId
                };
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

        public async Task<GroupMemberRole> DeleteGroupMemberRoleAsync(string groupMemberRoleId)
        {
            try
            {
                var groupMember = await GetGroupMemberRoleAsync(groupMemberRoleId);
                _dbContext.GroupMemberRoles.Remove(groupMember);
                await SaveChangesAsync();
                return groupMember;
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

        public async Task<GroupMemberRole> GetGroupMemberRoleAsync(string groupMemberRoleId)
        {
            try
            {
                return (await _dbContext.GroupMemberRoles.Select(e => new GroupMemberRole
                {
                    RoleId = e.RoleId,
                    Id = e.Id,
                    GroupMemberId = e.GroupMemberId
                }).Where(e => e.Id == groupMemberRoleId)
                    .FirstOrDefaultAsync())!;
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

    }
}

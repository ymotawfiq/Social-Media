
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Repository.GroupRoleRepository;

namespace SocialMedia.Repository.GroupRepository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IGroupRoleRepository _groupRoleRepository;
        public GroupRepository(ApplicationDbContext _dbContext, IGroupRoleRepository _groupRoleRepository)
        {
            this._dbContext = _dbContext;
            this._groupRoleRepository = _groupRoleRepository;
        }
        public async Task<Group> AddGroupAsync(Group group)
        {
            try
            {
                await _dbContext.Groups.AddAsync(group);
                await SaveChangesAsync();
                return new Group
                {
                    Id = group.Id,
                    CreatedAt = group.CreatedAt,
                    CreatedUserId = group.CreatedUserId,
                    Description = group.Description,
                    GroupPolicyId = group.GroupPolicyId,
                    Name = group.Name
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Group> DeleteGroupByIdAsync(string groupId)
        {
            try
            {
                var group = await GetGroupByIdAsync(groupId);
                _dbContext.Groups.Remove(group);
                await SaveChangesAsync();
                return group;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            try
            {
                return await _dbContext.Groups.Select(e=>new Group
                {
                    Id = e.Id,
                    CreatedAt = e.CreatedAt,
                    CreatedUserId = e.CreatedUserId,
                    Description = e.Description,
                    GroupPolicyId = e.GroupPolicyId,
                    Name = e.Name
                }).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Group>> GetAllGroupsByUserIdAsync(string userId)
        {
            try
            {
                return from g in await GetAllGroupsAsync()
                       where g.CreatedUserId == userId
                       select g;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Group> GetGroupByIdAsync(string groupId)
        {
            try
            {
                return (await _dbContext.Groups.Select(e => new Group
                {
                    Id = e.Id,
                    CreatedAt = e.CreatedAt,
                    CreatedUserId = e.CreatedUserId,
                    Description = e.Description,
                    GroupPolicyId = e.GroupPolicyId,
                    Name = e.Name
                }).Where(e => e.Id == groupId).FirstOrDefaultAsync())!;
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

        public async Task<Group> UpdateGroupAsync(Group group)
        {
            try
            {
                var existGroup = await GetGroupByIdAsync(group.Id);
                existGroup.Name = group.Name;
                existGroup.Description = group.Description;
                existGroup.GroupPolicyId = group.GroupPolicyId;
                await SaveChangesAsync();
                return existGroup;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

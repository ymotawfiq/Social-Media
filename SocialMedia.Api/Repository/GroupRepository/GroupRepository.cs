
using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GroupRoleRepository;

namespace SocialMedia.Api.Repository.GroupRepository
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

        public async Task<Group> AddAsync(Group t)
        {
            try
            {
                await _dbContext.Groups.AddAsync(t);
                await SaveChangesAsync();
                return new Group
                {
                    Id = t.Id,
                    CreatedAt = t.CreatedAt,
                    CreatedUserId = t.CreatedUserId,
                    Description = t.Description,
                    GroupPolicyId = t.GroupPolicyId,
                    Name = t.Name
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Group> DeleteByIdAsync(string id)
        {
            try
            {
                var group = await GetByIdAsync(id);
                _dbContext.Groups.Remove(group);
                await SaveChangesAsync();
                return group;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Groups.Select(e => new Group
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
                return from g in await GetAllAsync()
                       where g.CreatedUserId == userId
                       select g;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Group> GetByIdAsync(string id)
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
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
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

        public async Task<Group> UpdateAsync(Group t)
        {
            try
            {
                var existGroup = await GetByIdAsync(t.Id);
                existGroup.Name = t.Name;
                existGroup.Description = t.Description;
                existGroup.GroupPolicyId = t.GroupPolicyId;
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

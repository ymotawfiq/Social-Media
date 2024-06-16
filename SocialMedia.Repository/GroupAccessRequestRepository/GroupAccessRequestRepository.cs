

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupAccessRequestRepository
{
    public class GroupAccessRequestRepository : IGroupAccessRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupAccessRequestRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<GroupAccessRequest> AddAsync(GroupAccessRequest t)
        {
            try
            {
                await _dbContext.GroupAccessRequests.AddAsync(t);
                await SaveChangesAsync();
                return new GroupAccessRequest
                {
                    UserId = t.UserId,
                    Id = t.Id,
                    GroupId = t.GroupId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<GroupAccessRequest> DeleteByIdAsync(string id)
        {
            try
            {
                var request = await GetByIdAsync(id);
                _dbContext.GroupAccessRequests.Remove(request);
                await SaveChangesAsync();
                return request;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupAccessRequest> DeleteGroupAccessRequestAsync(string groupId, string userId)
        {
            var request = await GetGroupAccessRequestAsync(groupId, userId);
            _dbContext.GroupAccessRequests.Remove(request);
            await SaveChangesAsync();
            return request;
        }


        public async Task<IEnumerable<GroupAccessRequest>> GetAllAsync()
        {
            return await _dbContext.GroupAccessRequests.Select(e=>new GroupAccessRequest
            {
                Id = e.Id,
                UserId = e.UserId,
                GroupId = e.GroupId
            }).ToListAsync();
        }

        public async Task<GroupAccessRequest> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.GroupAccessRequests.Select(e => new GroupAccessRequest
                {
                    GroupId = e.GroupId,
                    UserId = e.UserId,
                    Id = e.Id
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupAccessRequest> GetGroupAccessRequestAsync(string groupId, string userId)
        {
            try
            {
                return (await _dbContext.GroupAccessRequests.Select(e => new GroupAccessRequest
                {
                    GroupId = e.GroupId,
                    UserId = e.UserId,
                    Id = e.Id
                }).Where(e => e.GroupId == groupId)
                    .Where(e=>e.UserId==userId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GroupAccessRequest>> GetGroupAccessRequestsByGroupIdAsync(string groupId)
        {
            try
            {
                return from g in await GetAllAsync()
                       where g.GroupId == groupId
                       select (new GroupAccessRequest
                       {
                           UserId = g.UserId,
                           GroupId = g.GroupId,
                           Id = g.Id
                       });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GroupAccessRequest>> GetGroupAccessRequestsByUserIdAsync(string userId)
        {
            try
            {
                return from g in await GetAllAsync()
                       where g.UserId == userId
                       select (new GroupAccessRequest
                       {
                           UserId = g.UserId,
                           GroupId = g.GroupId,
                           Id = g.Id
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

        public async Task<GroupAccessRequest> UpdateAsync(GroupAccessRequest t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}

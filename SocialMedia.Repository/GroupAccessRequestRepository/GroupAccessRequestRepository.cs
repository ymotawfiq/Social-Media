

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
        public async Task<GroupAccessRequest> AddGroupAccessRequestAsync(GroupAccessRequest request)
        {
            try
            {
                await _dbContext.GroupAccessRequests.AddAsync(request);
                await SaveChangesAsync();
                return new GroupAccessRequest
                {
                    UserId = request.UserId,
                    Id = request.Id,
                    GroupId = request.GroupId
                };
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

        public async Task<GroupAccessRequest> DeleteGroupAccessRequestByIdAsync(string groupAccessRequestId)
        {
            try
            {
                var request = await GetGroupAccessRequestByIdAsync(groupAccessRequestId);
                _dbContext.GroupAccessRequests.Remove(request);
                await SaveChangesAsync();
                return request;
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

        public async Task<GroupAccessRequest> GetGroupAccessRequestByIdAsync(string groupAccessRequestId)
        {
            try
            {
                return (await _dbContext.GroupAccessRequests.Select(e => new GroupAccessRequest
                {
                    GroupId = e.GroupId,
                    UserId = e.UserId,
                    Id = e.Id
                }).Where(e => e.Id == groupAccessRequestId).FirstOrDefaultAsync())!;
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
                return from g in await _dbContext.GroupAccessRequests.ToListAsync()
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
                return from g in await _dbContext.GroupAccessRequests.ToListAsync()
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
    }
}



using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.FriendRequestRepository
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FriendRequestRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<FriendRequest> AddFriendRequestAsync(FriendRequest friendRequest)
        {
            try
            {
                await _dbContext.FriendRequests.AddAsync(friendRequest);
                await SaveChangesAsync();
                return new FriendRequest
                {
                    Id = friendRequest.Id,
                    IsAccepted = friendRequest.IsAccepted,
                    UserWhoReceivedId = friendRequest.UserWhoReceivedId,
                    UserWhoSendId = friendRequest.UserWhoSendId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendRequest> DeleteFriendRequestByAsync(string friendRequestId)
        {
            try
            {
                var friendRequest = await GetFriendRequestByIdAsync(friendRequestId);
                _dbContext.Remove(friendRequest);
                await SaveChangesAsync();
                friendRequest.User = null;
                return friendRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FriendRequest>> GetAllFriendRequestsAsync()
        {
            try
            {
                return await _dbContext.FriendRequests.Select(e=>new FriendRequest
                {
                    UserWhoSendId = e.UserWhoSendId,
                    UserWhoReceivedId = e.UserWhoReceivedId,
                    IsAccepted = e.IsAccepted,
                    Id = e.Id
                }).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendRequest> GetFriendRequestByIdAsync(string friendRequestId)
        {
            try
            {
                return (await _dbContext.FriendRequests.Select(e => new FriendRequest
                {
                    UserWhoSendId = e.UserWhoSendId,
                    UserWhoReceivedId = e.UserWhoReceivedId,
                    IsAccepted = e.IsAccepted,
                    Id = e.Id
                }).Where(e => e.Id == friendRequestId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendRequest> GetFriendRequestByUserAndPersonIdAsync(string userId, string personId)
        {
            var user1 = (await _dbContext.FriendRequests.Select(e => new FriendRequest
            {
                UserWhoSendId = e.UserWhoSendId,
                UserWhoReceivedId = e.UserWhoReceivedId,
                IsAccepted = e.IsAccepted,
                Id = e.Id
            }).Where(e => e.UserWhoSendId == userId).Where(e => e.UserWhoReceivedId == personId)
            .FirstOrDefaultAsync())!;
            var user2 = (await _dbContext.FriendRequests.Select(e => new FriendRequest
            {
                UserWhoSendId = e.UserWhoSendId,
                UserWhoReceivedId = e.UserWhoReceivedId,
                IsAccepted = e.IsAccepted,
                Id = e.Id
            }).Where(e => e.UserWhoSendId == personId).Where(e => e.UserWhoReceivedId == userId)
            .FirstOrDefaultAsync())!;
            return user1 == null ? user2! : user1;
        }

        public async Task<IEnumerable<FriendRequest>> GetReceivedFriendRequestsByUserIdAsync(string userId)
        {
            try
            {
                return
                    from u in await GetAllFriendRequestsAsync()
                    where u.UserWhoReceivedId == userId
                    select u;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FriendRequest>> GetSentFriendRequestsByUserIdAsync(string userId)
        {
            try
            {
                return
                    from u in await GetAllFriendRequestsAsync()
                    where u.UserWhoSendId == userId
                    select u;
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

        public async Task<FriendRequest> UpdateFriendRequestAsync(FriendRequest friendRequest)
        {
            try
            {
                var friendRequest1 = await GetFriendRequestByIdAsync(friendRequest.Id);
                friendRequest1.IsAccepted = friendRequest.IsAccepted;
                await SaveChangesAsync();
                friendRequest1.User = null;
                return friendRequest1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

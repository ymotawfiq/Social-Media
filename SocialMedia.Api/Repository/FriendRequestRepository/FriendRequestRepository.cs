

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.FriendRequestRepository
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FriendRequestRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<FriendRequest> AddAsync(FriendRequest t)
        {
            try
            {
                await _dbContext.FriendRequests.AddAsync(t);
                await SaveChangesAsync();
                return new FriendRequest
                {
                    Id = t.Id,
                    IsAccepted = t.IsAccepted,
                    UserWhoReceivedId = t.UserWhoReceivedId,
                    UserWhoSendId = t.UserWhoSendId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<FriendRequest> DeleteByIdAsync(string id)
        {
            try
            {
                var friendRequest = await GetByIdAsync(id);
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


        public async Task<IEnumerable<FriendRequest>> GetAllAsync()
        {
            try
            {
                return await _dbContext.FriendRequests.Select(e => new FriendRequest
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


        public async Task<FriendRequest> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.FriendRequests.Select(e => new FriendRequest
                {
                    UserWhoSendId = e.UserWhoSendId,
                    UserWhoReceivedId = e.UserWhoReceivedId,
                    IsAccepted = e.IsAccepted,
                    Id = e.Id
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<FriendRequest> GetByUserAndPersonIdAsync(string userId, string personId)
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
                    from u in await GetAllAsync()
                    where u.UserWhoReceivedId == userId
                    select (new FriendRequest
                    {
                        Id = u.Id,
                        IsAccepted = u.IsAccepted,
                        UserWhoReceivedId = u.UserWhoReceivedId,
                        UserWhoSendId = u.UserWhoSendId
                    });
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
                    from u in await GetAllAsync()
                    where u.UserWhoSendId == userId
                    select (new FriendRequest
                    {
                        Id = u.Id,
                        IsAccepted = u.IsAccepted,
                        UserWhoReceivedId = u.UserWhoReceivedId,
                        UserWhoSendId = u.UserWhoSendId
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

        public async Task<FriendRequest> UpdateAsync(FriendRequest t)
        {
            try
            {
                var friendRequest1 = await GetByIdAsync(t.Id);
                friendRequest1.IsAccepted = t.IsAccepted;
                await SaveChangesAsync();
                return new FriendRequest
                {
                    UserWhoSendId = friendRequest1.UserWhoSendId,
                    UserWhoReceivedId = friendRequest1.UserWhoReceivedId,
                    IsAccepted = friendRequest1.IsAccepted,
                    Id = friendRequest1.Id
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}

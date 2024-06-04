

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.FriendRequestRepository
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<SiteUser> _userManager;
        public FriendRequestRepository(ApplicationDbContext _dbContext, UserManager<SiteUser> _userManager)
        {
            this._dbContext = _dbContext;
            this._userManager = _userManager;
        }
        public async Task<FriendRequest> AddFriendRequestAsync(FriendRequest friendRequest)
        {
            try
            {
                await _dbContext.FriendRequests.AddAsync(friendRequest);
                await SaveChangesAsync();
                friendRequest.User = null;
                return friendRequest;
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
                return await _dbContext.FriendRequests.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FriendRequest>> GetAllFriendRequestsByUserIdAsync(string userId)
        {
            try
            {
                return
                    from u in await GetAllFriendRequestsAsync()
                    where u.UserWhoReceivedId == userId && u.IsAccepted == false
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

        public async Task<IEnumerable<FriendRequest>> GetAllFriendRequestsByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                return await GetAllFriendRequestsByUserIdAsync(user!.Id);
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
                return await _dbContext.FriendRequests.Where(e => e.Id == friendRequestId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendRequest> GetFriendRequestByUserAndPersonIdAsync
            (string userId, string personId)
        {
            var user1 = await _dbContext.FriendRequests.Where(e => e.UserWhoSendId == userId)
                .Where(e => e.UserWhoReceivedId == personId).FirstOrDefaultAsync();
            var user2 = await _dbContext.FriendRequests.Where(e => e.UserWhoSendId == personId)
                .Where(e => e.UserWhoReceivedId == userId).FirstOrDefaultAsync();
            return user1 == null ? user2! : user1;
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

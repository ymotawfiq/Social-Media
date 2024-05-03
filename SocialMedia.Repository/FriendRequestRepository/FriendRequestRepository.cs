

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
                return friendRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendRequest> DeleteFriendRequestByAsync(Guid friendRequestId)
        {
            try
            {
                var friendRequest = await GetFriendRequestByIdAsync(friendRequestId);
                _dbContext.Remove(friendRequest);
                await SaveChangesAsync();
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
                    where u.PersonId == userId
                    select u;
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

        public async Task<FriendRequest> GetFriendRequestByIdAsync(Guid friendRequestId)
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
            return await _dbContext.FriendRequests.Where(e => e.UserId == userId)
                .Where(e => e.PersonId == personId).FirstOrDefaultAsync();
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
                return friendRequest1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

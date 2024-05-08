

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Repository.FriendRequestRepository;

namespace SocialMedia.Repository.FriendsRepository
{
    public class FriendsRepository : IFriendsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFriendRequestRepository _friendRequestRepository;
        public FriendsRepository(ApplicationDbContext _dbContext, 
            IFriendRequestRepository _friendRequestRepository)
        {
            this._dbContext = _dbContext;
            this._friendRequestRepository = _friendRequestRepository;
        }
        public async Task<Friend> AddFriendAsync(Friend friend)
        {
            await _dbContext.Friends.AddAsync(friend);
            await SaveChangesAsync();
            friend.User = null;
            return friend;
        }

        public async Task<Friend> DeleteFriendAsync(string userId, string friendId)
        {
            var friend = await GetFriendByUserAndFriendIdAsync(userId, friendId);
            _dbContext.Friends.Remove(friend);
            await SaveChangesAsync();
            friend.User = null;
            return friend;
        }

        public async Task<IEnumerable<Friend>> GetAllUserFriendsAsync(string userId)
        {
            return
                from f in await _dbContext.Friends.ToListAsync()
                where f.UserId == userId || f.FriendId==userId
                select (new Friend
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    FriendId = f.FriendId
                });
        }

        public async Task<Friend> GetFriendByUserAndFriendIdAsync(string userId, string friendId)
        {
            var friend1 = await _dbContext.Friends.Where(e => e.UserId == userId)
                .Where(e => e.FriendId == friendId).FirstOrDefaultAsync();
            var friend2 = await _dbContext.Friends.Where(e => e.UserId == friendId)
                .Where(e => e.FriendId == userId).FirstOrDefaultAsync();
            return friend1 == null ? friend2! : friend1;
        }

        public async Task<IEnumerable<IEnumerable<Friend>>> GetUserFriendsOfFriendsAsync(string userId)
        {
            var userFriends = await GetAllUserFriendsAsync(userId);
            var friendsOfFriends = new List<List<Friend>>();
            if (userFriends != null)
            {
                foreach(var f in userFriends)
                {
                    friendsOfFriends.Add((await GetAllUserFriendsAsync(f.UserId)).ToList());
                }
            }
            return friendsOfFriends;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}



using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;


namespace SocialMedia.Repository.FriendsRepository
{
    public class FriendsRepository : IFriendsRepository
    {

        private readonly ApplicationDbContext _dbContext;
        public FriendsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Friend> DeleteFriendAsync(string userId, string friendId)
        {
            var friend = await GetByUserAndFriendIdAsync(userId, friendId);
            _dbContext.Friends.Remove(friend);
            await SaveChangesAsync();
            friend.User = null;
            return friend;
        }

        public async Task<IEnumerable<Friend>> GetAllUserFriendsAsync(string userId)
        {
            return
                from f in await GetAllAsync()
                where f.UserId == userId || f.FriendId==userId
                select (new Friend
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    FriendId = f.FriendId
                });
        }

        public async Task<Friend> GetByUserAndFriendIdAsync(string userId, string friendId)
        {
            var friend1 = (await _dbContext.Friends.Select(e => new Friend
            {
                UserId = e.UserId,
                FriendId = e.FriendId,
                Id = e.Id
            }).Where(e => e.UserId == userId).Where(e => e.FriendId == friendId).FirstOrDefaultAsync())!;
            var friend2 = (await _dbContext.Friends.Select(e => new Friend
            {
                UserId = e.UserId,
                FriendId = e.FriendId,
                Id = e.Id
            }).Where(e => e.UserId == friendId).Where(e => e.FriendId == userId).FirstOrDefaultAsync())!;
            return friend1 == null ? friend2! : friend1;
        }

        public async Task<IEnumerable<Friend>> GetSharedFriendsAsync(string userId, string routeUserId)
        {
            var currentUserFriends = await GetAllUserFriendsAsync(userId);
            var routeUserFriends = await GetAllUserFriendsAsync(routeUserId);

            var sharedFrieds = from f1 in currentUserFriends
                       from f2 in routeUserFriends
                       where GetDifferntIdAsync(f1, userId) == GetDifferntIdAsync(f2, routeUserId)
                       select (new Friend
                       {
                           Id = f1.Id,
                           UserId = userId,
                           FriendId = GetDifferntIdAsync(f1, userId)
                       });
            return sharedFrieds;
        }

        private string GetDifferntIdAsync(Friend friend, string userId)
        {
            if (friend.FriendId == userId)
            {
                return friend.UserId;
            }
            return friend.FriendId;
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

        public async Task<Friend> AddAsync(Friend t)
        {
            await _dbContext.Friends.AddAsync(t);
            await SaveChangesAsync();
            return new Friend
            {
                Id = t.Id,
                FriendId = t.FriendId,
                UserId = t.UserId
            };
        }

        public async Task<Friend> UpdateAsync(Friend t)
        {
            return await DeleteByIdAsync(t.Id);
        }

        public async Task<Friend> DeleteByIdAsync(string id)
        {
            var friend = await GetByIdAsync(id);
            _dbContext.Friends.Remove(friend);
            await SaveChangesAsync();
            return new Friend
            {
                Id = friend.Id,
                FriendId = friend.FriendId,
                UserId = friend.UserId
            };
        }

        public async Task<Friend> GetByIdAsync(string id)
        {
            return (await _dbContext.Friends.Select(e => new Friend
            {
                FriendId = e.FriendId,
                Id = e.Id,
                UserId = e.UserId
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<Friend>> GetAllAsync()
        {
            return await _dbContext.Friends.Select(e=>new Friend
            {
                UserId = e.UserId,
                Id = e.Id,
                FriendId = e.FriendId
            }).ToListAsync();
        }
    }
}

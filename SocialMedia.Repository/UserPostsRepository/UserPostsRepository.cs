

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.UserPostsRepository
{
    public class UserPostsRepository : IUserPostsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserPostsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<UserPosts> AddUserPostAsync(UserPosts userPosts)
        {
            try
            {
                await _dbContext.UserPosts.AddAsync(userPosts);
                await SaveChangesAsync();
                return userPosts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserPosts> DeleteUserPostByIdAsync(string userPostId)
        {
            try
            {
                var userPost = await GetUserPostByIdAsync(userPostId);
                _dbContext.UserPosts.Remove(userPost);
                await SaveChangesAsync();
                return userPost;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserPosts> DeleteUserPostByUserAndPostIdAsync(string userId, string postId)
        {
            try
            {
                var userPost = await GetUserPostByUserAndPostIdAsync(userId, postId);
                _dbContext.UserPosts.Remove(userPost);
                await SaveChangesAsync();
                return userPost;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserPosts> GetUserPostByIdAsync(string userPostId)
        {
            return (await _dbContext.UserPosts.Where(e => e.Id == userPostId).FirstOrDefaultAsync())!;
        }

        public async Task<UserPosts> GetUserPostByPostIdAsync(string postId)
        {
            return (await _dbContext.UserPosts.Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
        }

        public async Task<UserPosts> GetUserPostByUserAndPostIdAsync(string userId, string postId)
        {
            return (await _dbContext.UserPosts.Where(e => e.UserId == userId)
                .Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<UserPosts>> GetUserPostsByUserIdAsync(string userId)
        {
            return from u in await _dbContext.UserPosts.Include(e=>e.Post).ToListAsync()
                   where u.UserId==userId
                   select u;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        
    }
}



using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupPostsRepository
{
    public class GroupPostsRepository : IGroupPostsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupPostsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<GroupPost> AddGroupPostAsync(GroupPost groupPost)
        {
            try
            {
                await _dbContext.GroupPosts.AddAsync(groupPost);
                await SaveChangesAsync();
                return new GroupPost
                {
                    Id = groupPost.Id,
                    GroupId = groupPost.GroupId,
                    PostId = groupPost.PostId,
                    UserId = groupPost.UserId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupPost> DeleteGroupPostByIdAsync(string groupPostId)
        {
            try
            {
                var groupPost = await GetGroupPostByIdAsync(groupPostId);
                _dbContext.GroupPosts.Remove(groupPost);
                await SaveChangesAsync();
                return groupPost;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupPost> GetGroupPostByIdAsync(string groupPostId)
        {
            try
            {
                return (await _dbContext.GroupPosts.Select(e=>new GroupPost
                {
                    UserId = e.UserId,
                    PostId = e.PostId,
                    GroupId = e.GroupId,
                    Id = e.Id
                }).Where(e => e.Id == groupPostId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupPost> GetGroupPostByPostIdAsync(string postId)
        {
            return (await _dbContext.GroupPosts.Select(e => new GroupPost
            {
                UserId = e.UserId,
                PostId = e.PostId,
                GroupId = e.GroupId,
                Id = e.Id
            }).Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<GroupPost>> GetGroupPostsAsync(string groupId)
        {
            try
            {
                return from p in await _dbContext.GroupPosts.ToListAsync()
                       where p.GroupId == groupId
                       select (new GroupPost
                       {
                           Id = p.Id,
                           GroupId = p.GroupId,
                           PostId = p.PostId,
                           UserId = p.UserId
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

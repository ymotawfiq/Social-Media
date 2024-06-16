

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

        public async Task<GroupPost> AddAsync(GroupPost t)
        {
            try
            {
                await _dbContext.GroupPosts.AddAsync(t);
                await SaveChangesAsync();
                return new GroupPost
                {
                    Id = t.Id,
                    GroupId = t.GroupId,
                    PostId = t.PostId,
                    UserId = t.UserId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupPost> DeleteByIdAsync(string id)
        {
            try
            {
                var groupPost = await GetByIdAsync(id);
                _dbContext.GroupPosts.Remove(groupPost);
                await SaveChangesAsync();
                return groupPost;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<GroupPost>> GetAllAsync()
        {
            return await _dbContext.GroupPosts.Select(e=>new GroupPost
            {
                Id = e.Id,
                GroupId = e.GroupId,
                PostId = e.PostId,
                UserId = e.UserId
            }).ToListAsync();
        }

        public async Task<GroupPost> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.GroupPosts.Select(e => new GroupPost
                {
                    UserId = e.UserId,
                    PostId = e.PostId,
                    GroupId = e.GroupId,
                    Id = e.Id
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
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
                return from p in await GetAllAsync()
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

        public async Task<GroupPost> UpdateAsync(GroupPost t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}

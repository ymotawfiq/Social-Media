

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.PostCommentsRepository
{
    public class PostCommentsRepository : IPostCommentsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostCommentsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<PostComment> AddAsync(PostComment t)
        {
            try
            {
                await _dbContext.PostComments.AddAsync(t);
                await SaveChangesAsync();
                return new PostComment
                {
                    UserId = t.UserId,
                    Comment = t.Comment,
                    Id = t.Id,
                    CommentImage = t.CommentImage,
                    PostId = t.PostId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<PostComment> DeleteByIdAsync(string id)
        {
            try
            {
                var postComment = await GetByIdAsync(id);
                _dbContext.PostComments.Remove(postComment);
                await SaveChangesAsync();
                return postComment;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<PostComment> DeletePostCommentByPostIdAndUserIdAsync(string postId, string userId)
        {
            try
            {
                var postComment = await GetPostCommentByPostIdAndUserIdAsync(postId, userId);
                _dbContext.PostComments.Remove(postComment);
                await SaveChangesAsync();
                return postComment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostComment> DeletePostCommentImageAsync(string postId, string userId)
        {
            try
            {
                var postComment = await GetPostCommentByPostIdAndUserIdAsync(postId, userId);
                postComment.CommentImage = null;
                await SaveChangesAsync();
                return postComment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostComment> DeletePostCommentImageAsync(string postCommentId)
        {
            try
            {
                var postComment = await GetByIdAsync(postCommentId);
                postComment.CommentImage = null;
                await SaveChangesAsync();
                return postComment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostComment>> GetAllAsync()
        {
            return await _dbContext.PostComments.Select(e=>new PostComment
            {
                Id = e.Id,
                Comment = e.Comment,
                CommentId = e.CommentId,
                CommentImage = e.CommentImage,
                PostId = e.PostId,
                UserId = e.UserId
            }).ToListAsync();
        }

        public async Task<PostComment> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.PostComments.Select(e => new PostComment
                {
                    UserId = e.UserId,
                    Comment = e.Comment,
                    Id = e.Id,
                    CommentImage = e.CommentImage,
                    PostId = e.PostId
                }).Where(e => e.Id == id)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<PostComment> GetPostCommentByPostIdAndUserIdAsync(string postId, string userId)
        {
            try
            {
                return (await _dbContext.PostComments.Select(e => new PostComment
                {
                    UserId = e.UserId,
                    Comment = e.Comment,
                    Id = e.Id,
                    CommentImage = e.CommentImage,
                    PostId = e.PostId
                }).Where(e => e.PostId == postId).Where(e => e.UserId == userId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostComment>> GetPostCommentsByPostIdAndUserIdAsync(
            string postId, string userId)
        {
            try
            {
                return await _dbContext.PostComments.Select(e => new PostComment
                {
                    UserId = e.UserId,
                    Comment = e.Comment,
                    Id = e.Id,
                    CommentImage = e.CommentImage,
                    PostId = e.PostId
                }).Where(e => e.PostId == postId).Where(e => e.UserId == userId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostComment>> GetPostCommentsByPostIdAsync(string postId)
        {
            try
            {
                return from p in await _dbContext.PostComments.ToListAsync()
                       where p.PostId == postId
                       select (new PostComment
                       {
                           PostId = p.PostId,
                           Comment = p.Comment,
                           CommentImage = p.CommentImage,
                           Id = p.Id,
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

        public async Task<PostComment> UpdateAsync(PostComment t)
        {
            try
            {
                var postComment1 = await GetPostCommentByPostIdAndUserIdAsync(t.PostId,
                    t.UserId);
                postComment1.Comment = t.Comment;
                postComment1.CommentImage = t.CommentImage;
                await SaveChangesAsync();
                return postComment1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostComment> UpdatePostCommentAsync(PostComment postComments)
        {
            try
            {
                var postComment1 = await GetPostCommentByPostIdAndUserIdAsync(postComments.PostId,
                    postComments.UserId);
                postComment1.Comment = postComments.Comment;
                postComment1.CommentImage = postComments.CommentImage;
                await SaveChangesAsync();
                return postComment1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

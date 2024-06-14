

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostCommentsRepository
{
    public class PostCommentsRepository : IPostCommentsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostCommentsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<PostComment> AddPostCommentAsync(PostComment postComments)
        {
            try
            {
                await _dbContext.PostComments.AddAsync(postComments);
                await SaveChangesAsync();
                return new PostComment
                {
                    UserId = postComments.UserId,
                    Comment = postComments.Comment,
                    Id = postComments.Id,
                    CommentImage = postComments.CommentImage,
                    PostId = postComments.PostId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostComment> DeletePostCommentByIdAsync(string postCommentId)
        {
            try
            {
                var postComment = await GetPostCommentByIdAsync(postCommentId);
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
                var postComment = await GetPostCommentByIdAsync(postCommentId);
                postComment.CommentImage = null;
                await SaveChangesAsync();
                return postComment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostComment> GetPostCommentByIdAsync(string postCommentId)
        {
            try
            {
                return (await _dbContext.PostComments.Select(e=>new PostComment
                {
                    UserId = e.UserId,
                    Comment = e.Comment,
                    Id = e.Id,
                    CommentImage = e.CommentImage,
                    PostId = e.PostId
                }).Where(e => e.Id == postCommentId)
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

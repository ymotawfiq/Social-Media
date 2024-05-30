

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
        public async Task<PostComments> AddPostCommentAsync(PostComments postComments)
        {
            try
            {
                await _dbContext.PostComments.AddAsync(postComments);
                await SaveChangesAsync();
                return postComments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostComments> DeletePostCommentByIdAsync(string postCommentId)
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

        public async Task<PostComments> DeletePostCommentByPostIdAndUserIdAsync(string postId, string userId)
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

        public async Task<PostComments> DeletePostCommentImageAsync(string postId, string userId)
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

        public async Task<PostComments> DeletePostCommentImageAsync(string postCommentId)
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

        public async Task<PostComments> GetPostCommentByIdAsync(string postCommentId)
        {
            try
            {
                return (await _dbContext.PostComments.Where(e => e.Id == postCommentId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostComments> GetPostCommentByPostIdAndUserIdAsync(string postId, string userId)
        {
            try
            {
                return (await _dbContext.PostComments.Where(e => e.PostId == postId)
                    .Where(e => e.UserId == userId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostComments>> GetPostCommentsByPostIdAsync(string postId)
        {
            try
            {
                return from p in await _dbContext.PostComments.ToListAsync()
                       where p.PostId == postId
                       select p;
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

        public async Task<PostComments> UpdatePostCommentAsync(PostComments postComments)
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

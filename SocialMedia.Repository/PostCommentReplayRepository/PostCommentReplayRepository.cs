

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostCommentReplayRepository
{
    public class PostCommentReplayRepository : IPostCommentReplayRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostCommentReplayRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<PostCommentReplay> AddPostCommentReplayAsync(PostCommentReplay postCommentReplay)
        {
            try
            {
                await _dbContext.PostCommentReplay.AddAsync(postCommentReplay);
                await SaveChangesAsync();
                return postCommentReplay;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostCommentReplay> DeletePostCommentReplayByIdAsync(string postCommentReplayId)
        {
            try
            {
                var commentReplay = await GetPostCommentReplayByIdAsync(postCommentReplayId);
                _dbContext.PostCommentReplay.Remove(commentReplay);
                await SaveChangesAsync();
                return commentReplay;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostCommentReplay> GetPostCommentReplayByIdAsync(string postCommentReplayId)
        {
            try
            {
                return (await _dbContext.PostCommentReplay.Where(e => e.Id == postCommentReplayId)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostCommentReplay>> GetPostCommentReplaysAsync(string postCommentId)
        {
            try
            {
                return from c in await _dbContext.PostCommentReplay.ToListAsync()
                       where c.PostCommentId == postCommentId
                       select c;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostCommentReplay>> GetReplaysOfReplayAsync(string replayId)
        {
            try
            {
                return from c in await _dbContext.PostCommentReplay.ToListAsync()
                       where c.PostCommentReplayId == replayId
                       select c;
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

        public async Task<PostCommentReplay> UpdatePostCommentReplayAsync(PostCommentReplay postCommentReplay)
        {
            try
            {
                var commentReplay = await GetPostCommentReplayByIdAsync(postCommentReplay.Id);
                commentReplay.Replay = postCommentReplay.Replay;
                commentReplay.ReplayImage = postCommentReplay.ReplayImage;
                await SaveChangesAsync();
                return commentReplay;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

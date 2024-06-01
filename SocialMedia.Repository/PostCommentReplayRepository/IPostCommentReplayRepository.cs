

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostCommentReplayRepository
{
    public interface IPostCommentReplayRepository
    {
        Task<PostCommentReplay> AddPostCommentReplayAsync(PostCommentReplay postCommentReplay);
        Task<PostCommentReplay> UpdatePostCommentReplayAsync(PostCommentReplay postCommentReplay);
        Task<PostCommentReplay> DeletePostCommentReplayByIdAsync(string postCommentReplayId);
        Task<PostCommentReplay> GetPostCommentReplayByIdAsync(string postCommentReplayId);
        Task<IEnumerable<PostCommentReplay>> GetPostCommentReplaysAsync(string postCommentId);
        Task<IEnumerable<PostCommentReplay>> GetReplaysOfReplayAsync(string replayId);
        Task SaveChangesAsync();
    }
}

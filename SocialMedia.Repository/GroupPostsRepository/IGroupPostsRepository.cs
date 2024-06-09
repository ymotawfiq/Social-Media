

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.GroupPostsRepository
{
    public interface IGroupPostsRepository
    {
        Task<GroupPost> AddGroupPostAsync(GroupPost groupPost);
        Task<GroupPost> DeleteGroupPostByIdAsync(string groupPostId);
        Task<GroupPost> GetGroupPostByIdAsync(string groupPostId);
        Task<GroupPost> GetGroupPostByPostIdAsync(string postId);
        Task<IEnumerable<GroupPost>> GetGroupPostsAsync(string groupId);
        Task SaveChangesAsync();
    }
}

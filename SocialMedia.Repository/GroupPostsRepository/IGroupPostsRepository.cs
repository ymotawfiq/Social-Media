

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.GroupPostsRepository
{
    public interface IGroupPostsRepository : ICrud<GroupPost>
    {
        Task<GroupPost> GetGroupPostByPostIdAsync(string postId);
        Task<IEnumerable<GroupPost>> GetGroupPostsAsync(string groupId);
    }
}



using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.GroupPostsRepository
{
    public interface IGroupPostsRepository : ICrud<GroupPost>
    {
        Task<GroupPost> GetGroupPostByPostIdAsync(string postId);
        Task<IEnumerable<GroupPost>> GetGroupPostsAsync(string groupId);
    }
}

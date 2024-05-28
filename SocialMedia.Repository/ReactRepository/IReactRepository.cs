

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.ReactRepository
{
    public interface IReactRepository
    {
        Task<React> AddReactAsync(React react);
        Task<React> UpdateReactAsync(React react);
        Task<React> DeleteReactByIdAsync(string reactId);
        Task<React> GetReactByIdAsync(string reactId);
        Task<IEnumerable<React>> GetAllReactsAsync();
        Task SaveChangesAsync();
    }
}

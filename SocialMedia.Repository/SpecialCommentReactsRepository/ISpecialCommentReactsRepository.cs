

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.SpecialCommentReactsRepository
{
    public interface ISpecialCommentReactsRepository
    {
        Task<SpecialCommentReacts> AddSpecialCommentReactsAsync(SpecialCommentReacts specialCommentReacts);
        Task<SpecialCommentReacts> UpdateSpecialCommentReactsAsync(SpecialCommentReacts specialCommentReacts);
        Task<SpecialCommentReacts> GetSpecialCommentReactsByIdAsync(string Id);
        Task<SpecialCommentReacts> GetSpecialCommentReactsByReactIdAsync(string reactId);
        Task<SpecialCommentReacts> DeleteSpecialCommentReactsByIdAsync(string Id);
        Task<SpecialCommentReacts> DeleteSpecialCommentReactsByReactIdAsync(string reactId);
        Task<IEnumerable<SpecialCommentReacts>> GetSpecialCommentReactsAsync();
        Task SaveChangesAsync();
    }
}

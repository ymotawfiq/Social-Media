

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models;

namespace SocialMedia.Service.SpecialCommentReactsService
{
    public interface ISpecialCommentReactsService
    {
        Task<ApiResponse<SpecialCommentReacts>> AddSpecialCommentReactsAsync(
            AddSpecialCommentReactsDto addSpecialCommentReactsDto);
        Task<ApiResponse<SpecialCommentReacts>> UpdateSpecialCommentReactsAsync(
            UpdateSpecialCommentReactsDto updateSpecialCommentReactsDto);
        Task<ApiResponse<SpecialCommentReacts>> GetSpecialCommentReactsByIdAsync(string Id);
        Task<ApiResponse<SpecialCommentReacts>> GetSpecialCommentReactsByReactIdAsync(string reactId);
        Task<ApiResponse<SpecialCommentReacts>> DeleteSpecialCommentReactsByIdAsync(string Id);
        Task<ApiResponse<SpecialCommentReacts>> DeleteSpecialCommentReactsByReactIdAsync(string reactId);
        Task<ApiResponse<IEnumerable<SpecialCommentReacts>>> GetSpecialCommentReactsAsync();
    }
}

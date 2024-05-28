

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.SpecialPostReactService
{
    public interface ISpecialPostReactService
    {
        Task<ApiResponse<SpecialPostReacts>> AddSpecialPostReactsAsync(
            AddSpecialPostsReactsDto addSpecialPostsReactsDto);
        Task<ApiResponse<SpecialPostReacts>> UpdateSpecialPostReactsAsync(
            UpdateSpecialPostsReactsDto updateSpecialPostsReactsDto);
        Task<ApiResponse<SpecialPostReacts>> GetSpecialPostReactsByIdAsync(string Id);
        Task<ApiResponse<SpecialPostReacts>> GetSpecialPostReactsByReactIdAsync(string reactId);
        Task<ApiResponse<SpecialPostReacts>> DeleteSpecialPostReactsByIdAsync(string Id);
        Task<ApiResponse<SpecialPostReacts>> DeleteSpecialPostReactsByReactIdAsync(string reactId);
        Task<ApiResponse<IEnumerable<SpecialPostReacts>>> GetSpecialPostReactsAsync();
    }
}

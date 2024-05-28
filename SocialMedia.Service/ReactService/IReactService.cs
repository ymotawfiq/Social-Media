
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.ReactService
{
    public interface IReactService
    {
        Task<ApiResponse<React>> AddReactAsync(ReactDto reactDto);
        Task<ApiResponse<React>> UpdateReactAsync(ReactDto reactDto);
        Task<ApiResponse<React>> DeleteReactByIdAsync(string reactId);
        Task<ApiResponse<React>> GetReactByIdAsync(string reactId);
        Task<ApiResponse<IEnumerable<React>>> GetAllReactsAsync();
    }
}

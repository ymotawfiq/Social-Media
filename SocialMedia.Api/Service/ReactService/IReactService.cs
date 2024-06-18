
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;

namespace SocialMedia.Api.Service.ReactService
{
    public interface IReactService
    {
        Task<ApiResponse<React>> AddReactAsync(AddReactDto addReactDto);
        Task<ApiResponse<React>> UpdateReactAsync(UpdateReactDto updateReactDto);
        Task<ApiResponse<React>> DeleteReactByIdAsync(string reactId);
        Task<ApiResponse<React>> DeleteReactByNameAsync(string reactName);
        Task<ApiResponse<React>> GetReactByNameAsync(string reactName);
        Task<ApiResponse<React>> GetReactByIdAsync(string reactId);
        Task<ApiResponse<IEnumerable<React>>> GetAllReactsAsync();
    }
}

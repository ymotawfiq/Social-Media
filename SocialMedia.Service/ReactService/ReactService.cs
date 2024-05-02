

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.ReactRepository;

namespace SocialMedia.Service.ReactService
{
    public class ReactService : IReactService
    {
        private readonly IReactRepository _reactRepository;
        public ReactService(IReactRepository _reactRepository)
        {
            this._reactRepository = _reactRepository;
        }
        public async Task<ApiResponse<React>> AddReactAsync(ReactDto reactDto)
        {
            if (reactDto.ReactValue == null)
            {
                return new ApiResponse<React>
                {
                    IsSuccess = false,
                    Message = "React value must not be null",
                    StatusCode = 400
                };
            }
            var newReact = await _reactRepository.AddReactAsync(
                ConvertFromDto.ConvertFromReactDto_Add(reactDto));
            return new ApiResponse<React>
            {
                StatusCode = 201,
                IsSuccess = true,
                Message = "React added successfully",
                ResponseObject = newReact
            };
        }

        public async Task<ApiResponse<React>> DeleteReactByIdAsync(Guid reactId)
        {
            var react = await _reactRepository.GetReactByIdAsync(reactId);
            if (react == null)
            {
                return new ApiResponse<React>
                {
                    IsSuccess = false,
                    Message = "React not found",
                    StatusCode = 404
                };
            }
            var deletedReact = await _reactRepository.DeleteReactByIdAsync(reactId);
            return new ApiResponse<React>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "React deleted successfully",
                ResponseObject = deletedReact
            };
        }

        public async Task<ApiResponse<IEnumerable<React>>> GetAllReactsAsync()
        {
            var reacts = await _reactRepository.GetAllReactsAsync();
            if (reacts.ToList().Count==0)
            {
                return new ApiResponse<IEnumerable<React>>
                {
                    IsSuccess = true,
                    Message = "No reacts found",
                    StatusCode = 200,
                    ResponseObject = reacts
                };
            }
            return new ApiResponse<IEnumerable<React>>
            {
                IsSuccess = true,
                Message = "Reacts found successfully",
                StatusCode = 200,
                ResponseObject = reacts
            };
        }

        public async Task<ApiResponse<React>> GetReactByIdAsync(Guid reactId)
        {
            var react = await _reactRepository.GetReactByIdAsync(reactId);
            if (react == null)
            {
                return new ApiResponse<React>
                {
                    IsSuccess = false,
                    Message = "React not found",
                    StatusCode = 404
                };
            }
            return new ApiResponse<React>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "React founded successfully",
                ResponseObject = react
            };
        }

        public async Task<ApiResponse<React>> UpdateReactAsync(ReactDto reactDto)
        {
            if (reactDto.ReactValue == null)
            {
                return new ApiResponse<React>
                {
                    IsSuccess = false,
                    Message = "React value must not be null",
                    StatusCode = 400
                };
            }
            else if(reactDto.Id == null)
            {
                return new ApiResponse<React>
                {
                    IsSuccess = false,
                    Message = "React id must not be null",
                    StatusCode = 400
                };
            }
            var updatedReact = await _reactRepository.UpdateReactAsync(
                ConvertFromDto.ConvertFromReactDto_Update(reactDto));
            return new ApiResponse<React>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "React updated successfully",
                ResponseObject = updatedReact
            };
        }
    }
}

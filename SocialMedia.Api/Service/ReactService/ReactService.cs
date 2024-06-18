

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Repository.ReactRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.ReactService
{
    public class ReactService : IReactService
    {
        private readonly IReactRepository _reactRepository;
        public ReactService(IReactRepository _reactRepository)
        {
            this._reactRepository = _reactRepository;
        }
        public async Task<ApiResponse<React>> AddReactAsync(AddReactDto addReactDto)
        {
            var existReact = await _reactRepository.GetReactByNameAsync(addReactDto.ReactValue);
            if (existReact == null)
            {
                var newReact = await _reactRepository.AddAsync(
                ConvertFromDto.ConvertFromReactDto_Add(addReactDto));
                return StatusCodeReturn<React>
                    ._201_Created("React added successfully", newReact);
            }
            return StatusCodeReturn<React>
                ._403_Forbidden("React already exists");
        }

        public async Task<ApiResponse<React>> DeleteReactByIdAsync(string reactId)
        {
            var react = await _reactRepository.GetByIdAsync(reactId);
            if (react != null)
            {
                await _reactRepository.DeleteByIdAsync(reactId);
                return StatusCodeReturn<React>
                    ._200_Success("React deleted successfully", react);
            }
            return StatusCodeReturn<React>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<React>> DeleteReactByNameAsync(string reactName)
        {
            var react = await _reactRepository.GetReactByNameAsync(reactName);
            if (react != null)
            {
                await _reactRepository.DeleteByIdAsync(react.Id);
                return StatusCodeReturn<React>
                    ._200_Success("React found successfully", react);
            }
            return StatusCodeReturn<React>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<IEnumerable<React>>> GetAllReactsAsync()
        {
            var reacts = await _reactRepository.GetAllAsync();
            if (reacts.ToList().Count==0)
            {
                return StatusCodeReturn<IEnumerable<React>>
                    ._200_Success("No reacts found", reacts);
                
            }
            return StatusCodeReturn<IEnumerable<React>>
                    ._200_Success("Reacts found successfully", reacts);
        }

        public async Task<ApiResponse<React>> GetReactByIdAsync(string reactId)
        {
            var react = await _reactRepository.GetByIdAsync(reactId);
            if (react != null)
            {
                return StatusCodeReturn<React>
                    ._200_Success("React found successfully", react);
            }
            return StatusCodeReturn<React>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<React>> GetReactByNameAsync(string reactName)
        {
            var react = await _reactRepository.GetReactByNameAsync(reactName);
            if (react != null)
            {
                return StatusCodeReturn<React>
                    ._200_Success("React found successfully", react);
            }
            return StatusCodeReturn<React>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<React>> UpdateReactAsync(UpdateReactDto updateReactDto)
        {
            var reactById = await _reactRepository.GetByIdAsync(updateReactDto.Id);
            if (reactById != null)
            {
                var reactByName = await _reactRepository.GetReactByNameAsync(updateReactDto.ReactValue);
                if (reactByName == null)
                {
                    var updatedReact = await _reactRepository.UpdateAsync(
                                    ConvertFromDto.ConvertFromReactDto_Update(updateReactDto));
                    return StatusCodeReturn<React>
                            ._200_Success("React updated successfully", updatedReact);
                }
                return StatusCodeReturn<React>
                    ._403_Forbidden("React already exists");
            }
            return StatusCodeReturn<React>
                ._404_NotFound("React not found");
        }
    }
}

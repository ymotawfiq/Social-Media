

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Service.GenericReturn;

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
                return StatusCodeReturn<React>
                    ._400_BadRequest("React value must not be null");
            }
            var newReact = await _reactRepository.AddReactAsync(
                ConvertFromDto.ConvertFromReactDto_Add(reactDto));
            return StatusCodeReturn<React>
                ._201_Created("React added successfully", newReact);
        }

        public async Task<ApiResponse<React>> DeleteReactByIdAsync(string reactId)
        {
            var react = await _reactRepository.GetReactByIdAsync(reactId);
            if (react == null)
            {
                return StatusCodeReturn<React>
                    ._404_NotFound("React not found");
            }
            var deletedReact = await _reactRepository.DeleteReactByIdAsync(reactId);
            return StatusCodeReturn<React>
                ._200_Success("React deleted successfully", deletedReact);
        }

        public async Task<ApiResponse<IEnumerable<React>>> GetAllReactsAsync()
        {
            var reacts = await _reactRepository.GetAllReactsAsync();
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
            var react = await _reactRepository.GetReactByIdAsync(reactId);
            if (react == null)
            {
                return StatusCodeReturn<React>
                    ._404_NotFound("React not found");
            }
            return StatusCodeReturn<React>
                    ._200_Success("React found successfully", react);
        }

        public async Task<ApiResponse<React>> UpdateReactAsync(ReactDto reactDto)
        {
            if (reactDto.ReactValue == null)
            {
                return StatusCodeReturn<React>
                    ._400_BadRequest("React value must not be null");
            }
            else if(reactDto.Id == null)
            {
                return StatusCodeReturn<React>
                    ._400_BadRequest("React id must not be null");
            }
            var updatedReact = await _reactRepository.UpdateReactAsync(
                ConvertFromDto.ConvertFromReactDto_Update(reactDto));
            return StatusCodeReturn<React>
                    ._200_Success("React updated successfully", updatedReact);
        }
    }
}

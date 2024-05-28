
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Repository.SpecialPostsReactsRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.SpecialPostReactService
{
    public class SpecialPostReactsService : ISpecialPostReactService
    {
        private readonly ISpecialPostsReactsRepository _specialPostsReactsRepository;
        private readonly IReactRepository _reactRepository;
        public SpecialPostReactsService(ISpecialPostsReactsRepository _specialPostsReactsRepository,
            IReactRepository _reactRepository)
        {
            this._specialPostsReactsRepository = _specialPostsReactsRepository;
            this._reactRepository = _reactRepository;
        }
        public async Task<ApiResponse<SpecialPostReacts>> AddSpecialPostReactsAsync(
            AddSpecialPostsReactsDto addSpecialPostsReactsDto)
        {
            var react = await _reactRepository.GetReactByIdAsync(addSpecialPostsReactsDto.ReactId);
            if (react != null)
            {
                var existPostReact = await _specialPostsReactsRepository.GetSpecialPostReactsByIdAsync(
                    addSpecialPostsReactsDto.ReactId);
                if (existPostReact != null)
                {
                    return StatusCodeReturn<SpecialPostReacts>
                        ._403_Forbidden("Post react already exists");
                }
                var newPostReact = await _specialPostsReactsRepository.AddSpecialPostReactsAsync(
                    ConvertFromDto.ConvertSpecialPostReactsDto_Add(addSpecialPostsReactsDto));
                return StatusCodeReturn<SpecialPostReacts>
                    ._201_Created("Post react created successfully", newPostReact);
            }
            return StatusCodeReturn<SpecialPostReacts>
                        ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<SpecialPostReacts>> DeleteSpecialPostReactsByIdAsync(string Id)
        {
            var postReact = await _specialPostsReactsRepository.GetSpecialPostReactsByIdAsync(
                Id);
            if (postReact != null)
            {
                await _specialPostsReactsRepository.DeleteSpecialPostReactsByIdAsync(Id);
                return StatusCodeReturn<SpecialPostReacts>
                    ._200_Success("Post react deleted successfully", postReact);
            }
            return StatusCodeReturn<SpecialPostReacts>
                ._404_NotFound("Post react not found");
        }

        public async Task<ApiResponse<SpecialPostReacts>> DeleteSpecialPostReactsByReactIdAsync(string reactId)
        {
            var react = await _reactRepository.GetReactByIdAsync(reactId);
            if (react != null)
            {
                var postReact = await _specialPostsReactsRepository.GetSpecialPostReactsByReactIdAsync(
                    reactId);
                if (postReact != null)
                {
                    await _specialPostsReactsRepository.DeleteSpecialPostReactsByReactIdAsync(reactId);
                    return StatusCodeReturn<SpecialPostReacts>
                        ._200_Success("Post react deleted successfully", postReact);
                }
                return StatusCodeReturn<SpecialPostReacts>
                    ._404_NotFound("Post react not found");
            }
            return StatusCodeReturn<SpecialPostReacts>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<IEnumerable<SpecialPostReacts>>> GetSpecialPostReactsAsync()
        {
            var postReacts = await _specialPostsReactsRepository.GetSpecialPostReactsAsync();
            if (postReacts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<SpecialPostReacts>>
                    ._200_Success("No post reacts found", postReacts);
            }
            return StatusCodeReturn<IEnumerable<SpecialPostReacts>>
                    ._200_Success("Post reacts found successfully", postReacts);
        }

        public async Task<ApiResponse<SpecialPostReacts>> GetSpecialPostReactsByIdAsync(string Id)
        {
            var postReact = await _specialPostsReactsRepository.GetSpecialPostReactsByIdAsync(
                Id);
            if (postReact != null)
            {
                return StatusCodeReturn<SpecialPostReacts>
                    ._200_Success("Post react found successfully", postReact);
            }
            return StatusCodeReturn<SpecialPostReacts>
                ._404_NotFound("Post react not found");
        }

        public async Task<ApiResponse<SpecialPostReacts>> GetSpecialPostReactsByReactIdAsync(string reactId)
        {
            var react = await _reactRepository.GetReactByIdAsync(reactId);
            if (react != null)
            {
                var postReact = await _specialPostsReactsRepository.GetSpecialPostReactsByReactIdAsync(
                    reactId);
                if (postReact != null)
                {
                    return StatusCodeReturn<SpecialPostReacts>
                        ._200_Success("Post react found successfully", postReact);
                }
                return StatusCodeReturn<SpecialPostReacts>
                    ._404_NotFound("Post react not found");
            }
            return StatusCodeReturn<SpecialPostReacts>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<SpecialPostReacts>> UpdateSpecialPostReactsAsync(
            UpdateSpecialPostsReactsDto updateSpecialPostsReactsDto)
        {
            var react = await _reactRepository.GetReactByIdAsync(updateSpecialPostsReactsDto.ReactId);
            if (react != null)
            {
                var postReact = await _specialPostsReactsRepository.GetSpecialPostReactsByIdAsync(
                    updateSpecialPostsReactsDto.Id);
                if (postReact != null)
                {
                    var existPostReact = await _specialPostsReactsRepository.GetSpecialPostReactsByIdAsync(
                    updateSpecialPostsReactsDto.ReactId);
                    if (existPostReact == null)
                    {
                        var updatedPostReact = await _specialPostsReactsRepository
                            .UpdateSpecialPostReactsAsync(
                            ConvertFromDto.ConvertSpecialPostReactsDto_Update(updateSpecialPostsReactsDto));
                        return StatusCodeReturn<SpecialPostReacts>
                            ._200_Success("Post react updated successfully", updatedPostReact);
                    }
                    return StatusCodeReturn<SpecialPostReacts>
                        ._403_Forbidden("Post react already exists");
                }
                return StatusCodeReturn<SpecialPostReacts>
                        ._404_NotFound("Post react not found");
            }
            return StatusCodeReturn<SpecialPostReacts>
                        ._404_NotFound("React not found");
        }
    }
}

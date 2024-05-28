

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Repository.SpecialCommentReactsRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.SpecialCommentReactsService
{
    public class SpecialCommentReactsService : ISpecialCommentReactsService
    {
        private readonly ISpecialCommentReactsRepository _specialCommentReactsRepository;
        private readonly IReactRepository _reactRepository;
        public SpecialCommentReactsService(ISpecialCommentReactsRepository _specialCommentReactsRepository,
            IReactRepository _reactRepository)
        {
            this._specialCommentReactsRepository = _specialCommentReactsRepository;
            this._reactRepository = _reactRepository;
        }
        public async Task<ApiResponse<SpecialCommentReacts>> AddSpecialCommentReactsAsync(
            AddSpecialCommentReactsDto addSpecialCommentReactsDto)
        {
            var react = await _reactRepository.GetReactByIdAsync(addSpecialCommentReactsDto.ReactId);
            if (react != null)
            {
                var existCommentReact = await _specialCommentReactsRepository.GetSpecialCommentReactsByIdAsync(
                    addSpecialCommentReactsDto.ReactId);
                if (existCommentReact != null)
                {
                    return StatusCodeReturn<SpecialCommentReacts>
                        ._403_Forbidden("Comment react already exists");
                }
                var newCommentReact = await _specialCommentReactsRepository.AddSpecialCommentReactsAsync(
                    ConvertFromDto.ConvertSpecialCommentReactsDto_Add(addSpecialCommentReactsDto));
                return StatusCodeReturn<SpecialCommentReacts>
                    ._201_Created("Comment react created successfully", newCommentReact);
            }
            return StatusCodeReturn<SpecialCommentReacts>
                        ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<SpecialCommentReacts>> DeleteSpecialCommentReactsByIdAsync(string Id)
        {
            var CommentReact = await _specialCommentReactsRepository.GetSpecialCommentReactsByIdAsync(Id);
            if (CommentReact != null)
            {
                await _specialCommentReactsRepository.DeleteSpecialCommentReactsByIdAsync(Id);
                return StatusCodeReturn<SpecialCommentReacts>
                    ._200_Success("Comment react deleted successfully", CommentReact);
            }
            return StatusCodeReturn<SpecialCommentReacts>
                ._404_NotFound("Comment react not found");
        }

        public async Task<ApiResponse<SpecialCommentReacts>> DeleteSpecialCommentReactsByReactIdAsync(
            string reactId)
        {
            var react = await _reactRepository.GetReactByIdAsync(reactId);
            if (react != null)
            {
                var CommentReact = await _specialCommentReactsRepository.GetSpecialCommentReactsByReactIdAsync(
                    reactId);
                if (CommentReact != null)
                {
                    await _specialCommentReactsRepository.DeleteSpecialCommentReactsByReactIdAsync(reactId);
                    return StatusCodeReturn<SpecialCommentReacts>
                        ._200_Success("Comment react deleted successfully", CommentReact);
                }
                return StatusCodeReturn<SpecialCommentReacts>
                    ._404_NotFound("Comment react not found");
            }
            return StatusCodeReturn<SpecialCommentReacts>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<IEnumerable<SpecialCommentReacts>>> GetSpecialCommentReactsAsync()
        {
            var CommentReacts = await _specialCommentReactsRepository.GetSpecialCommentReactsAsync();
            if (CommentReacts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<SpecialCommentReacts>>
                    ._200_Success("No Comment reacts found", CommentReacts);
            }
            return StatusCodeReturn<IEnumerable<SpecialCommentReacts>>
                    ._200_Success("Comment reacts found successfully", CommentReacts);
        }

        public async Task<ApiResponse<SpecialCommentReacts>> GetSpecialCommentReactsByIdAsync(string Id)
        {
            var CommentReact = await _specialCommentReactsRepository.GetSpecialCommentReactsByIdAsync(Id);
            if (CommentReact != null)
            {
                return StatusCodeReturn<SpecialCommentReacts>
                    ._200_Success("Comment react found successfully", CommentReact);
            }
            return StatusCodeReturn<SpecialCommentReacts>
                ._404_NotFound("Comment react not found");
        }

        public async Task<ApiResponse<SpecialCommentReacts>> GetSpecialCommentReactsByReactIdAsync(
            string reactId)
        {
            var react = await _reactRepository.GetReactByIdAsync(reactId);
            if (react != null)
            {
                var CommentReact = await _specialCommentReactsRepository.GetSpecialCommentReactsByReactIdAsync(
                    reactId);
                if (CommentReact != null)
                {
                    return StatusCodeReturn<SpecialCommentReacts>
                        ._200_Success("Comment react found successfully", CommentReact);
                }
                return StatusCodeReturn<SpecialCommentReacts>
                    ._404_NotFound("Comment react not found");
            }
            return StatusCodeReturn<SpecialCommentReacts>
                    ._404_NotFound("React not found");
        }

        public async Task<ApiResponse<SpecialCommentReacts>> UpdateSpecialCommentReactsAsync(
            UpdateSpecialCommentReactsDto updateSpecialCommentReactsDto)
        {
            var react = await _reactRepository.GetReactByIdAsync(updateSpecialCommentReactsDto.ReactId);
            if (react != null)
            {
                var CommentReact = await _specialCommentReactsRepository.GetSpecialCommentReactsByIdAsync(
                    updateSpecialCommentReactsDto.Id);
                if (CommentReact != null)
                {
                    var existCommentReact = await _specialCommentReactsRepository.GetSpecialCommentReactsByIdAsync(
                    updateSpecialCommentReactsDto.ReactId);
                    if (existCommentReact == null)
                    {
                        var updatedCommentReact = await _specialCommentReactsRepository
                            .UpdateSpecialCommentReactsAsync(
                            ConvertFromDto.ConvertSpecialCommentReactsDto_Update(updateSpecialCommentReactsDto));
                        return StatusCodeReturn<SpecialCommentReacts>
                            ._200_Success("Comment react updated successfully", updatedCommentReact);
                    }
                    return StatusCodeReturn<SpecialCommentReacts>
                        ._403_Forbidden("Comment react already exists");
                }
                return StatusCodeReturn<SpecialCommentReacts>
                        ._404_NotFound("Comment react not found");
            }
            return StatusCodeReturn<SpecialCommentReacts>
                        ._404_NotFound("React not found");
        }
    }
}

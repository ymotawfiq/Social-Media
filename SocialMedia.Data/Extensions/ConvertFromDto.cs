

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.Extensions
{
    public static class ConvertFromDto
    {
        public static React ConvertFromReactDto_Add(ReactDto reactDto)
        {
            return new React
            {
                ReactValue = reactDto.ReactValue
            };
        }

        public static React ConvertFromReactDto_Update(ReactDto reactDto)
        {
            if (reactDto.Id == null)
            {
                throw new NullReferenceException("React id must not be null");
            }
            return new React
            {
                Id = new Guid(reactDto.Id),
                ReactValue = reactDto.ReactValue
            };
        }
    }
}

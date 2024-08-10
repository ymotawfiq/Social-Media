
using SocialMedia.Api.Data.DTOs.Mappers;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Extensions
{
    public static class ConvertToDto
    {
        
        public static UserMapperDto FromUserToUserMapperDto(SiteUser user){
            return new UserMapperDto{
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                UserName = user.UserName!,
                DisplayName = user.DisplayName,
            };
        }

        public static UserMapperDto FromUserToUserMapperDto(SiteUser user, IList<string> roles){
            return new UserMapperDto{
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                UserName = user.UserName!,
                DisplayName = user.DisplayName,
                Roles = roles
            };
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Api.Data.DTOs.Mappers
{
    public class UserMapperDto
    {
        public string Id {get; set;} = null!;
        public string FirstName {get; set;} = null!;
        public string LastName {get; set;} = null!;
        public string Email {get; set;} = null!;
        public string UserName {get; set;} = null!;
        public string DisplayName {get; set;} = null!;
        public IList<string>? Roles {get; set;}
    }
}
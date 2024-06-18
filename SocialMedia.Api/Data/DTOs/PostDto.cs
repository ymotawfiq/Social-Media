


using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.DTOs
{
    public class PostDto
    {

        public Post Post { get; set; } = null!;

        public List<PostImages>? Images { get; set; }

    }
}

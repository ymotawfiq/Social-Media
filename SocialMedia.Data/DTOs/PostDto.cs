

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.DTOs
{
    public class PostDto
    {

        public Post Post { get; set; } = null!;

        public List<PostImages>? Images { get; set; }

    }
}

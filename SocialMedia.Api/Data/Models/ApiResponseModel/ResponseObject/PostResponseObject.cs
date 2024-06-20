
namespace SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject
{
    public class PostResponseObject
    {

        public Post Post { get; set; } = null!;

        public List<PostImages>? Images { get; set; }

    }
}

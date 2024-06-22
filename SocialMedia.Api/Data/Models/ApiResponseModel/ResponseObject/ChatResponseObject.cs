using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject
{
    public class ChatResponseObject
    {
        public Chat Chat { get; set; } = null!;
        public Policy Policy { get; set; } = null!;
        public SiteUser? User { get; set; }
    }
}

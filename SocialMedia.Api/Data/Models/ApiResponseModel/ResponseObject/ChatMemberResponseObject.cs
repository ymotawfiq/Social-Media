using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject
{
    public class ChatMemberResponseObject
    {
        public ChatMember ChatMember { get; set; } = null!;
        public ChatResponseObject Chat { get; set; } = null!;
        public SiteUser Member { get; set; } = null!;

    }
}

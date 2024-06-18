

namespace SocialMedia.Api.Data.DTOs
{
    public class SendSarahaMessageDto
    {
        public string UserIdOrNameOrEmail { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool ShareYourName { get; set; }
    }
}

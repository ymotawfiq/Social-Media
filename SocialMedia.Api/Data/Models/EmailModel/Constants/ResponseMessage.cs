

namespace SocialMedia.Api.Data.Models.EmailModel.Constants
{
    public class ResponseMessage
    {
        public static string GetEmailSuccessMessage(string emailAddress)
        {
            return $"Email sent successfully to {emailAddress}";
        }
    }
}

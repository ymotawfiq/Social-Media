

using SocialMedia.Api.Data.Models.MessageModel;

namespace SocialMedia.Api.Service.SendEmailService
{
    public interface ISendEmailService
    {
        string SendEmail(Message message);
    }
}

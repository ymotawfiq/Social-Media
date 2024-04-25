

using SocialMedia.Data.Models.MessageModel;

namespace SocialMedia.Service.SendEmailService
{
    public interface IEmailService
    {
        string SendEmail(Message message);
    }
}

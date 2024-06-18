

using SocialMedia.Api.Data.Models.MessageModel;

namespace SocialMedia.Api.Service.SendEmailService
{
    public interface IEmailService
    {
        string SendEmail(Message message);
    }
}

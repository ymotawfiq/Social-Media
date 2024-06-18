

using MailKit.Net.Smtp;
using MimeKit;
using SocialMedia.Api.Data.Models.EmailModel;
using SocialMedia.Api.Data.Models.EmailModel.Constants;
using SocialMedia.Api.Data.Models.MessageModel;

namespace SocialMedia.Api.Service.SendEmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration _emailConfig)
        {
            this._emailConfig = _emailConfig;
        }
        public string SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
            //var recipients = string.Join(", ", message.To);
            return ResponseMessage.GetEmailSuccessMessage("Success");
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private void Send(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.Username, _emailConfig.Password);

                client.Send(message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

    }
}



using MimeKit;
using System.Linq;

namespace SocialMedia.Data.Models.MessageModel
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Content { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public Message(IEnumerable<string> to, string Content, string Subject)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            this.Content = Content;
            this.Subject = Subject;
        }
    }
}

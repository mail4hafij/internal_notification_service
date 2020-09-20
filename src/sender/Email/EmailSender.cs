using SendGrid;
using SendGrid.Helpers.Mail;
using src.models;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace src.sender.Email
{
    public class EmailSender : IEmailSender
    {
        public void Send(IMessage message, ISettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Get("recipients")) || message.IsEmpty)
            {
                return;
            }

            var sender = settings.Get("sender") ?? "dev@org.com";
            var from = new EmailAddress(sender, "DEV ROBOT");
            var recipients = GetRecipients(settings);
            
            foreach (var recipient in recipients)
            {
                var to = new EmailAddress(recipient);
                var msg = MailHelper.CreateSingleEmail(from, to, message.Subject, message.Text, "<p>" + message.Text + "</p>");
                var res = Task.Run(() => SendMessageAsync(settings, msg)).Result;
            }
            
        }

        private static async Task<Response> SendMessageAsync(ISettings settings, SendGridMessage msg)
        {
            var apiKey = settings.Get("api_key");
            var client = new SendGridClient(apiKey);
            return await client.SendEmailAsync(msg);
        }

        public string[] GetRecipients(ISettings settings)
        {
            return settings.Get("recipients")
                .Replace(";", ",")
                .Split(',')
                .Select(o => o.Trim())
                .ToArray();
        }

    }
}

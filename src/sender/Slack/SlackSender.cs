using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using src.models;

namespace src.sender.Slack
{
    public class SlackSender : ISlackSender
    {
        private static readonly HttpClient Client = new HttpClient();

        public void Send(IMessage message, ISettings settings)
        {
            if (message.IsEmpty)
            {
                return;
            }

            var url = new Uri(settings.Get("url") ?? "https://hooks.slack.com/services/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");
            var channel = settings.Get("channel") ?? "#poc";

            
            var res = Task.Run(() => SendMessageAsync(url, message.Subject, message.Text, channel, message.Status)).Result;
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"Could not send to Slack. ErrorCode={res.StatusCode} {res.ReasonPhrase}");
            }
        }

        
        private static async Task<HttpResponseMessage> SendMessageAsync(Uri webhookUrl, string title, string message, string channel, Status status)
        {
            dynamic payload = new
            {
                channel,
                username = "RT-internal-control",
                attachments = new[]
                {
                    new
                    {
                        color = status == Status.Positive ? "good" : "danger",
                        title = title ?? string.Empty,
                        text = message ?? string.Empty
                    }
                }
            };

            var serializedPayload = JsonConvert.SerializeObject(payload);
            return await Client.PostAsync(webhookUrl, new StringContent(serializedPayload, Encoding.UTF8, "application/json"));
        }
    }
}

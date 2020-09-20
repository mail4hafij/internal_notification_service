using src.sender.Email;
using src.sender.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.sender
{
    public class SenderFactory : ISenderFactory
    {
        public IEmailSender Email()
        {
            return new EmailSender();
        }
        public ISlackSender Slack()
        {
            return new SlackSender();
        }
    }
}

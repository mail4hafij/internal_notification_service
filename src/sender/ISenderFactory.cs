using src.sender.Email;
using src.sender.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.sender
{
    public interface ISenderFactory
    {
        IEmailSender Email();
        ISlackSender Slack();
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net
{
    public class FakeEmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        public FakeEmailSender(ILogger<FakeEmailSender> logger)
        {
            _logger = logger;
        }

        private void WriteLog(string methodName, EmailMessage emailMessage)
        {
            StringBuilder sb = new StringBuilder(methodName);
            sb.AppendLine($"MailtoOnBehalfOf:{emailMessage.OnBehalfOf}");
            sb.AppendLine($"Mailto:{emailMessage.MailTo}");
            sb.AppendLine($"Subject:{emailMessage.Subject}");
            sb.AppendLine($"Body:{emailMessage.Body}");
            int attIndex = 1;
            if (emailMessage.Attachments != null && emailMessage.Attachments.Count > 0)
            {
                foreach (var att in emailMessage.Attachments)
                {
                    sb.AppendLine($"Att:{attIndex},{att.Key}");
                    attIndex++;
                }
            }
            _logger.LogInformation(sb.ToString());
        }

        public virtual void Send(EmailMessage emailMessage)
        {
            WriteLog("Send",emailMessage);
        }

        public virtual Task SendAsync(EmailMessage emailMessage)
        {
            WriteLog("SendAsync", emailMessage);
            return Task.CompletedTask;
        }
    }
}

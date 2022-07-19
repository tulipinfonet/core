using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace TulipInfo.Net
{
    public class FakeEmailSender : IEmailSender
    {
        protected ILogger Logger { get; private set; }
        protected SmtpEmailSenderOptions Options { get; private set; }
        public FakeEmailSender(ILogger<FakeEmailSender> logger,
            IOptions<SmtpEmailSenderOptions> options)
        {
            Logger = logger;
            Options = options.Value;
        }

        private void WriteLog(string methodName, SmtpEmailSenderOptions opt, EmailMessage emailMessage)
        {
            StringBuilder sb = new StringBuilder(methodName);
            sb.AppendLine($"Host:{opt.Host}");
            sb.AppendLine($"Port:{opt.Port}");
            sb.AppendLine($"Username:{opt.UserName}");
            sb.AppendLine($"Domain:{opt.Domain}");
            sb.AppendLine($"EnableSSL:{opt.EnableSSL}");
            sb.AppendLine($"MailFrom:{opt.MailFrom}");
            sb.AppendLine($"MailFromDisplayName:{opt.MailFromDisplayName}");
            sb.AppendLine($"OnBehalfOf:{opt.OnBehalfOf}");
            sb.AppendLine($"UseDefaultCredentials:{(opt.UseDefaultCredentials.HasValue ? string.Empty : opt.UseDefaultCredentials.ToString())}");
            sb.AppendLine($"SecurityProtocol:{opt.SecurityProtocol}");
            sb.AppendLine($"DeliveryMethod:{opt.DeliveryMethod}");
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
            Logger.LogInformation(sb.ToString());
        }

        public virtual void Send(EmailMessage emailMessage)
        {
            WriteLog("Send", this.Options, emailMessage);
        }

        public virtual void SendAsync(EmailMessage emailMessage)
        {
            WriteLog("SendAsync", this.Options, emailMessage);
        }
    }
}

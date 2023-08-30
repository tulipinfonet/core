using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly SmtpEmailSenderOptions _options;

        public SmtpEmailSender(ILogger<SmtpEmailSender> logger,
            IOptions<SmtpEmailSenderOptions> options)
        {
            _logger = logger;
            _options = options.Value;
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
            _logger.LogInformation(sb.ToString());
        }


        public void Send(EmailMessage emailMessage)
        {
            WriteLog("Send", _options, emailMessage);

            if (_options.UseFake == false)
            {
                SmtpEmailHelper.Send(_options, emailMessage);
            }
        }

        public Task SendAsync(EmailMessage emailMessage)
        {
            WriteLog("SendAsync", _options, emailMessage);

            if (_options.UseFake == false)
            {
                SmtpEmailHelper.SendAsync(_options, emailMessage, SmtpClientSendCompleted);
            }
            return Task.CompletedTask;
        }

        void SmtpClientSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _logger.LogError(e.Error, string.Format("Send Email Failed,\r\n{0}", GetSendingInformation(e.UserState)));
            }
            else
            {
                _logger.LogInformation(string.Format("Send Email Success,\r\n{0}", GetSendingInformation(e.UserState)));
            }

            ((SmtpClient)sender).Dispose();
        }

        private string GetSendingInformation(object state)
        {
            Dictionary<string, object> dicParam = state as Dictionary<string, object>;
            string info = "";
            if (dicParam != null)
            {
                foreach (var kv in dicParam)
                {
                    info += kv.Key + ":" + (kv.Value ?? "") + "\r\n";
                }
            }
            return info;
        }
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Reflection;
using System.IO;

namespace TulipInfo.Net.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly SendGridOptions _options;
        public SendGridEmailSender(ILogger<SendGridEmailSender> logger,
            IOptions<SendGridOptions> options
            ) 
        {
            _logger = logger;
            _options = options.Value;
        }

        public void Send(EmailMessage emailMessage)
        {
            this.SendAsync(emailMessage).Wait();
        }

        public async Task SendAsync(EmailMessage emailMessage)
        {
            StringBuilder sb = new StringBuilder("SendGrid:");
            sb.AppendLine($"MailFrom:{_options.MailFrom}");
            sb.AppendLine($"MailFromDisplayName:{_options.MailFromDisplayName}");
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

            try
            {
                var client = new SendGridClient(_options.ApiKey);
                var from = new EmailAddress(_options.MailFrom, _options.MailFromDisplayName);
                List<EmailAddress> tos = emailMessage.MailTo.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => new EmailAddress(t)).ToList();
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from,
                    tos,
                    emailMessage.Subject, "", emailMessage.Body);
                if (emailMessage.Attachments != null && emailMessage.Attachments.Count > 0)
                {
                    foreach (var att in emailMessage.Attachments)
                    {
                        await msg.AddAttachmentAsync(att.Key, new MemoryStream(att.Value));
                    }
                }

                var response = await client.SendEmailAsync(msg);

                string responseText = await response.Body.ReadAsStringAsync();

                _logger.LogInformation($"Send Grid Response:{responseText}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendGridError");
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace TulipInfo.Net
{
    public class SmtpEmailSender : FakeEmailSender
    {
        public SmtpEmailSender(ILogger<SmtpEmailSender> logger,
            IOptions<SmtpEmailSenderOptions> options
            ) : base(logger, options)
        {
        }

        public override void Send(EmailMessage emailMessage)
        {
            base.Send(emailMessage);

            if (this.Options.UseFake == false)
            {
                SmtpEmailHelper.Send(this.Options, emailMessage);
            }
        }

        public override void SendAsync(EmailMessage emailMessage)
        {
            base.SendAsync(emailMessage);

            if (this.Options.UseFake == false)
            {
                SmtpEmailHelper.SendAsync(this.Options, emailMessage, SmtpClientSendCompleted);
            }
        }

        void SmtpClientSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.Logger.LogError(e.Error, string.Format("Send Email Failed,\r\n{0}", GetSendingInformation(e.UserState)));
            }
            else
            {
                this.Logger.LogInformation(string.Format("Send Email Success,\r\n{0}", GetSendingInformation(e.UserState)));
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

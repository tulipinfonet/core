using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net
{
    public interface IEmailSender
    {
        void Send(EmailMessage emailMessage);
        void SendAsync(EmailMessage emailMessage);
    }
}

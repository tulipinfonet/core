using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net
{
    public interface IEmailSender
    {
        void Send(EmailMessage emailMessage);
        Task SendAsync(EmailMessage emailMessage);
    }
}

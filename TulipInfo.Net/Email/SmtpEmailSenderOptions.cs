using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TulipInfo.Net
{
    public class SmtpEmailSenderOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// optional
        /// </summary>
        public string Domain { get; set; }
        public bool EnableSSL { get; set; }
        public string MailFrom { get; set; }
        /// <summary>
        /// optional
        /// </summary>
        public string MailFromDisplayName { get; set; }
        /// <summary>
        /// optional
        /// </summary>
        public string OnBehalfOf { get; set; }
        public bool? UseDefaultCredentials { get; set; }
        public SecurityProtocolType SecurityProtocol { get; set; } = SecurityProtocolType.SystemDefault;
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;
        public bool UseFake { get; set; }
    }
}

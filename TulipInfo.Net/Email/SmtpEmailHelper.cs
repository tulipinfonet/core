using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Net.Mime;

namespace TulipInfo.Net
{
    public static class SmtpEmailHelper
    {
        public static void Send(SmtpEmailSenderOptions opt, EmailMessage emailMessage)
        {
            MailMessage email = GetMailMessage(opt, emailMessage);
            SmtpClient smtpClient = GetSmtpClient(opt);
            smtpClient.Send(email);
        }

        public static void SendAsync(SmtpEmailSenderOptions opt, EmailMessage emailMessage, SendCompletedEventHandler completedCallback)
        {
            MailMessage email = GetMailMessage(opt, emailMessage);
            SmtpClient smtpClient = GetSmtpClient(opt);
            if (completedCallback != null)
            {
                smtpClient.SendCompleted += completedCallback;
            }
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            dicParam.Add("Host", opt.Host);
            dicParam.Add("Port", opt.Port);
            dicParam.Add("Username", opt.UserName);
            dicParam.Add("Password", opt.Password);
            dicParam.Add("Domain", opt.Domain);
            dicParam.Add("EnableSSL", opt.EnableSSL);
            dicParam.Add("MailFrom", opt.MailFrom);
            dicParam.Add("MailFromDisplayName", opt.MailFromDisplayName);
            dicParam.Add("OnBehalfOf", opt.OnBehalfOf);
            dicParam.Add("UseDefaultCredentials", opt.UseDefaultCredentials.HasValue ? "" : opt.UseDefaultCredentials.ToString());
            dicParam.Add("SecurityProtocol", opt.SecurityProtocol);
            dicParam.Add("DeliveryMethod", opt.DeliveryMethod);
            dicParam.Add("MailtoOnBehalfOf", emailMessage.OnBehalfOf);
            dicParam.Add("Mailto", emailMessage.MailTo);
            dicParam.Add("Subject", emailMessage.Subject);
            dicParam.Add("Body", emailMessage.Body);
            int attIndex = 1;
            if (emailMessage.Attachments != null && emailMessage.Attachments.Count > 0)
            {
                foreach (var att in emailMessage.Attachments)
                {
                    dicParam.Add("Att" + attIndex, att.Key);
                    attIndex++;
                }
            }

            smtpClient.SendAsync(email, dicParam);
        }


        private static SmtpClient GetSmtpClient(string host, int port, string username, string password, bool enableSSL)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Timeout = 20000;
            smtpClient.Host = host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (port > 0)
            {
                smtpClient.Port = port;
            }
            smtpClient.UseDefaultCredentials = true;
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(username, password, "cogniciti.com");
            }
            else
            {
                //smtpClient.UseDefaultCredentials = true;
            }
            smtpClient.EnableSsl = enableSSL;
            return smtpClient;
        }

        private static SmtpClient GetSmtpClient(SmtpEmailSenderOptions opt)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Timeout = 20000;
            smtpClient.Host = opt.Host;
            smtpClient.DeliveryMethod = opt.DeliveryMethod;
            if (opt.Port > 0)
            {
                smtpClient.Port = opt.Port;
            }
            if (opt.UseDefaultCredentials.HasValue)
            {
                smtpClient.UseDefaultCredentials = opt.UseDefaultCredentials.Value;
            }
            if (!string.IsNullOrWhiteSpace(opt.UserName) && !string.IsNullOrWhiteSpace(opt.Password))
            {
                if (opt.UseDefaultCredentials.HasValue == false)
                {
                    smtpClient.UseDefaultCredentials = false;
                }
                if (!string.IsNullOrWhiteSpace(opt.Domain))
                {
                    smtpClient.Credentials = new NetworkCredential(opt.UserName, opt.Password, opt.Domain);
                }
                else
                {
                    smtpClient.Credentials = new NetworkCredential(opt.UserName, opt.Password);
                }
            }
            else
            {
                if (opt.UseDefaultCredentials.HasValue == false)
                {
                    smtpClient.UseDefaultCredentials = true;
                }
            }
            smtpClient.EnableSsl = opt.EnableSSL;
            if (opt.SecurityProtocol != SecurityProtocolType.SystemDefault)
            {
                ServicePointManager.SecurityProtocol = opt.SecurityProtocol;
            }
            return smtpClient;
        }

        private static MailMessage GetMailMessage(string mailFrom, string subject, string content, string[] mailToArray, IDictionary<string, byte[]> attachments)
        {
            MailMessage email = new MailMessage();
            email.Sender = new MailAddress(mailFrom, "Cogniciti Team");
            email.From = new MailAddress(mailFrom, "Cogniciti Team");
            foreach (string to in mailToArray)
            {
                try
                {
                    email.To.Add(new MailAddress(to));
                }
                catch
                {
                    //ignore 
                }
            }
            email.IsBodyHtml = true;
            email.BodyEncoding = Encoding.UTF8;
            email.Subject = subject;
            email.Body = content;
            if (attachments != null && attachments.Count > 0)
            {
                foreach (var att in attachments)
                {
                    email.Attachments.Add(new Attachment(new MemoryStream(att.Value), att.Key, MimeTypeMap.GetMimeType(att.Key)));
                }
            }
            return email;
        }

        private static MailMessage GetMailMessage(SmtpEmailSenderOptions opt, EmailMessage emailMessage)
        {
            MailMessage email = new MailMessage();
            email.Sender = new MailAddress(opt.MailFrom, opt.MailFromDisplayName);
            if (!string.IsNullOrWhiteSpace(emailMessage.OnBehalfOf))
            {
                email.From = new MailAddress(emailMessage.OnBehalfOf);
            }
            else if (!string.IsNullOrWhiteSpace(opt.OnBehalfOf))
            {
                email.From = new MailAddress(opt.OnBehalfOf);
            }
            else
            {
                email.From = email.Sender;
            }
            string[] mailToArray = emailMessage.MailTo.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string to in mailToArray)
            {
                try
                {
                    email.To.Add(new MailAddress(to));
                }
                catch
                {
                    //ignore 
                }
            }
            email.IsBodyHtml = true;
            email.BodyEncoding = Encoding.UTF8;
            email.Subject = emailMessage.Subject;
            email.Body = emailMessage.Body;
            if (emailMessage.Attachments != null && emailMessage.Attachments.Count > 0)
            {
                foreach (var att in emailMessage.Attachments)
                {
                    email.Attachments.Add(new Attachment(new MemoryStream(att.Value), att.Key, MimeTypeMap.GetMimeType(att.Key)));
                }
            }
            return email;
        }
    }
}

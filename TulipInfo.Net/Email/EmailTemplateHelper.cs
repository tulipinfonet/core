using System.Reflection;
using System.Text.Encodings.Web;

namespace TulipInfo.Net
{
    public static class EmailTemplateHelper
    {
        public static (string subject, string body) GetEmailContent(string templateFile, object formatData, bool useHtml = true)
        {
            string subject = "";
            string body = "";

            using (StreamReader sr = new StreamReader(templateFile))
            {
                subject = sr.ReadLine()!;
                body = sr.ReadToEnd();
            }

            if (formatData != null)
            {
                subject = StringTemplate.Format(subject, formatData, useHtml);
                body = StringTemplate.Format(body, formatData, useHtml);
            }

            return (subject, body);
        }
    }
}

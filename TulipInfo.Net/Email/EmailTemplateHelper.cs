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
                subject = GetFormattedTemplate(subject, formatData, useHtml);
                body = GetFormattedTemplate(body, formatData, useHtml);
            }

            return (subject, body);
        }

        public static string GetFormattedTemplate(string template, object data, bool useHtml = true)
        {
            Type type = data.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string content = template;
            foreach (var prop in properties)
            {
                object? propValue = prop.GetValue(data, null);
                if (propValue != null)
                {
                    string fieldValue = propValue.ToString() ?? "";
                    if (useHtml)
                    {
                        fieldValue = HtmlEncoder.Default.Encode(fieldValue);
                    }
                    content = content.Replace("$" + prop.Name + "$", fieldValue);
                }
            }

            return content;
        }
    }
}

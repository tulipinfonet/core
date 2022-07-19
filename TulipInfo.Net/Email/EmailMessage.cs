using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net
{
    public class EmailMessage
    {
        /// <summary>
        /// Mail From OnbehalfOf(Optional)
        /// </summary>
        public string OnBehalfOf { get; set; }
        /// <summary>
        /// Mail To Address, separate by comma or semicolon(Required)
        /// </summary>
        public string MailTo { get; set; }
        /// <summary>
        /// Subject(Required)
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Content(Required)
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Attachement,Key:FileName,Value:Filecontent (Optional)
        /// </summary>
        public IDictionary<string, byte[]> Attachments { get; set; }
    }
}

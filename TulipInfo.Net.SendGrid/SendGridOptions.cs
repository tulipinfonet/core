namespace TulipInfo.Net.SendGrid
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; } = null!;
        public string MailFrom { get; set; } = null!;
        /// <summary>
        /// optional
        /// </summary>
        public string? MailFromDisplayName { get; set; }
    }
}
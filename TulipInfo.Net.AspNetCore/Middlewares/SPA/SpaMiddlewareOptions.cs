namespace TulipInfo.Net.AspNetCore
{
    public class SpaMiddlewareOptions
    {
        public bool UseMultipleLanguages { get; set; } = true;
        public string ClientAppFolder { get; set; } = "ClientApp";
        public string DefaultLanguage { get; set; } = "en";
    }
}

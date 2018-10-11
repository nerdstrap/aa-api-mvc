namespace Nerdstrap.Identity.Services.RiskAnalysis.Models
{
    public class DeviceRequest
	{
        public string DevicePrint { get; set; }
        public string DeviceTokenCookie { get; set; }
        public string DeviceTokenFSO { get; set; }
        public string HttpAccept { get; set; }
        public string HttpAcceptChars { get; set; }
        public string HttpAcceptEncoding { get; set; }
        public string HttpAcceptLanguage { get; set; }
        public string HttpReferrer { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}

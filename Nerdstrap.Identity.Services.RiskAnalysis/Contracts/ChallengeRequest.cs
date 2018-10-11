using Nerdstrap.Identity.Services.RiskAnalysis.Enums;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class ChallengeRequest : BaseRequest
    {
        public CredentialTypeEnum CredentialType { get; set; }
        public string ContactInfo { get; set; }
        public string Label { get; set; }
    }
}
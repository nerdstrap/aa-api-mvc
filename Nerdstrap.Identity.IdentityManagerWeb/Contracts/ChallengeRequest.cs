using Nerdstrap.Identity.IdentityManagerWeb.Enums;

namespace Nerdstrap.Identity.IdentityManagerWeb.Contracts
{
    public class ChallengeRequest : BaseRequest
    {
        public CredentialTypeEnum CredentialType { get; set; }
        public string ContactInfo { get; set; }
        public string Label { get; set; }
    }
}
using Nerdstrap.Identity.IdentityManagerWeb.Enums;

namespace Nerdstrap.Identity.IdentityManagerWeb.Contracts
{
    public class AuthenticateRequest : BaseRequest
    {
        public CredentialTypeEnum CredentialType { get; set; }
        public string Credentials { get; set; }
        public bool BindDevice { get; set; }

        public bool ShouldSerializeCredentials()
        {
            return false;
        }
    }
}

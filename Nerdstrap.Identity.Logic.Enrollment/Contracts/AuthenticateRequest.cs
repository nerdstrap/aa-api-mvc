using Nerdstrap.Identity.Logic.Enrollment.Enums;

namespace Nerdstrap.Identity.Logic.Enrollment.Contracts
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

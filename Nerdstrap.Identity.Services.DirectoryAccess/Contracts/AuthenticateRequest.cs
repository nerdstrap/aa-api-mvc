namespace Nerdstrap.Identity.Services.DirectoryAccess.Contracts
{
    public class AuthenticateRequest : BaseRequest
    {
        public string Credentials { get; set; }

        public bool ShouldSerializeCredentials()
        {
            return false;
        }
    }
}

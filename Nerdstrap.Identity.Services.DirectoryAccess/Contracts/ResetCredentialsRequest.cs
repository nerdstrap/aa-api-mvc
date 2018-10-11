namespace Nerdstrap.Identity.Services.DirectoryAccess.Contracts
{
    public class ResetCredentialsRequest : BaseRequest
    {
        public string CurrentCredentials { get; set; }
        public string NewCredentials { get; set; }

        public bool ShouldSerializeCurrentCredentials()
        {
            return false;
        }

        public bool ShouldSerializeNewCredentials()
        {
            return false;
        }
    }
}

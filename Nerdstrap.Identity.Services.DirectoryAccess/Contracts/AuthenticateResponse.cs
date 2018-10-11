using Nerdstrap.Identity.Services.DirectoryAccess.Enums;

namespace Nerdstrap.Identity.Services.DirectoryAccess.Contracts
{
    public class AuthenticateResponse : BaseResponse
    {
        public AuthenticateResultEnum AuthenticateResult { get; set; }
    }
}

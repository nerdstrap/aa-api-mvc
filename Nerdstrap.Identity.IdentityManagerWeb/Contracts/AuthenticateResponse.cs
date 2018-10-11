using Nerdstrap.Identity.IdentityManagerWeb.Enums;

namespace Nerdstrap.Identity.IdentityManagerWeb.Contracts
{
    public class AuthenticateResponse : BaseResponse
    {
        public AuthenticateResultEnum AuthenticateResult { get; set; }
    }
}

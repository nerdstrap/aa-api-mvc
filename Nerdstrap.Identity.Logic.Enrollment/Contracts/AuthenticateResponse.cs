using Nerdstrap.Identity.Logic.Enrollment.Enums;

namespace Nerdstrap.Identity.Logic.Enrollment.Contracts
{
    public class AuthenticateResponse : BaseResponse
    {
        public AuthenticateResultEnum AuthenticateResult { get; set; }
    }
}

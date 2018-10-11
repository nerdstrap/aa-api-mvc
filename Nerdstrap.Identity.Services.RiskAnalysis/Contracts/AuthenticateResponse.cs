using Nerdstrap.Identity.Services.RiskAnalysis.Enums;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class AuthenticateResponse : BaseResponse
    {
        public AuthenticateResultEnum AuthenticateResult { get; set; }
    }
}
using Nerdstrap.Identity.Services.RiskAnalysis.Enums;
using System.Collections.Generic;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class AnalyzeResponse : BaseResponse
    {
        public ActionCodeEnum ActionCode { get; set; }
        public IEnumerable<CredentialTypeEnum> CollectibleCredentials { get; set; }
    }
}
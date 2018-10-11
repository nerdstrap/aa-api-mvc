using Nerdstrap.Identity.Logic.Enrollment.Enums;
using System.Collections.Generic;

namespace Nerdstrap.Identity.Logic.Enrollment.Contracts
{
    public class AnalyzeResponse : BaseResponse
    {
        public ActionCodeEnum ActionCode { get; set; }
        public IEnumerable<CredentialTypeEnum> CollectibleCredentials { get; set; }
    }
}
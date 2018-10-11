using Nerdstrap.Identity.IdentityManagerWeb.Enums;
using System.Collections.Generic;

namespace Nerdstrap.Identity.IdentityManagerWeb.Contracts
{
    public class AnalyzeResponse : BaseResponse
    {
        public ActionCodeEnum ActionCode { get; set; }
        public IEnumerable<CredentialTypeEnum> CollectibleCredentials { get; set; }
    }
}
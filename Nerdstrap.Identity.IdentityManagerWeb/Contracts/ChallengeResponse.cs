using Nerdstrap.Identity.IdentityManagerWeb.Enums;
using Nerdstrap.Identity.IdentityManagerWeb.Models;
using System.Collections.Generic;

namespace Nerdstrap.Identity.IdentityManagerWeb.Contracts
{
    public class ChallengeResponse : BaseResponse
    {
        public ChallengeResultEnum ChallengeResult { get; set; }
        public IEnumerable<ChallengeQuestion> ChallengeQuestions { get; set; }
    }
}
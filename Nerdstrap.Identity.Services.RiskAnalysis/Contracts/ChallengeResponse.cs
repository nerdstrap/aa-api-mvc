using Nerdstrap.Identity.Services.RiskAnalysis.Enums;
using Nerdstrap.Identity.Services.RiskAnalysis.Models;
using System.Collections.Generic;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class ChallengeResponse : BaseResponse
    {
        public ChallengeResultEnum ChallengeResult { get; set; }
        public IEnumerable<ChallengeQuestion> ChallengeQuestions { get; set; }
    }
}
using Nerdstrap.Identity.Services.RiskAnalysis.Models;
using System.Collections.Generic;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class CreateUserResponse : BaseResponse
    {
        public IEnumerable<ChallengeQuestion> ChallengeQuestions { get; set; }
    }
}
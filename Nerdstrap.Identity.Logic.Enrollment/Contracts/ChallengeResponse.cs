using Nerdstrap.Identity.Logic.Enrollment.Enums;
using Nerdstrap.Identity.Logic.Enrollment.Models;
using System.Collections.Generic;

namespace Nerdstrap.Identity.Logic.Enrollment.Contracts
{
    public class ChallengeResponse : BaseResponse
    {
        public ChallengeResultEnum ChallengeResult { get; set; }
        public IEnumerable<ChallengeQuestion> ChallengeQuestions { get; set; }
    }
}
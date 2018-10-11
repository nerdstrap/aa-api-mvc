using Nerdstrap.Identity.Services.RiskAnalysis.Models;
using System.Collections.Generic;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class QueryResponse : BaseResponse
    {
        public IEnumerable<ChallengeQuestion> ChallengeQuestions { get; set; }
        public IEnumerable<EmailContact> EmailContacts { get; set; }
        public IEnumerable<PhoneContact> PhoneContacts { get; set; }
        public IEnumerable<SmsContact> SmsContacts { get; set; }
    }
}
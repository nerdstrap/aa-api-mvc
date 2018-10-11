using Nerdstrap.Identity.IdentityManagerWeb.Models;
using System.Collections.Generic;

namespace Nerdstrap.Identity.IdentityManagerWeb.Contracts
{
    public class GetUserResponse : BaseResponse
    {
        public UserAttributes UserAttributes { get; set; }
        public IEnumerable<ChallengeQuestion> ChallengeQuestions { get; set; }
        public IEnumerable<EmailContact> EmailContacts { get; set; }
        public IEnumerable<PhoneContact> PhoneContacts { get; set; }
        public IEnumerable<SmsContact> SmsContacts { get; set; }
    }
}

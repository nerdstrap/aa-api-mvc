using Nerdstrap.Identity.Services.RiskAnalysis.Enums;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class BaseResponse
    {
        public int DeviceRiskScore { get; set; }
        public AuthStatusEnum DeviceAuthStatus { get; set; }
        public CallStatusEnum CallStatus { get; set; }
        public string DeviceToken { get; set; }

        public string SessionId { get; set; }
        public string TransactionId { get; set; }
        public UserStatusEnum UserStatus { get; set; }
        public UserTypeEnum UserType { get; set; }

        public string RequestId { get; set; }
        public string TimeStamp { get; set; }

        public int ReasonCode { get; set; }
        public int StatusCode { get; set; }
    }
}

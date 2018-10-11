using Nerdstrap.Identity.Services.RiskAnalysis.Enums;
using Nerdstrap.Identity.Services.RiskAnalysis.Models;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class BaseRequest
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
        public UserStatusEnum UserStatus { get; set; }
        public UserTypeEnum UserType { get; set; }
        public DeviceRequest DeviceRequest { get; set; }
    }
}

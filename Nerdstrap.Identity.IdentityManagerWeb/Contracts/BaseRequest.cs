using Nerdstrap.Identity.IdentityManagerWeb.Enums;
using Nerdstrap.Identity.IdentityManagerWeb.Models;

namespace Nerdstrap.Identity.IdentityManagerWeb.Contracts
{
    public class BaseRequest
    {
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
        public string UserToken { get; set; }

        public UserStatusEnum UserStatus { get; set; }
        public UserTypeEnum UserType { get; set; }
        public DeviceRequest DeviceRequest { get; set; }
    }
}

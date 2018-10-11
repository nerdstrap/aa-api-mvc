
namespace Nerdstrap.Identity.IdentityManagerWeb.Contracts
{
    public class BaseResponse
    {
        public string UserToken { get; set; }
        public string DeviceToken { get; set; }
        public int ErrorCode { get; set; }
    }
}

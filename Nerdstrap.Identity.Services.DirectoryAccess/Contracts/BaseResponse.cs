
namespace Nerdstrap.Identity.Services.DirectoryAccess.Contracts
{
    public class BaseResponse
    {
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
        public string UserToken { get; set; }
        public string DeviceToken { get; set; }
        public int ErrorCode { get; set; }
    }
}

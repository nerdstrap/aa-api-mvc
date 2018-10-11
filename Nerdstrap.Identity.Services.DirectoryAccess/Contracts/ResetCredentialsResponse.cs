using Nerdstrap.Identity.Services.DirectoryAccess.Enums;

namespace Nerdstrap.Identity.Services.DirectoryAccess.Contracts
{
    public class ResetCredentialsResponse : BaseResponse
    {
        public ResetCredentialsResultEnum ResetCredentialsResult { get; set; }
        public ReplicateCredentialsStatusEnum ReplicateCredentialsStatus { get; set; }
    }
}

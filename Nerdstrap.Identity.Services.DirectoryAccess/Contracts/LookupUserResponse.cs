using Nerdstrap.Identity.Services.DirectoryAccess.Enums;

namespace Nerdstrap.Identity.Services.DirectoryAccess.Contracts
{
    public class LookupUserResponse : BaseResponse
    {
        public LookupUserResultEnum LookupUserResult { get; set; }
    }
}

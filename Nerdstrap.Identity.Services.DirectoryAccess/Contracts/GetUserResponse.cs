using Nerdstrap.Identity.Services.DirectoryAccess.Models;

namespace Nerdstrap.Identity.Services.DirectoryAccess.Contracts
{
    public class GetUserResponse : BaseResponse
    {
        public UserAttributes UserAttributes { get; set; }
    }
}

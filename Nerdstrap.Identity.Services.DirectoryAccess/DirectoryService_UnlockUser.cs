using System;
using log4net;
using Nerdstrap.Identity.Services.DirectoryAccess.Contracts;

namespace Nerdstrap.Identity.Services.DirectoryAccess
{
    public partial class DirectoryService : IDirectoryService
    {
        public virtual UnlockUserResponse UnlockUser(UnlockUserRequest unlockUserRequest)
        {
            throw new NotImplementedException();
        }
    }
}

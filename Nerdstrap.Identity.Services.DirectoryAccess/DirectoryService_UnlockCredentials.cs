using System;
using log4net;
using Nerdstrap.Identity.Services.DirectoryAccess.Contracts;

namespace Nerdstrap.Identity.Services.DirectoryAccess
{
    public partial class DirectoryService : IDirectoryService
    {
        public virtual UnlockCredentialsResponse UnlockCredentials(UnlockCredentialsRequest unlockUserCredentialsRequest)
        {
            throw new NotImplementedException();
        }
    }
}

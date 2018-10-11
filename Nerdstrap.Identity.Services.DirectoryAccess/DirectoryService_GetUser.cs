using Nerdstrap.Identity.Services.DirectoryAccess.Constants;
using Nerdstrap.Identity.Services.DirectoryAccess.Contracts;
using Nerdstrap.Identity.Services.DirectoryAccess.Enums;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;

namespace Nerdstrap.Identity.Services.DirectoryAccess
{
    public partial class DirectoryService : IDirectoryService
    {
        public virtual GetUserResponse GetUser(GetUserRequest getUserRequest)
        {
            // 1. validate input params
            if (string.IsNullOrWhiteSpace(getUserRequest.UserId))
            {
                throw new ArgumentNullException(nameof(getUserRequest.UserId));
            }

            var getUserResponse = new GetUserResponse();
            try
            {
                // 2. get admin directory connection
                using (LdapConnection adminDirectoryConnection = GetAdminDirectoryConnectionRetry() as LdapConnection)
                {
                    // 3. search for user
                    getUserResponse.UserAttributes = GetUserAttributes(UsersDistinguishedName, getUserRequest.UserId, adminDirectoryConnection);
                }
            }
            catch (LdapException ldapException)
            {
                Logger.Error(TraceLoggerLiterals.GET_USER_EXCEPTION, ldapException);
            }
            catch (Exception genericException)
            {
                Logger.Error(TraceLoggerLiterals.GET_USER_EXCEPTION, genericException);
            }

            return getUserResponse;
        }
    }
}

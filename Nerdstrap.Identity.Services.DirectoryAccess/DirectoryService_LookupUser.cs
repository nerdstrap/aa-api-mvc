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
        public virtual LookupUserResponse LookupUser(LookupUserRequest lookupUserRequest)
        {
            // 1. validate input params
            if (string.IsNullOrWhiteSpace(lookupUserRequest.UserId))
            {
                throw new ArgumentNullException(nameof(lookupUserRequest.UserId));
            }

            var checkUserExistsResponse = new LookupUserResponse();
            try
            {
                // 2. get admin directory connection
                using (LdapConnection adminDirectoryConnection = GetAdminDirectoryConnectionRetry() as LdapConnection)
                {
                    // 3. search for user
                    if (GetUserCount(UsersDistinguishedName, lookupUserRequest.UserId, adminDirectoryConnection) > 0)
                    {
                        checkUserExistsResponse.LookupUserResult = LookupUserResultEnum.Success;
                    }
                    else
                    {
                        checkUserExistsResponse.LookupUserResult = LookupUserResultEnum.Fail;
                    }
                }
            }
            catch (LdapException ldapException)
            {
                Logger.Error(TraceLoggerLiterals.CHECK_USER_EXISTS_EXCEPTION, ldapException);
            }
            catch (Exception genericException)
            {
                Logger.Error(TraceLoggerLiterals.CHECK_USER_EXISTS_EXCEPTION, genericException);
            }

            return checkUserExistsResponse;
        }
    }
}

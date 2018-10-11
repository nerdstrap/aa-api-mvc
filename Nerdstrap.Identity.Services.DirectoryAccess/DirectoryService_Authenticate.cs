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
        public virtual AuthenticateResponse Authenticate(AuthenticateRequest AuthenticateRequest)
        {
            // 1. validate input params
            if (AuthenticateRequest == null)
            {
                throw new ArgumentNullException(nameof(AuthenticateRequest));
            }
            if (string.IsNullOrWhiteSpace(AuthenticateRequest.UserId))
            {
                throw new ArgumentNullException(nameof(AuthenticateRequest.UserId));
            }
            if (string.IsNullOrWhiteSpace(AuthenticateRequest.Credentials))
            {
                throw new ArgumentNullException(nameof(AuthenticateRequest.Credentials));
            }

            var AuthenticateResponse = new AuthenticateResponse();
            try
            {
                // 2. get admin directory connection
                using (LdapConnection adminDirectoryConnection = GetAdminDirectoryConnectionRetry() as LdapConnection)
                {
                    // 3. search for user
                    // 4. deserialize user attributes
                    var userAttributes = GetUserAttributes(UsersDistinguishedName, AuthenticateRequest.UserId, adminDirectoryConnection);
                    if (userAttributes == null)
                    {
                        AuditLogger.Info(AuditLoggerLiterals.AUTHENTICATE_USER_DOES_NOT_EXIST);
                        AuthenticateResponse.AuthenticateResult = AuthenticateResultEnum.DoesNotExist;
                        return AuthenticateResponse;
                    }

                    // 5. get user directory connection
                    using (LdapConnection userDirectoryConnection = GetUserDirectoryConnectionRetry(userAttributes.DistinguishedName, AuthenticateRequest.Credentials) as LdapConnection)
                    {
                        if (userDirectoryConnection != null)
                        {
                            // 6. search for ldap groups
                            // 7. validate ldap groups
                            var ldapGroups = GetGroups(GroupsDistinguishedName, AuthenticateRequest.UserId, adminDirectoryConnection);
                            if (ValidateLdapGroups(ldapGroups) == false)
                            {
                                AuditLogger.Info(AuditLoggerLiterals.AUTHENTICATE_NOT_AUTHORIZED);
                                AuthenticateResponse.AuthenticateResult = AuthenticateResultEnum.NotAuthorized;
                                return AuthenticateResponse;
                            }

                            // 8. search for password policy
                            // 9. check password expiration
                            var passwordMaxAge = GetPasswordMaxAge(userAttributes.PasswordPolicyDistinguishedName, AuthenticateRequest.UserId, userDirectoryConnection);
                            if (GetPasswordExpired(userAttributes.PasswordChangedTime, passwordMaxAge) == true)
                            {
                                AuditLogger.Info(AuditLoggerLiterals.AUTHENTICATE_CURRENT_PASSWORD_EXPIRED);
                                AuthenticateResponse.AuthenticateResult = AuthenticateResultEnum.Expired;
                                return AuthenticateResponse;
                            }

                            AuditLogger.Info(AuditLoggerLiterals.AUTHENTICATE_BIND_SUCCESS);
                            AuthenticateResponse.AuthenticateResult = AuthenticateResultEnum.Allow;
                        }
                        else
                        {
                            AuditLogger.Info(AuditLoggerLiterals.AUTHENTICATE_BIND_FAILURE);
                            AuthenticateResponse.AuthenticateResult = AuthenticateResultEnum.Deny;
                        }
                    }
                }
            }
            catch (LdapException ldapException)
            {
                if (IsLdapExceptionInvalidPassword(ldapException))
                {
                    AuditLogger.Info(AuditLoggerLiterals.AUTHENTICATE_CURRENT_PASSWORD_INVALID);
                    Logger.Error(TraceLoggerLiterals.AUTHENTICATE_CREDENTIALS_EXCEPTION, ldapException);
                    AuthenticateResponse.AuthenticateResult = AuthenticateResultEnum.Error;
                }
                else
                {
                    AuditLogger.Info(AuditLoggerLiterals.AUTHENTICATE_UNKNOWN_ERROR);
                    Logger.Error(TraceLoggerLiterals.AUTHENTICATE_CREDENTIALS_EXCEPTION, ldapException);
                    AuthenticateResponse.AuthenticateResult = AuthenticateResultEnum.Error;
                }
            }
            catch (Exception genericException)
            {
                AuditLogger.Info(AuditLoggerLiterals.AUTHENTICATE_UNKNOWN_ERROR);
                Logger.Error(TraceLoggerLiterals.AUTHENTICATE_CREDENTIALS_EXCEPTION, genericException);
                AuthenticateResponse.AuthenticateResult = AuthenticateResultEnum.Error;
            }

            return AuthenticateResponse;
        }

        protected internal virtual bool ValidateLdapGroups(IEnumerable<string> ldapGroups)
        {
            var isValid = true;
            if (!AuthorizedGroupList.Contains(LdapLiterals.AUTHORIZED_GROUPS_ALL, StringComparer.OrdinalIgnoreCase))
            {
                isValid = ldapGroups.Intersect(AuthorizedGroupList, StringComparer.OrdinalIgnoreCase).Any();
            }
            return isValid;
        }
    }
}

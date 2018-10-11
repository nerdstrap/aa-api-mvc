using log4net;
using Nerdstrap.Identity.Services.DirectoryAccess.Constants;
using Nerdstrap.Identity.Services.DirectoryAccess.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Nerdstrap.Identity.Services.DirectoryAccess.Proxies
{
    public class LdapInterfaceClient : LdapInterface
    {
        #region Properties

        private static ILog _logger;
        public ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(typeof(DirectoryService))); }
            set { _logger = value; }
        }

        public string UsersDistinguishedName { get; set; }
        public string GroupsDistinguishedName { get; set; }
        public string PasswordPolicyDistinguishedName { get; set; }

        #endregion Properties

        #region ctor

        public LdapInterfaceClient(string usersDistinguishedName, string groupsDistinguishedName, string passwordPolicyDistinguishedName)
        {
            if (string.IsNullOrEmpty(usersDistinguishedName))
            {
                throw new ArgumentNullException(nameof(usersDistinguishedName));
            }
            if (string.IsNullOrEmpty(groupsDistinguishedName))
            {
                throw new ArgumentNullException(nameof(groupsDistinguishedName));
            }
            if (string.IsNullOrEmpty(passwordPolicyDistinguishedName))
            {
                throw new ArgumentNullException(nameof(passwordPolicyDistinguishedName));
            }

            UsersDistinguishedName = usersDistinguishedName;
            GroupsDistinguishedName = groupsDistinguishedName;
            PasswordPolicyDistinguishedName = passwordPolicyDistinguishedName;
        }

        #endregion ctor

        public DirectoryConnection GetDirectoryConnection(string[] servers, string userName, string password)
        {
            var fullyQualifiedDnsHostName = false;
            var connectionless = false;
            var ldapDirectoryIdentifier = new LdapDirectoryIdentifier(servers, fullyQualifiedDnsHostName, connectionless);
            var ldapConnection = new LdapConnection(ldapDirectoryIdentifier);
            ldapConnection.AuthType = AuthType.Basic;
            ldapConnection.SessionOptions.ProtocolVersion = 3;
            ldapConnection.SessionOptions.SecureSocketLayer = true;
            var networkCredential = new System.Net.NetworkCredential();
            networkCredential.UserName = userName;
            networkCredential.Password = password;
            ldapConnection.Credential = networkCredential;

            ldapConnection.Bind();

            return ldapConnection;
        }

        public SearchResponse SearchForUser(string userId, string[] attributeList, string usersDistinguishedName, DirectoryConnection directoryConnection)
        {
            string ldapFilter = string.Format(LdapLiterals.USER_SEARCH_FILTER_FORMAT_STRING, userId);
            var searchRequest = new SearchRequest(usersDistinguishedName, ldapFilter, SearchScope.Subtree, attributeList);

            return directoryConnection.SendRequest(searchRequest) as SearchResponse;
        }

        public SearchResponse SearchForPasswordPolicy(string userId, string[] attributeList, string passwordPolicyDistinguishedName, DirectoryConnection adminDirectoryConnection)
        {
            string ldapFilter = LdapLiterals.PASSWORD_POLICY_SEARCH_FILTER;
            if (string.IsNullOrEmpty(passwordPolicyDistinguishedName))
            {
                passwordPolicyDistinguishedName = PasswordPolicyDistinguishedName;
            }
            var searchRequest = new SearchRequest(passwordPolicyDistinguishedName, ldapFilter, SearchScope.Subtree, attributeList);

            return adminDirectoryConnection.SendRequest(searchRequest) as SearchResponse;
        }

        public SearchResponse SearchForGroups(string groupDistinguishedName, string userId, DirectoryConnection adminDirectoryConnection)
        {
            List<string> userLdapGroups = new List<string>();
            string ldapFilter = string.Format(LdapLiterals.GROUPS_SEARCH_FILTER, userId);
            string[] attributeList = new string[] { LdapLiterals.GROUPS_SEARCH_ATTRIBUTE_LIST };
            var searchRequest = new SearchRequest(groupDistinguishedName, ldapFilter, SearchScope.Subtree, attributeList);

            return adminDirectoryConnection.SendRequest(searchRequest) as SearchResponse;
        }

        public ModifyResponse SendUnlockGdsModifyRequest(string userDistinguishedName, DirectoryConnection adminDirectoryConnection)
        {
            var modifyRequest = new ModifyRequest();
            modifyRequest.DistinguishedName = userDistinguishedName;

            var pwdAccountLockedTimeModification = new DirectoryAttributeModification();
            pwdAccountLockedTimeModification.Operation = DirectoryAttributeOperation.Delete;
            pwdAccountLockedTimeModification.Name = LdapLiterals.PWD_ACCOUNT_LOCKED_TIME_ATTRIBUTE_NAME;
            modifyRequest.Modifications.Add(pwdAccountLockedTimeModification);

            return adminDirectoryConnection.SendRequest(modifyRequest) as ModifyResponse;
        }

        public ModifyResponse SendUserPasswordModifyRequest(string userDistinguishedName, string directoryAttributeValue, DirectoryConnection adminLdapConnection)
        {
            var modifyRequest = new ModifyRequest();
            modifyRequest.DistinguishedName = userDistinguishedName;

            var directoryAttributeModification = new DirectoryAttributeModification();
            directoryAttributeModification.Operation = DirectoryAttributeOperation.Replace;
            directoryAttributeModification.Name = LdapLiterals.USER_PASSWORD_ATTRIBUTE_NAME;
            directoryAttributeModification.Add(directoryAttributeValue);
            modifyRequest.Modifications.Add(directoryAttributeModification);

            return adminLdapConnection.SendRequest(modifyRequest) as ModifyResponse;
        }

        public ModifyResponse SendPasswordPublishModifyRequest(string userDistinguishedName, DirectoryConnection adminLdapConnection, string passwordPublishStatusAttributeValue)
        {
            var modifyRequest = new ModifyRequest();
            modifyRequest.DistinguishedName = userDistinguishedName;

            var passwordPublishStatusDirectoryAttributeModification = new DirectoryAttributeModification();
            passwordPublishStatusDirectoryAttributeModification.Operation = DirectoryAttributeOperation.Replace;
            passwordPublishStatusDirectoryAttributeModification.Name = LdapLiterals.PASSWORD_PUBLISH_STATUS_ATTRIBUTE_NAME;
            passwordPublishStatusDirectoryAttributeModification.Add(passwordPublishStatusAttributeValue);
            modifyRequest.Modifications.Add(passwordPublishStatusDirectoryAttributeModification);

            var passwordPublishDirectoryAttributeModification = new DirectoryAttributeModification();
            passwordPublishDirectoryAttributeModification.Operation = DirectoryAttributeOperation.Replace;
            passwordPublishDirectoryAttributeModification.Name = LdapLiterals.PASSWORD_PUBLISH_STATUS_ATTRIBUTE_NAME;
            passwordPublishDirectoryAttributeModification.Add(LdapLiterals.PASSWORD_PUBLISH_ATTRIBUTE_VALUE);
            modifyRequest.Modifications.Add(passwordPublishDirectoryAttributeModification);

            return adminLdapConnection.SendRequest(modifyRequest) as ModifyResponse;
        }

        public ModifyResponse SendUnlockPublishModifyRequest(string userDistinguishedName, DirectoryConnection adminLdapConnection, string unlockPublishStatusAttributeValue)
        {
            var modifyRequest = new ModifyRequest();
            modifyRequest.DistinguishedName = userDistinguishedName;

            var unlockPublishStatusDirectoryAttributeModification = new DirectoryAttributeModification();
            unlockPublishStatusDirectoryAttributeModification.Operation = DirectoryAttributeOperation.Replace;
            unlockPublishStatusDirectoryAttributeModification.Name = LdapLiterals.UNLOCK_PUBLISH_STATUS_ATTRIBUTE_NAME;
            unlockPublishStatusDirectoryAttributeModification.Add(unlockPublishStatusAttributeValue);
            modifyRequest.Modifications.Add(unlockPublishStatusDirectoryAttributeModification);

            var unlockPublishDirectoryAttributeModification = new DirectoryAttributeModification();
            unlockPublishDirectoryAttributeModification.Operation = DirectoryAttributeOperation.Replace;
            unlockPublishDirectoryAttributeModification.Name = LdapLiterals.UNLOCK_PUBLISH_ATTRIBUTE_NAME;
            unlockPublishDirectoryAttributeModification.Add(LdapLiterals.UNLOCK_PUBLISH_ATTRIBUTE_VALUE);
            modifyRequest.Modifications.Add(unlockPublishDirectoryAttributeModification);

            return adminLdapConnection.SendRequest(modifyRequest) as ModifyResponse;
        }

        public ExtendedResponse SendResetPasswordExtendedRequest(byte[] resetPasswordRequestValue, DirectoryConnection userDirectoryConnection)
        {
            var extendedRequest = new ExtendedRequest(LdapLiterals.RESET_PASSWORD_REQUEST_NAME);
            extendedRequest.RequestValue = resetPasswordRequestValue;

            return userDirectoryConnection.SendRequest(extendedRequest) as ExtendedResponse;
        }
    }
}

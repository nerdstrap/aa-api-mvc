using log4net;
using Nerdstrap.Identity.Services.DirectoryAccess.Constants;
using Nerdstrap.Identity.Services.DirectoryAccess.Models;
using Nerdstrap.Identity.Services.DirectoryAccess.Proxies;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Nerdstrap.Identity.Services.DirectoryAccess
{
    public partial class DirectoryService : IDirectoryService
    {
        #region Properties

        private static ILog _logger;
        public ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(typeof(DirectoryService))); }
            set { _logger = value; }
        }

        private ILog _auditLogger;
        public ILog AuditLogger
        {
            get { return _auditLogger ?? (_auditLogger = LogManager.GetLogger("AuditLogger")); }
            set { _auditLogger = value; }
        }

        public string DirectoryServers { get; set; }
        internal string[] DirectoryServerArray { get; set; }
        public string AdminUserDistinguishedName { get; set; }
        public string AdminUserCredentials { get; set; }
        public string UsersDistinguishedName { get; set; }
        public string GroupsDistinguishedName { get; set; }
        public string PasswordPolicyDistinguishedName { get; set; }
        public string AuthorizedGroups { get; set; }
        internal List<string> AuthorizedGroupList { get; set; }
        public int ConnectionAttempts { get; set; }
        public LdapInterfaceClient Client { get; set; }

        #endregion Properties

        #region ctor

        public DirectoryService(string directoryServers, string adminUserDistinguishedName, string adminUserCredentials, string usersDistinguishedName, string groupsDistinguishedName, string passwordPolicyDistinguishedName, string authorizedGroups, int connectionAttempts)
        {
            if (string.IsNullOrEmpty(directoryServers))
            {
                throw new ArgumentNullException(nameof(directoryServers));
            }
            if (string.IsNullOrEmpty(adminUserDistinguishedName))
            {
                throw new ArgumentNullException(nameof(adminUserDistinguishedName));
            }
            if (string.IsNullOrEmpty(adminUserCredentials))
            {
                throw new ArgumentNullException(nameof(adminUserCredentials));
            }
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
            if (string.IsNullOrEmpty(authorizedGroups))
            {
                authorizedGroups = LdapLiterals.AUTHORIZED_GROUPS_ALL;
            }
            if (connectionAttempts < 1)
            {
                connectionAttempts = LdapLiterals.CONNECTION_ATTEMPTS;
            }

            DirectoryServers = directoryServers;
            DirectoryServerArray = DirectoryServers.Split(',');
            AdminUserDistinguishedName = adminUserDistinguishedName;
            AdminUserCredentials = adminUserCredentials;
            UsersDistinguishedName = usersDistinguishedName;
            GroupsDistinguishedName = groupsDistinguishedName;
            PasswordPolicyDistinguishedName = passwordPolicyDistinguishedName;
            AuthorizedGroups = authorizedGroups;
            AuthorizedGroupList = authorizedGroups.Split(',').ToList();
        }

        #endregion ctor

        #region Search Methods

        protected internal virtual DirectoryConnection GetAdminDirectoryConnectionRetry()
        {
            LdapConnection ldapConnection = null;

            for (int attempts = 0; attempts < LdapLiterals.CONNECTION_ATTEMPTS; attempts++)
            {
                try
                {
                    ldapConnection = Client.GetDirectoryConnection(DirectoryServerArray, AdminUserDistinguishedName, AdminUserCredentials) as LdapConnection;
                    ldapConnection.Bind();
                    break;
                }
                catch (LdapException ldapException)
                {
                    Logger.Fatal("", ldapException);
                    if (attempts >= LdapLiterals.CONNECTION_ATTEMPTS - 1)
                    {
                        throw;
                    }
                }
                catch (Exception genericException)
                {
                    Logger.Fatal("", genericException);
                    if (attempts >= LdapLiterals.CONNECTION_ATTEMPTS - 1)
                    {
                        throw;
                    }
                }
            }

            return ldapConnection;
        }

        protected internal virtual DirectoryConnection GetUserDirectoryConnectionRetry(string userName, string password)
        {
            LdapConnection ldapConnection = null;

            for (int attempts = 0; attempts < LdapLiterals.CONNECTION_ATTEMPTS; attempts++)
            {
                try
                {
                    ldapConnection = Client.GetDirectoryConnection(DirectoryServerArray, userName, password) as LdapConnection;
                    ldapConnection.Bind();
                    break;
                }
                catch (LdapException ldapException)
                {
                    Logger.Fatal("", ldapException);
                    if (attempts >= LdapLiterals.CONNECTION_ATTEMPTS - 1)
                    {
                        throw;
                    }
                }
                catch (Exception genericException)
                {
                    Logger.Fatal("", genericException);
                    if (attempts >= LdapLiterals.CONNECTION_ATTEMPTS - 1)
                    {
                        throw;
                    }
                }
            }

            return ldapConnection;
        }

        protected internal virtual int GetUserCount(string usersDistinguishedName, string userId, DirectoryConnection directoryConnection)
        {
            var userCount = 0;
            string[] returnAttributes = new string[] { LdapLiterals.USER_COUNT_SEARCH_ATTRIBUTE };
            var searchResponse = Client.SearchForUser(userId, returnAttributes, usersDistinguishedName, directoryConnection);
            if (searchResponse != null && searchResponse.ResultCode == ResultCode.Success)
            {
                if (searchResponse.Entries != null)
                {
                    userCount = searchResponse.Entries.Count;
                }
            }
            return userCount;
        }

        protected internal virtual UserAttributes GetUserAttributes(string usersDistinguishedName, string userId, DirectoryConnection directoryConnection)
        {
            UserAttributes userAttributes = null;
            string[] returnAttributes = new string[] { LdapLiterals.USER_SEARCH_ATTRIBUTE_0, LdapLiterals.USER_SEARCH_ATTRIBUTE_1 };
            var searchResponse = Client.SearchForUser(userId, returnAttributes, usersDistinguishedName, directoryConnection);
            if (searchResponse != null && searchResponse.ResultCode == ResultCode.Success)
            {
                if (searchResponse.Entries != null && searchResponse.Entries.Count > 0)
                {
                    var searchResultEntry = searchResponse.Entries[0];
                    userAttributes = DeserializeUserAttributes(searchResultEntry.Attributes);
                    if (string.IsNullOrEmpty(userAttributes.DisplayName))
                    {
                        userAttributes.DisplayName = GetDisplayName(userAttributes);
                    }
                    userAttributes.DistinguishedName = searchResultEntry.DistinguishedName;
                    userAttributes.ExternalPartner = userAttributes.DistinguishedName.IndexOf(LdapLiterals.DISTINGUISHED_NAME_PARTNERS, StringComparison.CurrentCultureIgnoreCase) >= 0;
                }
            }

            return userAttributes;
        }

        protected internal virtual double GetPasswordMaxAge(string passwordDistinguishedName, string userId, DirectoryConnection directoryConnection)
        {
            double maxAge = 0;
            string[] attrList = new string[] { LdapLiterals.PASSWORD_POLICY_SEARCH_ATTRIBUTE_0, LdapLiterals.PASSWORD_POLICY_SEARCH_ATTRIBUTE_1 };
            var searchResponse = Client.SearchForPasswordPolicy(userId, attrList, passwordDistinguishedName, directoryConnection);
            if (searchResponse != null && searchResponse.Entries != null && searchResponse.Entries.Count > 0)
            {
                maxAge = DeserializePasswordPolicyAttributes(searchResponse.Entries[0]);

            }
            return maxAge;
        }

        protected internal virtual List<string> GetGroups(string groupDistinguishedName, string userId, DirectoryConnection directoryConnection)
        {
            List<string> ldapGroups = new List<string>();
            var searchResponse = Client.SearchForGroups(groupDistinguishedName, userId, directoryConnection);
            if (searchResponse != null && searchResponse.Entries != null && searchResponse.Entries.Count > 0)
            {
                foreach (SearchResultEntry entry in searchResponse.Entries)
                {
                    ldapGroups.Add(DeserializeGroupAttributes(entry.DistinguishedName));
                }
            }
            return ldapGroups;
        }

        #endregion Search Methods

        #region Utility Methods

        protected internal virtual UserAttributes DeserializeUserAttributes(SearchResultAttributeCollection searchResultAttributes)
        {
            var userAttributes = new UserAttributes();
            foreach (DirectoryAttribute userDirectoryAttribute in searchResultAttributes.Values)
            {
                if (string.Compare(userDirectoryAttribute.Name, LdapLiterals.ATTRIBUTE_LAST_NAME, true) == 0 && userDirectoryAttribute.Count > 0)
                {
                    userAttributes.LastName = (string)userDirectoryAttribute[0];
                    continue;
                }

                if (string.Compare(userDirectoryAttribute.Name, LdapLiterals.ATTRIBUTE_FIRST_NAME, true) == 0 && userDirectoryAttribute.Count > 0)
                {
                    userAttributes.FirstName = (string)userDirectoryAttribute[0];
                    continue;
                }

                if (string.Compare(userDirectoryAttribute.Name, LdapLiterals.ATTRIBUTE_MIDDLE_NAME, true) == 0 && userDirectoryAttribute.Count > 0)
                {
                    userAttributes.MiddleName = (string)userDirectoryAttribute[0];
                    continue;
                }

                if (string.Compare(userDirectoryAttribute.Name, LdapLiterals.ATTRIBUTE_DISPLAY_NAME, true) == 0 && userDirectoryAttribute.Count > 0)
                {
                    userAttributes.DisplayName = (string)userDirectoryAttribute[0];
                    continue;
                }

                if (string.Compare(userDirectoryAttribute.Name, LdapLiterals.ATTRIBUTE_EMAIL_ADDRESS, true) == 0 && userDirectoryAttribute.Count > 0)
                {
                    userAttributes.EmailAddress = (string)userDirectoryAttribute[0];
                    continue;
                }

                if (string.Compare(userDirectoryAttribute.Name, LdapLiterals.ATTRIBUTE_PASSWORD_CHANGED_TIME, true) == 0 && userDirectoryAttribute.Count > 0)
                {
                    DateTime timeStamp;
                    var format = LdapLiterals.PASSWORD_CHANGED_TIMESTAMP_FORMAT_STRING;
                    if (DateTime.TryParseExact((string)userDirectoryAttribute[0], format, null, DateTimeStyles.None, out timeStamp) == true)
                    {
                        userAttributes.PasswordChangedTime = timeStamp;
                    }
                    continue;
                }

                if (string.Compare(userDirectoryAttribute.Name, LdapLiterals.ATTRIBUTE_PASSWORD_POLICY_DISTINGUISHED_NAME, true) == 0 && userDirectoryAttribute.Count > 0)
                {
                    userAttributes.PasswordPolicyDistinguishedName = (string)userDirectoryAttribute[0];
                    continue;
                }

                if (string.Compare(userDirectoryAttribute.Name, LdapLiterals.PWD_ACCOUNT_LOCKED_TIME_ATTRIBUTE_NAME, true) == 0 && userDirectoryAttribute.Count > 0)
                {
                    userAttributes.Locked = true;
                    continue;
                }

                //if (string.Compare(userDirectoryAttribute.Name, DirectoryServices.PasswordPublishStatusAttributeName, true) == 0 && userDirectoryAttribute.Count > 0)
                //{
                //    userAttributes.PasswordPublishEventLogItems = DeserializeDirectoryEventLogItemsAttribute(userDirectoryAttribute);
                //    continue;
                //}

                //if (string.Compare(userDirectoryAttribute.Name, DirectoryServices.UnlockPublishStatusAttributeName, true) == 0 && userDirectoryAttribute.Count > 0)
                //{
                //    userAttributes.UnlockCredentialsEventLogItems = DeserializeDirectoryEventLogItemsAttribute(userDirectoryAttribute);
                //    continue;
                //}
            }

            return userAttributes;
        }

        protected internal virtual double DeserializePasswordPolicyAttributes(SearchResultEntry searchResultEntry)
        {
            double maxAge = 0;
            foreach (DirectoryAttribute directoryAttribute in searchResultEntry.Attributes.Values)
            {
                if (string.Compare(directoryAttribute.Name, LdapLiterals.ATTRIBUTE_PASSWORD_MAX_AGE, true) == 0 && directoryAttribute.Count > 0)
                {
                    double.TryParse((string)directoryAttribute[0], out maxAge);
                    break;
                }
            }
            return maxAge;
        }

        protected internal virtual string DeserializeGroupAttributes(string distinguishedName)
        {
            string ldapGroup = "";
            ldapGroup = distinguishedName.Split(LdapLiterals.GROUP_SPLIT_DELIMITER.ToCharArray()).ToList().Find(s => s.StartsWith(LdapLiterals.GROUP_KEY));
            ldapGroup = ldapGroup.Substring(3, ldapGroup.Length - 3);
            return ldapGroup;
        }

        protected internal virtual string GetDisplayName(UserAttributes userAttributes)
        {
            if (string.IsNullOrEmpty(userAttributes.MiddleName))
            {
                return userAttributes.FirstName + " " + userAttributes.LastName;
            }
            return userAttributes.FirstName + " " + userAttributes.MiddleName + ". " + userAttributes.LastName;
        }

        protected internal virtual bool GetPasswordExpired(DateTime? passwordChangedTime, double passwordMaxAge)
        {
            if (passwordChangedTime.HasValue && passwordMaxAge != 0)
            {
                return passwordChangedTime.Value.AddSeconds(passwordMaxAge) < DateTime.Now;
            }
            return false;
        }

        protected internal virtual byte[] GetEncodedPasswordPublishStatusAttributeValue()
        {
            return Encoding.UTF8.GetBytes(string.Format(LdapLiterals.PASSWORD_PUBLISH_INIT_LOG_FORMAT_STRING, DateTime.UtcNow.ToString(LdapLiterals.TIMESTAMP_FORMAT_STRING)));
        }

        protected internal virtual byte[] GetEncodedUnlockPublishStatusAttributeValue()
        {
            return Encoding.UTF8.GetBytes(string.Format(LdapLiterals.UNLOCK_PUBLISH_INIT_LOG_FORMAT_STRING, DateTime.UtcNow.ToString(LdapLiterals.TIMESTAMP_FORMAT_STRING)));
        }

        protected internal virtual bool IsLdapExceptionInvalidPassword(LdapException ldapException)
        {
            return ldapException.ErrorCode == LdapLiterals.INVALID_CREDENTIALS_ERROR_CODE;
        }

        #endregion Utility Methods
    }
}

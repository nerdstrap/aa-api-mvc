namespace Nerdstrap.Identity.Services.DirectoryAccess.Constants
{
    public static class LdapLiterals
    {
        public const string AUTHORIZED_GROUPS_ALL = "all";
        public const int CONNECTION_ATTEMPTS = 3;
        public const int INVALID_CREDENTIALS_ERROR_CODE = 49;

        public const string RESET_PASSWORD_REQUEST_NAME = "1.3.6.1.4.1.4203.1.11.1";
        public const string USER_SEARCH_FILTER_FORMAT_STRING = "(&(!(ou:dn:=automatons))(objectClass=inetOrgPerson)(uid={0}))";
        public const string PASSWORD_POLICY_SEARCH_FILTER = "(objectClass=*)";
        public const string GROUPS_SEARCH_FILTER = "(&(objectclass=groupofuniquenames)(uniquemember=uid={0},ou=Employees,ou=Users,dc=global,dc=aep,dc=com))";
        public const string USER_COUNT_SEARCH_ATTRIBUTE = "1.1";
        public const string USER_SEARCH_ATTRIBUTE_0 = "*";
        public const string USER_SEARCH_ATTRIBUTE_1 = "+";
        public const string PASSWORD_POLICY_SEARCH_ATTRIBUTE_0 = "*";
        public const string PASSWORD_POLICY_SEARCH_ATTRIBUTE_1 = "+";
        public const string GROUPS_SEARCH_ATTRIBUTE_LIST = "1.1";
        public const string GROUP_KEY = "cn=";
        public const string GROUP_SPLIT_DELIMITER = ",";

        public const string PASSWORD_PUBLISH_ATTRIBUTE_NAME = "AEPPWDPUB";
        public const string UNLOCK_PUBLISH_ATTRIBUTE_NAME = "AEPUNLOCKPUB";
        public const string PASSWORD_PUBLISH_STATUS_ATTRIBUTE_NAME = "AEPPWDPUBSTATUS";
        public const string UNLOCK_PUBLISH_STATUS_ATTRIBUTE_NAME = "AEPUNLOCKPUBSTATUS";
        public const string PWD_ACCOUNT_LOCKED_TIME_ATTRIBUTE_NAME = "pwdAccountLockedTime";
        public const string USER_PASSWORD_ATTRIBUTE_NAME = "userPassword";
        public const string UNLOCK_PUBLISHSTATUSATTRIBUTEVALUE = "Unlock publish init";
        public const string PASSWORD_PUBLISH_STATUS_ATTRIBUTE_VALUE = "Password publish init";
        public const string PASSWORD_PUBLISH_ATTRIBUTE_VALUE = "TRUE";
        public const string UNLOCK_PUBLISH_ATTRIBUTE_VALUE = "TRUE";

        public const string PASSWORD_PUBLISH_INIT_LOG_FORMAT_STRING = "{0}|IdentityManager|Password publish init|SUCCESS";
        public const string UNLOCK_PUBLISH_INIT_LOG_FORMAT_STRING = "{0}|IdentityManager|Unlock publish init|SUCCESS";
        
        public const string TIMESTAMP_FORMAT_STRING = "yyyyMMddHHmmss\".\"fff\"Z\"";
        public const string LOCKED_TIMESTAMP_FORMAT_STRING = "yyyyMMddHHmmssZ";
        public const string PASSWORD_CHANGED_TIMESTAMP_FORMAT_STRING = "yyyyMMddHHmmssZ";

        public const string DISTINGUISHED_NAME_PARTNERS = "OU=Partners";

        public const string ATTRIBUTE_LAST_NAME = "sn";
        public const string ATTRIBUTE_FIRST_NAME = "givenName";
        public const string ATTRIBUTE_MIDDLE_NAME = "initials";
        public const string ATTRIBUTE_DISPLAY_NAME = "displayName";
        public const string ATTRIBUTE_EMAIL_ADDRESS = "mail";
        public const string ATTRIBUTE_PASSWORD_CHANGED_TIME = "pwdChangedTime";
        public const string ATTRIBUTE_PASSWORD_POLICY_DISTINGUISHED_NAME = "pwdPolicySubEntry";
        public const string ATTRIBUTE_ACCOUNT_LOCKED = "pwdAccountLockedTime";
        public const string ATTRIBUTE_PASSWORD_MAX_AGE = "pwdMaxAge";
    }
}

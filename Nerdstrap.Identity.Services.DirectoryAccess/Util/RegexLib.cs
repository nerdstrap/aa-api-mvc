using System.Text.RegularExpressions;

namespace Nerdstrap.Identity.Services.DirectoryAccess.Util
{
    public class RegexLib
    {
        public static bool IsInsufficientAccessRights(string extendedInfo)
        {
            string pattern = @"(INSUFF_ACCESS_RIGHT)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsWillNotPerform(string extendedInfo)
        {
            string pattern = @"(WILL_NOT_PERFORM)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsSystemUnavailable(string extendedInfo)
        {
            string pattern = @"(Can't contact LDAP server)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsSystemError(string extendedInfo)
        {
            string pattern = @"(Invalid credentials)|(No plaintext password available)|(No unicode password value provided)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsOracleHistory(string extendedInfo)
        {
            string pattern = @"(ORA-28007)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsOraclePwdContainsError(string extendedInfo)
        {
            string pattern = @"(ORA-20003)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsOracleDbUnavailableError(string extendedInfo)
        {
            string pattern = @"(ORA-01033)|(ORA-12505)|(ORA-12514)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsOracleDbNeedOnboardedError(string extendedInfo)
        {
            string pattern = @"(ORA-01017)|(ORA-01031)|(ORA-28001)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsOracleHubErrorWhitelist(string extendedInfo)
        {
            string pattern = @"^(Attempted update on)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsOracleError(string extendedInfo)
        {
            string pattern = @"(ORA-)[0-9]{1,}";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(extendedInfo);
        }

        public static bool IsLdapPasswordComplexityError(string errorMessage)
        {
            string pattern = @"(Password fails quality checking policy)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(errorMessage);
        }

        public static bool IsLdapPasswordHistoryError(string errorMessage)
        {
            string pattern = @"(Password is in history of old passwords)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(errorMessage);
        }

        public static bool IsLdapPasswordExistingValueError(string errorMessage)
        {
            string pattern = @"(Password is not being changed from existing value)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(errorMessage);
        }

        public static bool IsSuccess(string message)
        {
            string pattern = @"(SUCCESS)";
            Regex extendedInfoRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return extendedInfoRegex.IsMatch(message);
        }
    }
}

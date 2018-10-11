namespace Nerdstrap.Identity.Services.DirectoryAccess.Constants
{
    public static class AuditLoggerLiterals
    {
        public const string AUTHENTICATE_USER_DOES_NOT_EXIST = "Authenticate|Failure|User does not exist.";
        public const string AUTHENTICATE_NOT_AUTHORIZED = "Authenticate|Failure|User is not in whitelist.";
        public const string AUTHENTICATE_USER_LOCKED = "Authenticate|Failure|User is locked in GDS.";
        public const string AUTHENTICATE_CURRENT_PASSWORD_EXPIRED = "Authenticate|Failure|Current password expired in GDS.";
        public const string AUTHENTICATE_BIND_SUCCESS = "Authenticate|Success|User successfully authenticated with id and password.";
        public const string AUTHENTICATE_BIND_FAILURE = "Authenticate|Failure|User failed to authenticate with id and password.";
        public const string AUTHENTICATE_CURRENT_PASSWORD_INVALID = "Authenticate|Failure|User failed to authenticate with id and password.";
        public const string AUTHENTICATE_UNKNOWN_ERROR = "Authenticate|Failure|Unknown error.";

        public const string RESET_CREDENTIALS_FORMAT_STRING = "Password Reset|Success|Reset initiated and user {0} know their current password.";
        public const string RESET_CREDENTIALS_KNOWN_PASSWORD = "did";
        public const string RESET_CREDENTIALS_FORGOT_PASSWORD = "did not";
        public const string RESET_CREDENTIALS_USER_DOES_NOT_EXIST = "Password Reset|Failure|User does not exist.";
        public const string RESET_CREDENTIALS_NEW_PASSWORD_DISPLAY_NAME_VIOLATION = "Password Reset|Failure|New password contains display name.";
        public const string RESET_CREDENTIALS_SET_TEMP_PASSWORD_SUCCESS = "Password Reset|Success|Current password set to temporary value.";
        public const string RESET_CREDENTIALS_SET_TEMP_PASSWORD_FAILURE = "Password Reset|Failure|Current password not set to temporary value.";
        public const string RESET_CREDENTIALS_CURRENT_PASSWORD_INVALID = "Password Reset|Failure|Current password not specified.";
        public const string RESET_CREDENTIALS_NEW_PASSWORD_IDENTICAL_VIOLATION = "Password Reset|Failure|New password matches current password.";
        public const string RESET_CREDENTIALS_PASSWORD_RESET_SUCCESS = "Password Reset|Success|Successfully set new password.";
        public const string RESET_CREDENTIALS_PASSWORD_RESET_FAILURE = "Password Reset|Failure|Failed to set new password.";
        public const string RESET_CREDENTIALS_PASSWORD_SPRAY_SUCCESS = "Password Reset|Success|Password spray initiated.";
        public const string RESET_CREDENTIALS_PASSWORD_SPRAY_FAILURE = "Password Reset|Failure|Password spray failed.";
        public const string RESET_CREDENTIALS_NEW_PASSWORD_HISTORY_VIOLATION = "Password Reset|Failure|New password in history of used passwords.";
        public const string RESET_CREDENTIALS_NEW_PASSWORD_COMPLEXITY_VIOLATION = "Password Reset|Failure|New password failed complexity challenge.";
        public const string RESET_CREDENTIALS_UNKNOWN_ERROR = "Password Reset|Failure|Unknown error.";
    }
}

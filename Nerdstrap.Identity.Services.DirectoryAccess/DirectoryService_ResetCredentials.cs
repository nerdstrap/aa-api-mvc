using Nerdstrap.Identity.Services.DirectoryAccess.Constants;
using Nerdstrap.Identity.Services.DirectoryAccess.Contracts;
using Nerdstrap.Identity.Services.DirectoryAccess.Enums;
using Nerdstrap.Identity.Services.DirectoryAccess.Models;
using Nerdstrap.Identity.Services.DirectoryAccess.Util;
using System;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace Nerdstrap.Identity.Services.DirectoryAccess
{
    public partial class DirectoryService : IDirectoryService
    {
        public virtual ResetCredentialsResponse ResetCredentials(ResetCredentialsRequest resetCredentialsRequest)
        {
            // 1. validate input params
            if (resetCredentialsRequest == null)
            {
                throw new ArgumentNullException(nameof(resetCredentialsRequest));
            }
            if (string.IsNullOrWhiteSpace(resetCredentialsRequest.UserId))
            {
                throw new ArgumentNullException(nameof(resetCredentialsRequest.UserId));
            }
            if (string.IsNullOrWhiteSpace(resetCredentialsRequest.NewCredentials))
            {
                throw new ArgumentNullException(nameof(resetCredentialsRequest.NewCredentials));
            }

            var resetUserCredentialsResponse = new ResetCredentialsResponse();
            try
            {
                bool currentCredentialsSpecified = string.IsNullOrWhiteSpace(resetCredentialsRequest.CurrentCredentials) ? false : true;

                // 2. get admin directory connection
                using (var adminDirectoryConnection = GetAdminDirectoryConnectionRetry() as LdapConnection)
                {
                    // 3. search for user
                    // 4. deserialize user attributes
                    UserAttributes userAttributes = GetUserAttributes(UsersDistinguishedName, resetCredentialsRequest.UserId, adminDirectoryConnection);
                    if (userAttributes == null)
                    {
                        AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_USER_DOES_NOT_EXIST);
                        resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.DoesNotExist;
                        return resetUserCredentialsResponse;
                    }
                    AuditLogger.Info(string.Format(AuditLoggerLiterals.RESET_CREDENTIALS_FORMAT_STRING, currentCredentialsSpecified ? AuditLoggerLiterals.RESET_CREDENTIALS_FORGOT_PASSWORD : AuditLoggerLiterals.RESET_CREDENTIALS_KNOWN_PASSWORD));

                    // 5. validate password policy
                    if (ValidateFullName(userAttributes.DisplayName, resetCredentialsRequest.NewCredentials) == false)
                    {
                        AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_NEW_PASSWORD_DISPLAY_NAME_VIOLATION);
                        resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.PolicyViolation;
                        return resetUserCredentialsResponse;
                    }

                    if (currentCredentialsSpecified != true)
                    {
                        // 6. get temporary password
                        var temporaryCredentials = GetTemporaryCredentials();

                        // 7. set current password to temporary value
                        var userPasswordModifyResponse = Client.SendUserPasswordModifyRequest(userAttributes.DistinguishedName, temporaryCredentials, adminDirectoryConnection);
                        if (userPasswordModifyResponse != null && userPasswordModifyResponse.ResultCode == ResultCode.Success)
                        {
                            AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_SET_TEMP_PASSWORD_SUCCESS);
                            resetCredentialsRequest.CurrentCredentials = temporaryCredentials;
                        }
                        else
                        {
                            AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_SET_TEMP_PASSWORD_FAILURE);
                            resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.Error;
                            return resetUserCredentialsResponse;
                        }
                    }

                    // 8. validate password policy
                    if (ValidateCurrentCredentials(resetCredentialsRequest.CurrentCredentials) == false)
                    {
                        AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_CURRENT_PASSWORD_INVALID);
                        resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.PolicyViolation;
                        return resetUserCredentialsResponse;
                    }

                    // 9. validate password policy
                    if (ValidateNewCredentials(resetCredentialsRequest.CurrentCredentials, resetCredentialsRequest.NewCredentials) == false)
                    {
                        AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_NEW_PASSWORD_IDENTICAL_VIOLATION);
                        resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.PolicyViolation;
                        return resetUserCredentialsResponse;
                    }

                    // 10. get user directory connection
                    using (LdapConnection userDirectoryConnection = GetUserDirectoryConnectionRetry(userAttributes.DistinguishedName, resetCredentialsRequest.CurrentCredentials) as LdapConnection)
                    {
                        // 11. encode new password
                        var resetPasswordRequestValue = BerEncodeExtendedRequestData(userAttributes.DistinguishedName, resetCredentialsRequest.NewCredentials, resetCredentialsRequest.CurrentCredentials);

                        // 12. set password
                        var resetPasswordExtendedResponse = Client.SendResetPasswordExtendedRequest(resetPasswordRequestValue, userDirectoryConnection);
                        if (resetPasswordExtendedResponse != null && resetPasswordExtendedResponse.ResultCode == ResultCode.Success)
                        {
                            AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_PASSWORD_RESET_SUCCESS);
                            resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.Success;

                            // 13. set password publish attribute
                            var passwordPublishStatusAttributeValue = LdapLiterals.PASSWORD_PUBLISH_STATUS_ATTRIBUTE_VALUE;
                            var sendPasswordPublishModifysponse = Client.SendPasswordPublishModifyRequest(userAttributes.DistinguishedName, adminDirectoryConnection, passwordPublishStatusAttributeValue);
                            if (sendPasswordPublishModifysponse != null && sendPasswordPublishModifysponse.ResultCode == ResultCode.Success)
                            {
                                AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_PASSWORD_SPRAY_SUCCESS);
                                resetUserCredentialsResponse.ReplicateCredentialsStatus = ReplicateCredentialsStatusEnum.Success;
                            }
                            else
                            {
                                AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_PASSWORD_SPRAY_FAILURE);
                                resetUserCredentialsResponse.ReplicateCredentialsStatus = ReplicateCredentialsStatusEnum.Error;
                            }
                        }
                        else
                        {
                            AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_PASSWORD_RESET_FAILURE);
                            resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.Fail;
                        }
                    }
                }
            }
            catch (DirectoryOperationException directoryOperationException)
            {
                var responseErrorMessage = GetDirectoryResponseErrorMessage(directoryOperationException);
                if (RegexLib.IsLdapPasswordHistoryError(responseErrorMessage))
                {
                    Logger.Error(TraceLoggerLiterals.RESET_CREDENTIALS_EXCEPTION, directoryOperationException);
                    AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_NEW_PASSWORD_HISTORY_VIOLATION);
                    resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.PolicyViolation;
                }
                else if (RegexLib.IsLdapPasswordComplexityError(responseErrorMessage))
                {
                    Logger.Error(TraceLoggerLiterals.RESET_CREDENTIALS_EXCEPTION, directoryOperationException);
                    AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_NEW_PASSWORD_COMPLEXITY_VIOLATION);
                    resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.PolicyViolation;
                }
                else
                {
                    Logger.Error(TraceLoggerLiterals.RESET_CREDENTIALS_EXCEPTION, directoryOperationException);
                    AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_UNKNOWN_ERROR);
                    resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.Error;
                }
            }
            catch (LdapException ldapException)
            {
                if (IsLdapExceptionInvalidPassword(ldapException))
                {
                    Logger.Error(TraceLoggerLiterals.RESET_CREDENTIALS_EXCEPTION, ldapException);
                    AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_CURRENT_PASSWORD_INVALID);
                    resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.Fail;
                }
                else
                {
                    Logger.Error(TraceLoggerLiterals.RESET_CREDENTIALS_EXCEPTION, ldapException);
                    AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_UNKNOWN_ERROR);
                    resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.Error;
                }
            }
            catch (Exception genericException)
            {
                Logger.Error(TraceLoggerLiterals.RESET_CREDENTIALS_EXCEPTION, genericException);
                AuditLogger.Info(AuditLoggerLiterals.RESET_CREDENTIALS_UNKNOWN_ERROR);
                resetUserCredentialsResponse.ResetCredentialsResult = ResetCredentialsResultEnum.Error;
            }

            return resetUserCredentialsResponse;
        }

        protected internal virtual string GetTemporaryCredentials()
        {
            Random seed = new Random();
            string temporaryPassword = "__temp" + seed.Next(0, 1000000).ToString("D6");
            return temporaryPassword;
        }

        protected internal virtual string ByteToHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }

        protected internal virtual byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length / 2)
                .Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
                .ToArray();
        }

        protected internal virtual byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        protected internal virtual byte[] BerEncodeExtendedRequestData(string userDistinguishedName, string newPassword, string oldPassword)
        {
            var userDistinguishedNameBytes = Encoding.ASCII.GetBytes(userDistinguishedName);
            var oldPasswordBytes = Encoding.ASCII.GetBytes(oldPassword);
            var newPasswordBytes = Encoding.ASCII.GetBytes(newPassword);

            int sequenceLength = userDistinguishedNameBytes.Length + oldPasswordBytes.Length + newPasswordBytes.Length + 6;

            var hex = new StringBuilder();
            hex.Append("30");
            hex.Append(sequenceLength.ToString("X2"));
            hex.Append("80");
            hex.Append(userDistinguishedNameBytes.Length.ToString("X2"));
            hex.Append(ByteToHexString(userDistinguishedNameBytes));
            hex.Append("81");
            hex.Append(oldPasswordBytes.Length.ToString("X2"));
            hex.Append(ByteToHexString(oldPasswordBytes));
            hex.Append("82");
            hex.Append(newPasswordBytes.Length.ToString("X2"));
            hex.Append(ByteToHexString(newPasswordBytes));

            return StringToByteArray(hex.ToString());
        }

        protected internal virtual bool ValidateFullName(string fullName, string credentials)
        {
            var isValid = true;
            string testFullName = Regex.Replace(fullName, "[^a-zA-Z]", delegate (Match match)
            {
                string removeChar = match.ToString();
                return "";
            }, RegexOptions.IgnoreCase);

            for (int i = 0; i < (testFullName.Length - 2); i++)
            {
                if (credentials.IndexOf(testFullName.Substring(i, 3), StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        protected internal virtual bool ValidateCurrentCredentials(string currentCredentials)
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(currentCredentials))
            {
                isValid = false;
            }
            return isValid;
        }

        protected internal virtual bool ValidateNewCredentials(string currentCredentials, string newCredentials)
        {
            var isValid = true;
            if (string.Compare(currentCredentials, newCredentials, false) == 0)
            {
                isValid = false;
            }
            return isValid;
        }

        protected internal virtual string GetDirectoryResponseErrorMessage(DirectoryOperationException directoryOperationException)
        {
            if (directoryOperationException != null && directoryOperationException.Response != null)
            {
                return directoryOperationException.Response.ErrorMessage;
            }
            return string.Empty;
        }
    }
}

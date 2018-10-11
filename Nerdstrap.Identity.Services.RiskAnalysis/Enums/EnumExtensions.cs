using System;
using Nerdstrap.Identity.Services.RiskAnalysis.Constants;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Enums
{
    public static class EnumExtensions
    {
        #region ActionCodeEnum

        public static ActionCodeEnum Map(this ActionCodeEnum actionCode, Proxies.ActionCode proxyActionCode)
        {
            actionCode = ConvertProxyActionCodeToActionCodeEnum(proxyActionCode);
            return actionCode;
        }

        public static ActionCodeEnum ConvertProxyActionCodeToActionCodeEnum(Proxies.ActionCode proxyActionCode)
        {
            if (proxyActionCode == Proxies.ActionCode.NONE
                || proxyActionCode == Proxies.ActionCode.ALLOW)
            {
                return ActionCodeEnum.Allow;
            }
            if (proxyActionCode == Proxies.ActionCode.CHALLENGE)
            {
                return ActionCodeEnum.Challenge;
            }
            if (proxyActionCode == Proxies.ActionCode.REVIEW
                || proxyActionCode == Proxies.ActionCode.DELAY_AND_REVIEW
                || proxyActionCode == Proxies.ActionCode.STOP_AND_REVIEW
                || proxyActionCode == Proxies.ActionCode.ELEVATE_SECURITY
                || proxyActionCode == Proxies.ActionCode.REDIRECT_CHALLENGE
                || proxyActionCode == Proxies.ActionCode.REDIRECT_COLLECT
                || proxyActionCode == Proxies.ActionCode.COLLECT
                || proxyActionCode == Proxies.ActionCode.DENY
                || proxyActionCode == Proxies.ActionCode.BLOCK
                || proxyActionCode == Proxies.ActionCode.LOCKED)
            {
                return ActionCodeEnum.Deny;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(proxyActionCode));
        }

        #endregion ActionCodeEnum

        #region AuthStatusEnum

        public static AuthStatusEnum Map(this AuthStatusEnum authStatus, string proxyAuthStatus)
        {
            authStatus = ConvertProxyAuthStatusToAuthStatusEnum(proxyAuthStatus);
            return authStatus;
        }

        public static AuthStatusEnum ConvertProxyAuthStatusToAuthStatusEnum(string proxyAuthStatus)
        {
            if (string.Compare(proxyAuthStatus, ProxyLiterals.AUTH_STATUS_DENY, true) == 0)
            {
                return AuthStatusEnum.Deny;
            }
            if (string.Compare(proxyAuthStatus, ProxyLiterals.AUTH_STATUS_SUCCESS, true) == 0)
            {
                return AuthStatusEnum.Success;
            }
            if (string.Compare(proxyAuthStatus, ProxyLiterals.AUTH_STATUS_PENDING, true) == 0)
            {
                return AuthStatusEnum.Pending;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(proxyAuthStatus));
        }

        #endregion AuthStatusEnum

        #region AuthenticateResultEnum

        public static AuthenticateResultEnum Map(this AuthenticateResultEnum authenticateResult, string proxyAuthenticateResult)
        {
            authenticateResult = ConvertProxyAuthenticateResultToAuthenticateResultEnum(proxyAuthenticateResult);
            return authenticateResult;
        }

        public static AuthenticateResultEnum ConvertProxyAuthenticateResultToAuthenticateResultEnum(string proxyAuthenticateResult)
        {
            if (string.Compare(proxyAuthenticateResult, ProxyLiterals.AUTHENTICATE_RESULT_ALLOW, true) == 0)
            {
                return AuthenticateResultEnum.Allow;
            }
            if (string.Compare(proxyAuthenticateResult, ProxyLiterals.AUTHENTICATE_RESULT_CHALLENGE, true) == 0)
            {
                return AuthenticateResultEnum.Challenge;
            }
            if (string.Compare(proxyAuthenticateResult, ProxyLiterals.AUTHENTICATE_RESULT_DENY, true) == 0)
            {
                return AuthenticateResultEnum.Deny;
            }
            if (string.Compare(proxyAuthenticateResult, ProxyLiterals.AUTHENTICATE_RESULT_NONE, true) == 0)
            {
                return AuthenticateResultEnum.None;
            }
            if (string.Compare(proxyAuthenticateResult, ProxyLiterals.AUTHENTICATE_RESULT_REVIEW, true) == 0)
            {
                return AuthenticateResultEnum.Review;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(proxyAuthenticateResult));
        }

        #endregion AuthenticateResultEnum

        #region CallStatusEnum

        public static CallStatusEnum Map(this CallStatusEnum callStatus, string proxyCallStatus)
        {
            callStatus = ConvertProxyCallStatusToCallStatusEnum(proxyCallStatus);
            return callStatus;
        }

        public static CallStatusEnum ConvertProxyCallStatusToCallStatusEnum(string proxyCallStatus)
        {
            if (string.Compare(proxyCallStatus, ProxyLiterals.AUTH_STATUS_DENY, true) == 0)
            {
                return CallStatusEnum.OK;
            }
            if (string.Compare(proxyCallStatus, ProxyLiterals.AUTH_STATUS_SUCCESS, true) == 0)
            {
                return CallStatusEnum.SystemError;
            }
            if (string.Compare(proxyCallStatus, ProxyLiterals.AUTH_STATUS_PENDING, true) == 0)
            {
                return CallStatusEnum.InvaliidUserRequest;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(proxyCallStatus));
        }

        #endregion CallStatusEnum

        #region ChallengeResultEnum

        public static ChallengeResultEnum Map(this ChallengeResultEnum challengeResult, string proxyChallengeResult)
        {
            challengeResult = ConvertProxyChallengeResultToChallengeResultEnum(proxyChallengeResult);
            return challengeResult;
        }

        public static ChallengeResultEnum ConvertProxyChallengeResultToChallengeResultEnum(string proxyChallengeResult)
        {
            if (string.Compare(proxyChallengeResult, "fail", true) == 0)
            {
                return ChallengeResultEnum.Fail;
            }
            if (string.Compare(proxyChallengeResult, "pending", true) == 0)
            {
                return ChallengeResultEnum.Pending;
            }
            if (string.Compare(proxyChallengeResult, "success", true) == 0)
            {
                return ChallengeResultEnum.Success;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(proxyChallengeResult));
        }

        #endregion ChallengeResultEnum

        #region ContactTypeEnum

        public static ContactTypeEnum Map(this ContactTypeEnum contactType, string proxyContactType)
        {
            contactType = ConvertProxyContactTypeToContactTypeEnum(proxyContactType);
            return contactType;
        }

        public static ContactTypeEnum ConvertProxyContactTypeToContactTypeEnum(string proxyContactType)
        {
            if (string.Compare(proxyContactType, "email", true) == 0)
            {
                return ContactTypeEnum.Email;
            }
            if (string.Compare(proxyContactType, "sms", true) == 0)
            {
                return ContactTypeEnum.Sms;
            }
            if (string.Compare(proxyContactType, "phone", true) == 0)
            {
                return ContactTypeEnum.Phone;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(proxyContactType));
        }

        #endregion ContactTypeEnum

        #region CredentialTypeEnum

        public static CredentialTypeEnum Map(this CredentialTypeEnum credentialTypeEnum, Proxies.CredentialType credentialType, string genericCredentialType)
        {
            credentialTypeEnum = ConvertProxyCredentialTypeToCredentialTypeEnum(credentialType, genericCredentialType);
            return credentialTypeEnum;
        }

        public static CredentialTypeEnum ConvertProxyCredentialTypeToCredentialTypeEnum(Proxies.CredentialType credentialType, string genericCredentialType)
        {
            if (credentialType == Proxies.CredentialType.OOBEMAIL)
            {
                return CredentialTypeEnum.Email;
            }
            if (credentialType == Proxies.CredentialType.USER_DEFINED)
            {
                if (string.Compare(genericCredentialType, "securid", true) == 0)
                {
                    return CredentialTypeEnum.Token;
                }
                if (string.Compare(genericCredentialType, "oobsms", true) == 0)
                {
                    return CredentialTypeEnum.Sms;
                }
                //else
                throw new ArgumentOutOfRangeException(nameof(credentialType));
            }
            if (credentialType == Proxies.CredentialType.OOBPHONE)
            {
                return CredentialTypeEnum.Sms;
            }
            if (credentialType == Proxies.CredentialType.QUESTION)
            {
                return CredentialTypeEnum.Questions;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(credentialType));
        }

        #endregion CredentialTypeEnum

        #region DataTypeEnum

        public static Proxies.DataType ConvertDataTypeEnumToProxyDataType(DataTypeEnum dataType)
        {
            if (dataType == DataTypeEnum.Default || dataType == DataTypeEnum.String)
            {
                return Proxies.DataType.STRING;
            }
            if (dataType == DataTypeEnum.Boolean)
            {
                return Proxies.DataType.BOOLEAN;
            }
            if (dataType == DataTypeEnum.Date)
            {
                return Proxies.DataType.DATE;
            }
            if (dataType == DataTypeEnum.Double)
            {
                return Proxies.DataType.DOUBLE;
            }
            if (dataType == DataTypeEnum.Float)
            {
                return Proxies.DataType.FLOAT;
            }
            if (dataType == DataTypeEnum.Integer)
            {
                return Proxies.DataType.INTEGER;
            }
            if (dataType == DataTypeEnum.IP)
            {
                return Proxies.DataType.IP;
            }
            throw new ArgumentOutOfRangeException(nameof(dataType));
        }

        #endregion DataTypeEnum

        #region UserStatusEnum

        public static UserStatusEnum Map(this UserStatusEnum userStatus, Proxies.UserStatus proxyUserStatus, string genericCredentialType)
        {
            userStatus = ConvertProxyUserStatusToUserStatusEnum(proxyUserStatus);
            return userStatus;
        }

        public static UserStatusEnum ConvertProxyUserStatusToUserStatusEnum(Proxies.UserStatus proxyUserStatus)
        {
            if (proxyUserStatus == Proxies.UserStatus.DELETE)
            {
                return UserStatusEnum.Delete;
            }
            if (proxyUserStatus == Proxies.UserStatus.LOCKOUT)
            {
                return UserStatusEnum.Lockout;
            }
            if (proxyUserStatus == Proxies.UserStatus.NOTENROLLED)
            {
                return UserStatusEnum.NotEnrolled;
            }
            if (proxyUserStatus == Proxies.UserStatus.UNLOCKED)
            {
                return UserStatusEnum.UnLocked;
            }
            if (proxyUserStatus == Proxies.UserStatus.UNVERIFIED)
            {
                return UserStatusEnum.UnVerified;
            }
            if (proxyUserStatus == Proxies.UserStatus.VERIFIED)
            {
                return UserStatusEnum.Verified;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(proxyUserStatus));
        }

        public static Proxies.UserStatus Map(this Proxies.UserStatus proxyUserStatus, UserStatusEnum userStatus, string genericCredentialType)
        {
            proxyUserStatus = ConvertUserStatusEnumToProxyUserStatus(userStatus);
            return proxyUserStatus;
        }

        public static Proxies.UserStatus ConvertUserStatusEnumToProxyUserStatus(UserStatusEnum userStatus)
        {
            if (userStatus == UserStatusEnum.Delete)
            {
                return Proxies.UserStatus.DELETE;
            }
            if (userStatus == UserStatusEnum.Lockout)
            {
                return Proxies.UserStatus.LOCKOUT;
            }
            if (userStatus == UserStatusEnum.NotEnrolled)
            {
                return Proxies.UserStatus.NOTENROLLED;
            }
            if (userStatus == UserStatusEnum.UnLocked)
            {
                return Proxies.UserStatus.UNLOCKED;
            }
            if (userStatus == UserStatusEnum.UnVerified)
            {
                return Proxies.UserStatus.UNVERIFIED;
            }
            if (userStatus == UserStatusEnum.Verified)
            {
                return Proxies.UserStatus.VERIFIED;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(userStatus));
        }

        #endregion UserStatusEnum

        #region UserTypeEnum

        public static Proxies.WSUserType Map(this Proxies.WSUserType proxyUserType, UserTypeEnum userType)
        {
            proxyUserType = ConvertUserTypeEnumToProxyUserType(userType);
            return proxyUserType;
        }

        public static Proxies.WSUserType ConvertUserTypeEnumToProxyUserType(UserTypeEnum userType)
        {
            if (userType == UserTypeEnum.Persistent)
            {
                return Proxies.WSUserType.PERSISTENT;
            }
            if (userType == UserTypeEnum.NonPersistent)
            {
                return Proxies.WSUserType.NONPERSISTENT;
            }
            if (userType == UserTypeEnum.Bait)
            {
                return Proxies.WSUserType.BAIT;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(userType));
        }

        public static UserTypeEnum Map(this UserTypeEnum userType, Proxies.WSUserType proxyUserType)
        {
            userType = ConvertProxyUserTypeToUserTypeEnum(proxyUserType);
            return userType;
        }

        public static UserTypeEnum ConvertProxyUserTypeToUserTypeEnum(Proxies.WSUserType userType)
        {
            if (userType == Proxies.WSUserType.PERSISTENT)
            {
                return UserTypeEnum.Persistent;
            }
            if (userType == Proxies.WSUserType.NONPERSISTENT)
            {
                return UserTypeEnum.NonPersistent;
            }
            if (userType == Proxies.WSUserType.BAIT)
            {
                return UserTypeEnum.Bait;
            }
            //else
            throw new ArgumentOutOfRangeException(nameof(userType));
        }

        #endregion UserTypeEnum
    }
}

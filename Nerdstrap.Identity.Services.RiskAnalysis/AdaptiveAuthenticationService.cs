using System;
using log4net;
using Nerdstrap.Identity.Services.RiskAnalysis.Contracts;
using Nerdstrap.Identity.Services.RiskAnalysis.Enums;

namespace Nerdstrap.Identity.Services.RiskAnalysis
{
    public partial class AdaptiveAuthenticationService : IAdaptiveAuthenticationService
    {
        #region Properties

        private static ILog _logger;
        public ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(typeof(AdaptiveAuthenticationService))); }
            set { _logger = value; }
        }

        public Proxies.AdaptiveAuthenticationInterface Client { get; set; }
        public string OrgName { get; set; }
        public string ApiUserName { get; set; }
        public string ApiCredentials { get; set; }
        public int NumberOfQuestion { get; set; }

        #endregion Properties

        #region ctor

        public AdaptiveAuthenticationService(Proxies.AdaptiveAuthenticationInterface client, string orgName, string apiUserName, string apiCredentials, int numberOfQuestion)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            if (string.IsNullOrEmpty(orgName))
            {
                throw new ArgumentNullException(nameof(orgName));
            }
            if (string.IsNullOrEmpty(apiUserName))
            {
                throw new ArgumentNullException(nameof(apiUserName));
            }
            if (string.IsNullOrEmpty(apiCredentials))
            {
                throw new ArgumentNullException(nameof(apiCredentials));
            }
            if (numberOfQuestion == 0)
            {
                throw new ArgumentException(nameof(numberOfQuestion));
            }

            Client = client;
            OrgName = orgName;
            ApiUserName = apiUserName;
            ApiCredentials = apiCredentials;
            NumberOfQuestion = numberOfQuestion;
        }

        #endregion ctor

        #region proxy request

        internal virtual void SetDeviceRequest(Proxies.GenericRequest proxyRequest, BaseRequest request)
        {
            proxyRequest.deviceRequest = new Proxies.DeviceRequest
            {
                devicePrint = request.DeviceRequest.DevicePrint,
                deviceTokenCookie = request.DeviceRequest.DeviceTokenCookie,
                deviceTokenFSO = request.DeviceRequest.DeviceTokenFSO,
                httpAccept = request.DeviceRequest.HttpAccept,
                httpAcceptChars = request.DeviceRequest.HttpAcceptChars,
                httpAcceptEncoding = request.DeviceRequest.HttpAcceptEncoding,
                httpAcceptLanguage = request.DeviceRequest.HttpAcceptLanguage,
                httpReferrer = request.DeviceRequest.HttpReferrer,
                ipAddress = request.DeviceRequest.IpAddress,
                userAgent = request.DeviceRequest.UserAgent
            };
        }

        internal virtual void SetIdentificationData(Proxies.GenericRequest proxyRequest, BaseRequest request)
        {

            proxyRequest.identificationData = new Proxies.IdentificationData
            {
                orgName = OrgName,
                sessionId = request.SessionId,
                transactionId = request.TransactionId,
                userCountry = string.Empty,
                userLanguage = string.Empty,
                userName = request.UserId,
                userStatus = EnumExtensions.ConvertUserStatusEnumToProxyUserStatus(request.UserStatus),
                userStatusSpecified = true,
                userType = EnumExtensions.ConvertUserTypeEnumToProxyUserType(request.UserType),
                userTypeSpecified = true
            };
        }

        internal virtual void SetMessageHeader(Proxies.GenericRequest request, Proxies.RequestType requestType)
        {
            request.messageHeader = new Proxies.MessageHeader
            {
                apiType = Proxies.APIType.DIRECT_SOAP_API,
                apiTypeSpecified = true,
                requestType = requestType,
                requestTypeSpecified = true,
                version = Proxies.MessageVersion.Item70,
                versionSpecified = true
            };
        }

        internal virtual void SetSecurityHeader(Proxies.GenericRequest request)
        {
            request.securityHeader = new Proxies.SecurityHeader
            {
                callerId = ApiUserName,
                callerCredential = ApiCredentials,
                method = Proxies.AuthorizationMethod.PASSWORD
            };
        }

        #endregion proxy request

        #region proxy response

        internal virtual void GetDeviceResult(Proxies.GenericResponse proxyResponse, BaseResponse response)
        {
            if (proxyResponse.deviceResult != null)
            {
                if (proxyResponse.deviceResult.authenticationResult != null)
                {
                    response.DeviceAuthStatus = EnumExtensions.ConvertProxyAuthStatusToAuthStatusEnum(proxyResponse.deviceResult.authenticationResult.authStatusCode);

                    if (proxyResponse.deviceResult.authenticationResult.riskSpecified == true)
                    {
                        response.DeviceRiskScore = proxyResponse.deviceResult.authenticationResult.risk;
                    }
                }

                if (proxyResponse.deviceResult.callStatus != null)
                {
                    response.CallStatus = EnumExtensions.ConvertProxyCallStatusToCallStatusEnum(proxyResponse.deviceResult.callStatus.statusCode);
                    Logger.Debug(proxyResponse.deviceResult.callStatus.statusDescription);
                }

                if (proxyResponse.deviceResult.deviceData != null)
                {
                    response.DeviceToken = proxyResponse.deviceResult.deviceData.deviceTokenCookie;
                }
            }
        }

        internal virtual void GetIdentificationData(Proxies.GenericResponse proxyResponse, BaseResponse response)
        {
            if (proxyResponse.identificationData != null)
            {
                if (proxyResponse.identificationData.userStatusSpecified)
                {
                    response.UserStatus = EnumExtensions.ConvertProxyUserStatusToUserStatusEnum(proxyResponse.identificationData.userStatus);
                }
                if (proxyResponse.identificationData.userTypeSpecified)
                {
                    response.UserType = EnumExtensions.ConvertProxyUserTypeToUserTypeEnum(proxyResponse.identificationData.userType);
                }
                response.SessionId = proxyResponse.identificationData.sessionId;
                response.TransactionId = proxyResponse.identificationData.transactionId;
            }
        }

        internal virtual void GetMessageHeader(Proxies.GenericResponse proxyResponse, BaseResponse response)
        {
            if (proxyResponse.messageHeader != null)
            {
                response.RequestId = proxyResponse.messageHeader.requestId;
                response.TimeStamp = proxyResponse.messageHeader.timeStamp;
            }
        }

        internal virtual void GetStatusHeader(Proxies.GenericResponse proxyResponse, BaseResponse response)
        {
            if (proxyResponse.statusHeader != null)
            {
                if (proxyResponse.statusHeader.reasonCodeSpecified)
                {
                    response.ReasonCode = proxyResponse.statusHeader.reasonCode;
                }
                if (proxyResponse.statusHeader.statusCodeSpecified)
                {
                    response.StatusCode = proxyResponse.statusHeader.statusCode;
                }
            }
        }

        #endregion proxy response
    }
}

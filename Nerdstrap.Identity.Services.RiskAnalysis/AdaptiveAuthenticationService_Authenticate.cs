using Nerdstrap.Identity.Services.RiskAnalysis.Contracts;
using Nerdstrap.Identity.Services.RiskAnalysis.Enums;

namespace Nerdstrap.Identity.Services.RiskAnalysis
{
    public partial class AdaptiveAuthenticationService : IAdaptiveAuthenticationService
    {
        public virtual AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest)
        {
            var authenticateResponse = new AuthenticateResponse();

            #region proxy request

            Proxies.AuthenticateRequest proxyAuthenticateRequest = new Proxies.AuthenticateRequest();

            // action type list
            proxyAuthenticateRequest.actionTypeList = new Proxies.GenericActionTypeList
            {
                genericActionTypes = new Proxies.GenericActionType[]
                {
                    Proxies.GenericActionType.SET_USER_STATUS
                }
            };

            // device request
            SetDeviceRequest(proxyAuthenticateRequest, authenticateRequest);

            // identification data
            SetIdentificationData(proxyAuthenticateRequest, authenticateRequest);

            // message header
            SetMessageHeader(proxyAuthenticateRequest, Proxies.RequestType.AUTHENTICATE);

            // security header
            SetSecurityHeader(proxyAuthenticateRequest);

            // event data list
            proxyAuthenticateRequest.eventDataList = new Proxies.EventData[]
            {
                new Proxies.EventData
                {
                    eventTypeSpecified = true,
                    eventType = Proxies.EventType.SESSION_SIGNIN
                }
            };

            // credential data
            var credentialDataList = new Proxies.CredentialDataList();

            // challenge question answers
            if (authenticateRequest.CredentialType == CredentialTypeEnum.Questions)
            {
                credentialDataList.challengeQuestionData = new Proxies.ChallengeQuestionData();
                var challengeQuestionList = authenticateRequest.Credentials.Split(",".ToCharArray());
                if (challengeQuestionList != null && challengeQuestionList.Length > 0)
                {
                    Proxies.ChallengeQuestion[] payload = new Proxies.ChallengeQuestion[challengeQuestionList.Length];
                    if (payload != null && payload.Length > 0)
                    {
                        for (var i = 0; i < challengeQuestionList.Length; i++)
                        {
                            var challengeQuestionData = challengeQuestionList[i].Split("|".ToCharArray());
                            var questionId = challengeQuestionData[0];
                            var userAnswer = challengeQuestionData[1];
                            if (questionId != null && userAnswer != null)
                            {
                                payload[i] = new Proxies.ChallengeQuestion
                                {
                                    questionId = challengeQuestionData[0],
                                    userAnswer = challengeQuestionData[1]
                                };
                            }
                        }
                        credentialDataList.challengeQuestionData.payload = payload;
                    }
                }
            }

            // email
            if (authenticateRequest.CredentialType == CredentialTypeEnum.Email)
            {
                credentialDataList.oobEmailData = new Proxies.OobEmailData
                {
                    payload = new Proxies.OOBInfoRequestPayload
                    {
                        token = authenticateRequest.Credentials
                    }
                };
            }

            // sms
            if (authenticateRequest.CredentialType == CredentialTypeEnum.Sms)
            {
                credentialDataList.acspAuthenticationRequestData = new Proxies.AcspAuthenticationRequestData
                {
                    payload = new Proxies.OOBSMSAuthenticationRequest
                    {
                        otp = authenticateRequest.Credentials
                    }
                };
            }

            // secur id
            if (authenticateRequest.CredentialType == CredentialTypeEnum.Token)
            {
                credentialDataList.acspAuthenticationRequestData = new Proxies.AcspAuthenticationRequestData
                {
                    payload = new Proxies.SecurIdAuthenticationRequest
                    {
                        secIdToken = authenticateRequest.Credentials,
                        requstedAuthAction = Proxies.AuthAction.TOKEN_AUTH
                    }
                };
            }

            proxyAuthenticateRequest.credentialDataList = credentialDataList;

            // device management
            if (authenticateRequest.BindDevice)
            {
                proxyAuthenticateRequest.deviceManagementRequest = new Proxies.DeviceManagementRequestPayload
                {
                    actionTypeList = new Proxies.DeviceActionTypeList
                    {
                        deviceActionTypes = new Proxies.DeviceActionType[]
                        {
                            Proxies.DeviceActionType.UPDATE_DEVICE
                        }
                    },
                    deviceData = new Proxies.DeviceData
                    {
                        bindingType = Proxies.BindingType.HARD_BIND,
                        bindingTypeSpecified = true
                    }
                };
            }

            #endregion proxy request

            var proxyAuthenticateResponse = Client.authenticate(proxyAuthenticateRequest);

            #region proxy response

            if (proxyAuthenticateResponse != null)
            {
                GetDeviceResult(proxyAuthenticateResponse, authenticateResponse);
                GetIdentificationData(proxyAuthenticateResponse, authenticateResponse);
                GetMessageHeader(proxyAuthenticateResponse, authenticateResponse);
                GetStatusHeader(proxyAuthenticateResponse, authenticateResponse);

                // credential data
                if (proxyAuthenticateResponse.credentialAuthResultList != null)
                {
                    // challenge question answers
                    if (authenticateRequest.CredentialType == CredentialTypeEnum.Questions
                        && proxyAuthenticateResponse.credentialAuthResultList.challengeQuestionAuthResult != null
                        && proxyAuthenticateResponse.credentialAuthResultList.challengeQuestionAuthResult.payload != null
                        && proxyAuthenticateResponse.credentialAuthResultList.challengeQuestionAuthResult.payload.authenticationResult != null)
                    {
                        //ALLOW, CHALLENGE, DENY, NONE, REVIEW
                        authenticateResponse.AuthenticateResult = EnumExtensions.ConvertProxyAuthenticateResultToAuthenticateResultEnum(proxyAuthenticateResponse.credentialAuthResultList.challengeQuestionAuthResult.payload.authenticationResult.authStatusCode);
                    }

                    // email
                    if (authenticateRequest.CredentialType == CredentialTypeEnum.Email
                        && proxyAuthenticateResponse.credentialAuthResultList.oobEmailAuthResult != null
                        && proxyAuthenticateResponse.credentialAuthResultList.oobEmailAuthResult.payload != null
                        && proxyAuthenticateResponse.credentialAuthResultList.oobEmailAuthResult.payload.authenticationResult != null)
                    {
                        //ALLOW, CHALLENGE, DENY, NONE, REVIEW
                        authenticateResponse.AuthenticateResult = EnumExtensions.ConvertProxyAuthenticateResultToAuthenticateResultEnum(proxyAuthenticateResponse.credentialAuthResultList.oobEmailAuthResult.payload.authenticationResult.authStatusCode);
                    }

                    // sms or securid
                    if ((authenticateRequest.CredentialType == CredentialTypeEnum.Sms
                        || authenticateRequest.CredentialType == CredentialTypeEnum.Token)
                        && proxyAuthenticateResponse.credentialAuthResultList.acspAuthenticationResponseData != null
                        && proxyAuthenticateResponse.credentialAuthResultList.acspAuthenticationResponseData.callStatus != null)
                    {
                        //ALLOW, CHALLENGE, DENY, NONE, REVIEW
                        authenticateResponse.AuthenticateResult = EnumExtensions.ConvertProxyAuthenticateResultToAuthenticateResultEnum(proxyAuthenticateResponse.credentialAuthResultList.acspAuthenticationResponseData.callStatus.statusCode);
                    }
                }
            }

            #endregion proxy response

            return authenticateResponse;
        }
    }
}

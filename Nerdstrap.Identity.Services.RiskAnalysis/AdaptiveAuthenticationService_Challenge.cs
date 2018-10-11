using Nerdstrap.Identity.Services.RiskAnalysis.Constants;
using Nerdstrap.Identity.Services.RiskAnalysis.Contracts;
using Nerdstrap.Identity.Services.RiskAnalysis.Enums;
using Nerdstrap.Identity.Services.RiskAnalysis.Models;
using System.Linq;

namespace Nerdstrap.Identity.Services.RiskAnalysis
{
    public partial class AdaptiveAuthenticationService : IAdaptiveAuthenticationService
    {
        public virtual ChallengeResponse Challenge(ChallengeRequest challengeRequest)
        {
            var challengeResponse = new ChallengeResponse();

            #region proxy request                

            Proxies.ChallengeRequest proxyChallengeRequest = new Proxies.ChallengeRequest();

            // action type list

            // device request
            SetDeviceRequest(proxyChallengeRequest, challengeRequest);

            // identification data
            SetIdentificationData(proxyChallengeRequest, challengeRequest);

            // message header
            SetMessageHeader(proxyChallengeRequest, Proxies.RequestType.CHALLENGE);

            // security header
            SetSecurityHeader(proxyChallengeRequest);

            // event data
            proxyChallengeRequest.eventDataList = new Proxies.EventData[]
            {
                new Proxies.EventData
                {
                    eventType = Proxies.EventType.SESSION_SIGNIN,
                    eventTypeSpecified = true
                }
            };

            // credential challenge request
            var credentialChallengeRequestList = new Proxies.CredentialChallengeRequestList();

            // challenge question answers
            if (challengeRequest.CredentialType == CredentialTypeEnum.Questions)
            {
                credentialChallengeRequestList.challengeQuestionChallengeRequest = new Proxies.ChallengeQuestionChallengeRequest
                {
                    payload = new Proxies.ChallengeQuestionChallengeRequestPayload
                    {
                        numberOfQuestion = NumberOfQuestion,
                        numberOfQuestionSpecified = true
                    }
                };
            }

            // email
            if (challengeRequest.CredentialType == CredentialTypeEnum.Email)
            {
                credentialChallengeRequestList.oobEmailChallengeRequest = new Proxies.OobEmailChallengeRequest
                {
                    payload = new Proxies.OOBEmailChallengeRequestPayload
                    {
                        emailInfo = new Proxies.EmailInfo
                        {
                            address = challengeRequest.ContactInfo,
                            label = challengeRequest.Label
                        },
                        noOp = false,
                        noOpSpecified = true
                    }
                };
            }

            // sms
            if (challengeRequest.CredentialType == CredentialTypeEnum.Sms)
            {
                string countryCode = ProxyLiterals.COUNTRY_CODE_UNITED_STATES;
                string areaCode = null;
                string phoneNumber = null;
                string extension = null;
                string label = challengeRequest.Label;

                if (!string.IsNullOrEmpty(challengeRequest.ContactInfo))
                {
                    if (challengeRequest.ContactInfo.Length > 2)
                    {
                        areaCode = challengeRequest.ContactInfo.Substring(0, 3);
                    }
                    if (challengeRequest.ContactInfo.Length > 3)
                    {
                        phoneNumber = challengeRequest.ContactInfo.Substring(3);
                    }
                }

                credentialChallengeRequestList.acspChallengeRequestData = new Proxies.AcspChallengeRequestData
                {
                    payload = new Proxies.OOBSMSChallengeRequest
                    {
                        contactList = new Proxies.OOBPhoneInfo
                        {
                            countryCode = countryCode,
                            areaCode = areaCode,
                            phoneNumber = phoneNumber,
                            extension = extension,
                            label = label
                        }
                    }
                };
            }

            // securid
            if (challengeRequest.CredentialType == CredentialTypeEnum.Token)
            {
                credentialChallengeRequestList.acspChallengeRequestData = new Proxies.AcspChallengeRequestData
                {
                    payload = new Proxies.SecurIdChallengeRequest()
                };
            }

            proxyChallengeRequest.credentialChallengeRequestList = credentialChallengeRequestList;

            #endregion proxy request

            var proxyChallengeResponse = Client.challenge(proxyChallengeRequest);

            #region proxy response

            if (proxyChallengeResponse != null)
            {
                GetDeviceResult(proxyChallengeResponse, challengeResponse);
                GetIdentificationData(proxyChallengeResponse, challengeResponse);
                GetMessageHeader(proxyChallengeResponse, challengeResponse);
                GetStatusHeader(proxyChallengeResponse, challengeResponse);

                // credential challenge
                if (proxyChallengeResponse.credentialChallengeList != null)
                {
                    // questions
                    if (challengeRequest.CredentialType == CredentialTypeEnum.Questions)
                    {
                        if (proxyChallengeResponse.credentialChallengeList.challengeQuestionChallenge != null
                            && proxyChallengeResponse.credentialChallengeList.challengeQuestionChallenge.payload != null
                            && proxyChallengeResponse.credentialChallengeList.challengeQuestionChallenge.payload.callStatus != null)
                        {
                            challengeResponse.ChallengeResult = EnumExtensions.ConvertProxyChallengeResultToChallengeResultEnum(proxyChallengeResponse.credentialChallengeList.challengeQuestionChallenge.payload.callStatus.statusCode);

                            if (proxyChallengeResponse.credentialChallengeList.challengeQuestionChallenge.payload.challengeQuestions != null
                                && proxyChallengeResponse.credentialChallengeList.challengeQuestionChallenge.payload.challengeQuestions.Length > 0)
                            {
                                challengeResponse.ChallengeQuestions = proxyChallengeResponse.credentialChallengeList.challengeQuestionChallenge.payload.challengeQuestions.Select(x => new ChallengeQuestion
                                {
                                    QuestionId = x.questionId,
                                    QuestionText = x.questionText
                                });
                            }
                        }
                    }

                    // email
                    if (challengeRequest.CredentialType == CredentialTypeEnum.Email)
                    {
                        if (proxyChallengeResponse.credentialChallengeList.oobEmailChallenge != null
                            && proxyChallengeResponse.credentialChallengeList.oobEmailChallenge.payload != null
                            && proxyChallengeResponse.credentialChallengeList.oobEmailChallenge.payload.authenticationResult != null)
                        {
                            challengeResponse.ChallengeResult = EnumExtensions.ConvertProxyChallengeResultToChallengeResultEnum(proxyChallengeResponse.credentialChallengeList.oobEmailChallenge.payload.callStatus.statusCode);
                        }
                    }

                    // sms
                    if (challengeRequest.CredentialType == CredentialTypeEnum.Sms)
                    {
                        if (proxyChallengeResponse.credentialChallengeList.acspChallengeResponseData != null
                            && proxyChallengeResponse.credentialChallengeList.acspChallengeResponseData.payload != null)
                        {
                            challengeResponse.ChallengeResult = EnumExtensions.ConvertProxyChallengeResultToChallengeResultEnum(proxyChallengeResponse.credentialChallengeList.acspChallengeResponseData.callStatus.statusCode);
                        }
                    }

                    // securid
                    if (challengeRequest.CredentialType == CredentialTypeEnum.Token)
                    {
                        if (proxyChallengeResponse.credentialChallengeList.acspChallengeResponseData != null
                            && proxyChallengeResponse.credentialChallengeList.acspChallengeResponseData.payload != null
                            && proxyChallengeResponse.credentialChallengeList.acspChallengeResponseData.callStatus != null)
                        {
                            var securIdChallengeResponse = proxyChallengeResponse.credentialChallengeList.acspChallengeResponseData.payload as Proxies.SecurIdChallengeResponse;
                            var requiredAction = securIdChallengeResponse.requiredAuthAction;
                            challengeResponse.ChallengeResult = EnumExtensions.ConvertProxyChallengeResultToChallengeResultEnum(proxyChallengeResponse.credentialChallengeList.acspChallengeResponseData.callStatus.statusCode);
                        }
                    }
                }
            }

            #endregion proxy response

            return challengeResponse;
        }
    }
}


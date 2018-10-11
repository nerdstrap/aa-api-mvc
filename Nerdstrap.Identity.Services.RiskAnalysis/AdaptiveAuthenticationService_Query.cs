using Nerdstrap.Identity.Services.RiskAnalysis.Contracts;
using Nerdstrap.Identity.Services.RiskAnalysis.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nerdstrap.Identity.Services.RiskAnalysis
{
    public partial class AdaptiveAuthenticationService : IAdaptiveAuthenticationService
    {
        public virtual QueryResponse Query(QueryRequest queryRequest)
        {
            var queryResponse = new QueryResponse();

            #region proxy request

            Proxies.QueryRequest proxyQueryRequest = new Proxies.QueryRequest();

            // action type list

            // device request

            // identification data
            SetIdentificationData(proxyQueryRequest, queryRequest);

            // message header
            SetMessageHeader(proxyQueryRequest, Proxies.RequestType.QUERY);

            // security header
            SetSecurityHeader(proxyQueryRequest);

            // credential management request
            var credentialManagementRequestList = new Proxies.CredentialManagementRequestList();

            // challenge question answers
            credentialManagementRequestList.challengeQuestionManagementRequest = new Proxies.ChallengeQuestionManagementRequest()
            {
                credentialProvisioningStatus = Proxies.CredentialProvisioningStatus.ACTIVE,
                credentialProvisioningStatusSpecified = true,
                payload = new Proxies.ChallengeQuestionManagementRequestPayload()
                {
                    actionTypeList = new Proxies.ChallengeQuestionActionTypeList()
                    {
                        challengeQuestionActionType = new Proxies.ChallengeQuestionActionType[]
                        {
                            Proxies.ChallengeQuestionActionType.GET_USER_QUESTION
                        }
                    },
                    challengeQuestionConfig = new Proxies.ChallengeQuestionConfig()
                    {
                        // todo what does this need set to
                        questionCount = 1,
                        questionCountSpecified = true,
                        includeRetired = false,
                        includeRetiredSpecified = true
                    }
                }
            };

            // email
            credentialManagementRequestList.oobEmailManagementRequest = new Proxies.OobEmailManagementRequest()
            {
                credentialProvisioningStatus = Proxies.CredentialProvisioningStatus.ACTIVE,
                credentialProvisioningStatusSpecified = true,
                payload = new Proxies.EmailManagementRequestPayload()
                {
                    oobActionTypeList = new Proxies.OOBActionTypeList()
                    {
                        oobActionType = new Proxies.OOBActionType[]
                        {
                            Proxies.OOBActionType.GET_OOB
                        }
                    }
                }
            };

            // sms
            credentialManagementRequestList.acspManagementRequestData = new Proxies.AcspManagementRequestData()
            {
                credentialProvisioningStatus = Proxies.CredentialProvisioningStatus.ACTIVE,
                credentialProvisioningStatusSpecified = true,
                payload = new Proxies.OOBSMSManagementRequest()
                {
                    action = Proxies.Action.GET,
                    actionSpecified = true
                }
            };

            // phone
            credentialManagementRequestList.oobPhoneManagementRequest = new Proxies.OobPhoneManagementRequest()
            {
                credentialProvisioningStatus = Proxies.CredentialProvisioningStatus.ACTIVE,
                credentialProvisioningStatusSpecified = true,
                payload = new Proxies.PhoneManagementRequestPayload()
                {
                    oobActionTypeList = new Proxies.OOBActionTypeList()
                    {
                        oobActionType = new Proxies.OOBActionType[]
                        {
                            Proxies.OOBActionType.GET_OOB
                        }
                    }
                }
            };

            proxyQueryRequest.credentialManagementRequestList = credentialManagementRequestList;

            #endregion proxy request

            var proxyQueryResponse = Client.query(proxyQueryRequest);

            #region proxy response

            if (proxyQueryResponse != null)
            {
                GetDeviceResult(proxyQueryResponse, queryResponse);
                GetIdentificationData(proxyQueryResponse, queryResponse);
                GetMessageHeader(proxyQueryResponse, queryResponse);
                GetStatusHeader(proxyQueryResponse, queryResponse);

                if (proxyQueryResponse.credentialManagementResponseList != null)
                {
                    // challenge question answers
                    if (proxyQueryResponse.credentialManagementResponseList.challengeQuestionManagementResponse != null
                        && proxyQueryResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload != null
                        && proxyQueryResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload.userChallQuesDataList != null
                        && proxyQueryResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload.userChallQuesDataList.Length > 0)
                    {
                        queryResponse.ChallengeQuestions = proxyQueryResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload.userChallQuesDataList.Select(x => new ChallengeQuestion
                        {
                            QuestionText = x.questionText,
                            QuestionId = x.questionId
                        });
                    }
                    else
                    {
                        queryResponse.ChallengeQuestions = new List<ChallengeQuestion>();
                    }

                    // email
                    if (proxyQueryResponse.credentialManagementResponseList.oobEmailManagementResponse != null
                        && proxyQueryResponse.credentialManagementResponseList.oobEmailManagementResponse.payload != null
                        && proxyQueryResponse.credentialManagementResponseList.oobEmailManagementResponse.payload.contactList != null
                        && proxyQueryResponse.credentialManagementResponseList.oobEmailManagementResponse.payload.contactList.Length > 0)
                    {
                        queryResponse.EmailContacts = proxyQueryResponse.credentialManagementResponseList.oobEmailManagementResponse.payload.contactList.Select(x => new EmailContact
                        {
                            EmailAddress = x.address,
                            Label = x.label
                        });
                    }
                    else
                    {
                        queryResponse.EmailContacts = new List<EmailContact>();
                    }

                    // sms
                    if (proxyQueryResponse.credentialManagementResponseList.acspManagementResponseData != null
                        && proxyQueryResponse.credentialManagementResponseList.acspManagementResponseData.payload != null)
                    {
                        var oobSMSManagementResponse = proxyQueryResponse.credentialManagementResponseList.acspManagementResponseData.payload as Proxies.OOBSMSManagementResponse;
                        if (oobSMSManagementResponse != null && oobSMSManagementResponse.contactList != null && oobSMSManagementResponse.contactList.Length > 0)
                        {
                            queryResponse.SmsContacts = oobSMSManagementResponse.contactList.Select(x => new SmsContact
                            {
                                PhoneNumber = x.countryCode + x.areaCode + x.phoneNumber + x.extension,
                                Label = x.label
                            });
                        }
                    }
                    else
                    {
                        queryResponse.SmsContacts = new List<SmsContact>();
                    }

                    // phone
                    if (proxyQueryResponse.credentialManagementResponseList.oobPhoneManagementResponse != null
                        && proxyQueryResponse.credentialManagementResponseList.oobPhoneManagementResponse.payload != null)
                    {
                        var phoneManagementResponse = proxyQueryResponse.credentialManagementResponseList.oobPhoneManagementResponse.payload as Proxies.PhoneManagementResponsePayload;
                        if (phoneManagementResponse != null && phoneManagementResponse.contactList != null && phoneManagementResponse.contactList.Length > 0)
                        {
                            queryResponse.PhoneContacts = phoneManagementResponse.contactList.Select(x => new PhoneContact
                            {
                                PhoneNumber = x.countryCode + x.areaCode + x.phoneNumber + x.extension,
                                Label = x.label
                            });
                        }
                    }
                    else
                    {
                        queryResponse.PhoneContacts = new List<PhoneContact>();
                    }
                }
            }

            #endregion proxy response

            return queryResponse;
        }
    }
}

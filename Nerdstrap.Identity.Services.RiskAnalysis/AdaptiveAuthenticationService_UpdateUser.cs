using Nerdstrap.Identity.Services.RiskAnalysis.Contracts;
using System.Linq;

namespace Nerdstrap.Identity.Services.RiskAnalysis
{
    public partial class AdaptiveAuthenticationService : IAdaptiveAuthenticationService
    {
        public virtual UpdateUserResponse UpdateUser(UpdateUserRequest updateUserRequest)
        {
            var updateUserResponse = new UpdateUserResponse();

            #region proxy request

            Proxies.UpdateUserRequest proxyUpdateUserRequest = new Proxies.UpdateUserRequest();

            // action type list
            proxyUpdateUserRequest.actionTypeList = new Proxies.GenericActionTypeList
            {
                genericActionTypes = new Proxies.GenericActionType[]
                {
                    Proxies.GenericActionType.SET_USER_STATUS
                }
            };

            // device request
            SetDeviceRequest(proxyUpdateUserRequest, updateUserRequest);

            // identification data
            SetIdentificationData(proxyUpdateUserRequest, updateUserRequest);

            // message header
            SetMessageHeader(proxyUpdateUserRequest, Proxies.RequestType.UPDATEUSER);

            // security header
            SetSecurityHeader(proxyUpdateUserRequest);

            // event data
            proxyUpdateUserRequest.eventDataList = new Proxies.EventData[]
            {
                new Proxies.EventData
                {
                    eventType = Proxies.EventType.ENROLL,
                    eventTypeSpecified = true
                }
            };

            // run risk type
            proxyUpdateUserRequest.runRiskType = Proxies.RunRiskType.NONE;

            // channel indicator
            proxyUpdateUserRequest.channelIndicator = Proxies.ChannelIndicatorType.WEB;
            proxyUpdateUserRequest.channelIndicatorSpecified = true;

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
                            Proxies.ChallengeQuestionActionType.SET_USER_QUESTION
                        }
                    },
                    challengeQuestionList = updateUserRequest.ChallengeQuestionAnswers.Select(x => new Proxies.ChallengeQuestion
                    {
                        questionId = x.QuestionId,
                        actualAnswer = x.ActualAnswer
                    }).ToArray()
                }
            };


            proxyUpdateUserRequest.credentialManagementRequestList = credentialManagementRequestList;

            #endregion proxy request

            var proxyUpdateUserResponse = Client.updateUser(proxyUpdateUserRequest);

            #region proxy response

            if (proxyUpdateUserResponse != null)
            {
                GetDeviceResult(proxyUpdateUserResponse, updateUserResponse);
                GetIdentificationData(proxyUpdateUserResponse, updateUserResponse);
                GetMessageHeader(proxyUpdateUserResponse, updateUserResponse);
                GetStatusHeader(proxyUpdateUserResponse, updateUserResponse);

                // credential management
            }

            #endregion proxy response

            return updateUserResponse;
        }
    }
}
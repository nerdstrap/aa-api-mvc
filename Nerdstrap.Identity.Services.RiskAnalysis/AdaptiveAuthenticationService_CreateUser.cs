using Nerdstrap.Identity.Services.RiskAnalysis.Contracts;
using Nerdstrap.Identity.Services.RiskAnalysis.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nerdstrap.Identity.Services.RiskAnalysis
{
    public partial class AdaptiveAuthenticationService : IAdaptiveAuthenticationService
    {
        public virtual CreateUserResponse CreateUser(CreateUserRequest createUserRequest)
        {
            var createUserResponse = new CreateUserResponse();

            #region proxy request

            Proxies.CreateUserRequest proxyCreateUserRequest = new Proxies.CreateUserRequest();

            // action type list
            proxyCreateUserRequest.actionTypeList = new Proxies.GenericActionTypeList
            {
                genericActionTypes = new Proxies.GenericActionType[]
                {
                    Proxies.GenericActionType.SET_USER_STATUS
                }
            };

            // device request
            SetDeviceRequest(proxyCreateUserRequest, createUserRequest);

            // identification data
            SetIdentificationData(proxyCreateUserRequest, createUserRequest);

            // message header
            SetMessageHeader(proxyCreateUserRequest, Proxies.RequestType.CREATEUSER);

            // security header
            SetSecurityHeader(proxyCreateUserRequest);

            // credential management request
            var credentialManagementRequestList = new Proxies.CredentialManagementRequestList();

            // challenge questions
            credentialManagementRequestList.challengeQuestionManagementRequest = new Proxies.ChallengeQuestionManagementRequest
            {
                credentialProvisioningStatus = Proxies.CredentialProvisioningStatus.ACTIVE,
                credentialProvisioningStatusSpecified = true,
                payload = new Proxies.ChallengeQuestionManagementRequestPayload
                {
                    actionTypeList = new Proxies.ChallengeQuestionActionTypeList
                    {
                        challengeQuestionActionType = new Proxies.ChallengeQuestionActionType[]
                        {
                            Proxies.ChallengeQuestionActionType.BROWSE_QUESTION
                        }
                    },
                    challengeQuestionConfig = new Proxies.ChallengeQuestionConfig
                    {
                        includeRetired = false,
                        includeRetiredSpecified = true
                    }
                }
            };

            proxyCreateUserRequest.credentialManagementRequestList = credentialManagementRequestList;

            #endregion proxy request

            var proxyCreateUserResponse = Client.createUser(proxyCreateUserRequest);

            #region proxy response

            if (proxyCreateUserResponse != null)
            {
                GetDeviceResult(proxyCreateUserResponse, createUserResponse);
                GetIdentificationData(proxyCreateUserResponse, createUserResponse);
                GetMessageHeader(proxyCreateUserResponse, createUserResponse);
                GetStatusHeader(proxyCreateUserResponse, createUserResponse);

                if (proxyCreateUserResponse.credentialManagementResponseList != null)
                {
                    // challenge questions
                    if (proxyCreateUserResponse.credentialManagementResponseList.challengeQuestionManagementResponse != null
                        && proxyCreateUserResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload != null
                        && proxyCreateUserResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload.browsableChallQuesGroupList != null
                        && proxyCreateUserResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload.browsableChallQuesGroupList.Length > 0)
                    {
                        var availableChallengeQuestions = new List<ChallengeQuestion>();
                        for (var i = 0; i < proxyCreateUserResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload.browsableChallQuesGroupList.Length; i++)
                        {
                            var challengeQuestions = proxyCreateUserResponse.credentialManagementResponseList.challengeQuestionManagementResponse.payload.browsableChallQuesGroupList[i].challengeQuestion;
                            if (challengeQuestions != null && challengeQuestions.Length > 0)
                            {
                                availableChallengeQuestions.AddRange(challengeQuestions.Select(x => new ChallengeQuestion
                                {
                                    QuestionText = x.questionText,
                                    QuestionId = x.questionId
                                }));
                            }
                        }
                        createUserResponse.ChallengeQuestions = availableChallengeQuestions;
                    }
                }
            }

            #endregion proxy response    

            return createUserResponse;
        }
    }
}

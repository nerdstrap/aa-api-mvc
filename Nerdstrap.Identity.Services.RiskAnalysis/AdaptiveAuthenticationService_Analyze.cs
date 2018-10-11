using Nerdstrap.Identity.Services.RiskAnalysis.Contracts;
using Nerdstrap.Identity.Services.RiskAnalysis.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Nerdstrap.Identity.Services.RiskAnalysis
{
    public partial class AdaptiveAuthenticationService : IAdaptiveAuthenticationService
    {
        public virtual AnalyzeResponse Analyze(AnalyzeRequest analyzeRequest)
        {
            var analyzeResponse = new AnalyzeResponse();

            #region proxy request

            Proxies.AnalyzeRequest proxyAnalyzeRequest = new Proxies.AnalyzeRequest();

            // action type list
            proxyAnalyzeRequest.actionTypeList = new Proxies.GenericActionTypeList
            {
                genericActionTypes = new Proxies.GenericActionType[]
                {
                    Proxies.GenericActionType.GET_USER_STATUS
                }
            };

            // device request
            SetDeviceRequest(proxyAnalyzeRequest, analyzeRequest);

            // identification data
            SetIdentificationData(proxyAnalyzeRequest, analyzeRequest);

            // message header
            SetMessageHeader(proxyAnalyzeRequest, Proxies.RequestType.ANALYZE);

            // security header
            SetSecurityHeader(proxyAnalyzeRequest);

            // auto-create user flag
            proxyAnalyzeRequest.autoCreateUserFlag = false;
            proxyAnalyzeRequest.autoCreateUserFlagSpecified = true;

            // event data list
            Proxies.ClientDefinedFact[] clientDefinedAttributeList = null;
            if (analyzeRequest.CustomFact != null)
            {
                var clientDefinedFact = new Proxies.ClientDefinedFact()
                {
                    name = analyzeRequest.CustomFact.Name,
                    value = analyzeRequest.CustomFact.Value
                };
                clientDefinedFact.dataType = EnumExtensions.ConvertDataTypeEnumToProxyDataType(analyzeRequest.CustomFact.DataType);
                clientDefinedAttributeList = new Proxies.ClientDefinedFact[]
                {
                    clientDefinedFact
                };
            }
            proxyAnalyzeRequest.eventDataList = new Proxies.EventData[]
            {
                new Proxies.EventData
                {
                    eventType = Proxies.EventType.SESSION_SIGNIN,
                    eventTypeSpecified = true,
                    clientDefinedAttributeList = clientDefinedAttributeList
                }
            };

            // run risk type
            proxyAnalyzeRequest.runRiskType = Proxies.RunRiskType.ALL;

            // channel indicator
            proxyAnalyzeRequest.channelIndicator = Proxies.ChannelIndicatorType.WEB;
            proxyAnalyzeRequest.channelIndicatorSpecified = true;

            #endregion proxy request

            var proxyAnalyzeResponse = Client.analyze(proxyAnalyzeRequest);

            #region proxy response

            if (proxyAnalyzeResponse != null)
            {
                GetDeviceResult(proxyAnalyzeResponse, analyzeResponse);
                GetIdentificationData(proxyAnalyzeResponse, analyzeResponse);
                GetMessageHeader(proxyAnalyzeResponse, analyzeResponse);
                GetStatusHeader(proxyAnalyzeResponse, analyzeResponse);

                // action code
                if (proxyAnalyzeResponse.riskResult != null && proxyAnalyzeResponse.riskResult.triggeredRule != null)
                {
                    analyzeResponse.ActionCode = EnumExtensions.ConvertProxyActionCodeToActionCodeEnum(proxyAnalyzeResponse.riskResult.triggeredRule.actionCode);
                }

                // required credentials
                var requiredCredentialTypes = new List<CredentialTypeEnum>();
                if (proxyAnalyzeResponse.requiredCredentialList != null && proxyAnalyzeResponse.requiredCredentialList.Length > 0)
                {
                    requiredCredentialTypes.AddRange(proxyAnalyzeResponse.requiredCredentialList.Select(x => EnumExtensions.ConvertProxyCredentialTypeToCredentialTypeEnum(x.credentialType, x.genericCredentialType)));
                }

                // action code overrides
                if (analyzeResponse.ActionCode != ActionCodeEnum.Deny)
                {
                    requiredCredentialTypes.Add(CredentialTypeEnum.Password);

                    if (analyzeResponse.UserStatus == UserStatusEnum.NotEnrolled
                        || analyzeResponse.UserStatus == UserStatusEnum.Delete
                        || analyzeResponse.UserStatus == UserStatusEnum.UnVerified)
                    {
                        analyzeResponse.ActionCode = ActionCodeEnum.Enroll;
                    }
                }

                analyzeResponse.CollectibleCredentials = requiredCredentialTypes;
            }

            #endregion proxy response

            return analyzeResponse;
        }
    }
}

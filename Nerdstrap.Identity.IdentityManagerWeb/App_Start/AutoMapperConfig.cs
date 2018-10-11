using AutoMapper;
//using Services = Nerdstrap.Identity.Services;
using WebApi = Nerdstrap.Identity.IdentityManagerWeb;

namespace Nerdstrap.Identity.IdentityManagerWeb
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Initialize()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                #region ui -> services

                //cfg.CreateMap<Services.Contracts.BaseRequest, WebApi.Contracts.BaseRequest>();
                //cfg.CreateMap<Services.Contracts.BaseResponse, WebApi.Contracts.BaseResponse>();

                //cfg.CreateMap<Services.Contracts.AnalyzeRequest, WebApi.Contracts.AnalyzeRequest>().IncludeBase<Services.Contracts.BaseRequest, WebApi.Contracts.BaseRequest>();
                //cfg.CreateMap<Services.Contracts.AnalyzeResponse, WebApi.Contracts.AnalyzeResponse>().IncludeBase<Services.Contracts.BaseResponse, WebApi.Contracts.BaseResponse>();
                //cfg.CreateMap<Services.Contracts.AuthenticateRequest, WebApi.Contracts.AuthenticateRequest>().IncludeBase<Services.Contracts.BaseRequest, WebApi.Contracts.BaseRequest>();
                //cfg.CreateMap<Services.Contracts.AuthenticateResponse, WebApi.Contracts.AuthenticateResponse>().IncludeBase<Services.Contracts.BaseResponse, WebApi.Contracts.BaseResponse>();
                //cfg.CreateMap<Services.Contracts.ChallengeRequest, WebApi.Contracts.ChallengeRequest>().IncludeBase<Services.Contracts.BaseRequest, WebApi.Contracts.BaseRequest>();
                //cfg.CreateMap<Services.Contracts.ChallengeResponse, WebApi.Contracts.ChallengeResponse>().IncludeBase<Services.Contracts.BaseResponse, WebApi.Contracts.BaseResponse>();

                //cfg.CreateMap<Services.Models.DeviceRequest, WebApi.Models.DeviceRequest>();

                //cfg.CreateMap<Services.Enums.AnalyzeResultCodeEnum, WebApi.Enums.AnalyzeResultCodeEnum>();
                //cfg.CreateMap<Services.Enums.AsyncSprayStatusCodeEnum, WebApi.Enums.AsyncSprayStatusCodeEnum>();
                //cfg.CreateMap<Services.Enums.AuthenticateResultCodeEnum, WebApi.Enums.AuthenticateResultCodeEnum>();
                //cfg.CreateMap<Services.Enums.BindDeviceResultCodeEnum, WebApi.Enums.BindDeviceResultCodeEnum>();
                //cfg.CreateMap<Services.Enums.ChallengeResultCodeEnum, WebApi.Enums.ChallengeResultCodeEnum>();
                //cfg.CreateMap<Services.Enums.ContactTypeEnum, WebApi.Enums.ContactTypeEnum>();
                //cfg.CreateMap<Services.Enums.CredentialTypeEnum, WebApi.Enums.CredentialTypeEnum>();

                #endregion ui -> services
            });

            config.AssertConfigurationIsValid();

            return config;
        }
    }
}

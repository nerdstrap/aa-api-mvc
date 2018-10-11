using AutoMapper;
using log4net;
using Nerdstrap.Identity.Services.DirectoryAccess;
using Nerdstrap.Identity.Services.RiskAnalysis;
using System;
using Nerdstrap.Identity.Logic.Enrollment.Contracts;

namespace Nerdstrap.Identity.Logic.Enrollment
{
    public partial class SelfServiceLogic : ISelfServiceLogic
    {
        #region Properties

        private static ILog _logger;
        public ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(typeof(SelfServiceLogic))); }
            set { _logger = value; }
        }

        public IMapper Mapper { get; set; }
        public IDirectoryService DirectoryService { get; set; }
        public IAdaptiveAuthenticationService AdaptiveAuthenticationService { get; set; }

        #endregion Properties

        #region ctor

        public SelfServiceLogic(IMapper mapper, IDirectoryService directoryService, IAdaptiveAuthenticationService adaptiveAuthenticationService)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (directoryService == null)
            {
                throw new ArgumentNullException(nameof(directoryService));
            }
            if (adaptiveAuthenticationService == null)
            {
                throw new ArgumentNullException(nameof(adaptiveAuthenticationService));
            }

            Mapper = mapper;
            DirectoryService = directoryService;
            AdaptiveAuthenticationService = adaptiveAuthenticationService;
        }

        #endregion ctor

        public AuthenticateResponse Signin(AuthenticateRequest authenticateRequest)
        {
            var AuthenticateRequestServiceContract = Mapper.Map<Services.DirectoryAccess.Contracts.AuthenticateRequest>(authenticateRequest);
            var AuthenticateResponseServiceContract = DirectoryService.Authenticate(AuthenticateRequestServiceContract);
            if (AuthenticateResponseServiceContract.AuthenticateResult == Services.DirectoryAccess.Enums.AuthenticateResultEnum.Allow)
            {
                // analyze
            }
            else if (AuthenticateResponseServiceContract.AuthenticateResult == Services.DirectoryAccess.Enums.AuthenticateResultEnum.Expired)
            {
                // expired redirect
            }
            else
            {
                // deny
            }
            var authenticateResponse = Mapper.Map<AuthenticateResponse>(AuthenticateResponseServiceContract);
            return authenticateResponse;
        }

        public ChallengeResponse Challenge(ChallengeRequest challengeRequest)
        {
            var challengeRequestServiceContract = Mapper.Map<Services.RiskAnalysis.Contracts.ChallengeRequest>(challengeRequest);
            var challengeResponseServiceContract = AdaptiveAuthenticationService.Challenge(challengeRequestServiceContract);
            var challengeResponse = Mapper.Map<ChallengeResponse>(challengeResponseServiceContract);
            return challengeResponse;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest)
        {
            var authenticateRequestServiceContract = Mapper.Map<Services.RiskAnalysis.Contracts.AuthenticateRequest>(authenticateRequest);
            var authenticateResponseServiceContract = AdaptiveAuthenticationService.Authenticate(authenticateRequestServiceContract);
            var authenticateResponse = Mapper.Map<AuthenticateResponse>(authenticateResponseServiceContract);
            return authenticateResponse;
        }

        public GetUserResponse GetUser(GetUserRequest getUserRequest)
        {
            var getUserRequestServiceContract = Mapper.Map<Services.DirectoryAccess.Contracts.GetUserRequest>(getUserRequest);
            var getUserResponseServiceContract = DirectoryService.GetUser(getUserRequestServiceContract);
            var getUserResponse = Mapper.Map<GetUserResponse>(getUserResponseServiceContract);
            return getUserResponse;
        }
    }
}

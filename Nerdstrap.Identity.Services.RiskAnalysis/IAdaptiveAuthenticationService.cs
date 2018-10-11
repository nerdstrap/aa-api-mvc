using Nerdstrap.Identity.Services.RiskAnalysis.Contracts;

namespace Nerdstrap.Identity.Services.RiskAnalysis
{
    public interface IAdaptiveAuthenticationService
    {
        AnalyzeResponse Analyze(AnalyzeRequest analyzeRequest);
        AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest);
        ChallengeResponse Challenge(ChallengeRequest challengeRequest);
        CreateUserResponse CreateUser(CreateUserRequest createUserRequest);
        QueryResponse Query(QueryRequest queryRequest);
        UpdateUserResponse UpdateUser(UpdateUserRequest updateUserRequest);
    }
}

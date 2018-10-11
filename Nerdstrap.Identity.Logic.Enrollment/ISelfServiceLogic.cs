using Nerdstrap.Identity.Logic.Enrollment.Contracts;

namespace Nerdstrap.Identity.Logic.Enrollment
{
	public interface ISelfServiceLogic
    {
        AuthenticateResponse Signin(AuthenticateRequest authenticateRequest);
        ChallengeResponse Challenge(ChallengeRequest challengeRequest);
        AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest);
        GetUserResponse GetUser(GetUserRequest getUserRequest);
    }
}

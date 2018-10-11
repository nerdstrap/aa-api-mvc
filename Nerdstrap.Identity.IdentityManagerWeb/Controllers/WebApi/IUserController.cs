using Nerdstrap.Identity.IdentityManagerWeb.Contracts;
using System.Web.Http;

namespace Nerdstrap.Identity.IdentityManagerWeb.Controllers.WebApi
{
	public interface IUserController
    {
        //IHttpActionResult RefreshToken(RefreshTokenRequest refreshTokenRequest);

        IHttpActionResult Signin(AuthenticateRequest authenticateRequest);
        IHttpActionResult Challenge(ChallengeRequest challengeRequest);
        IHttpActionResult Authenticate(AuthenticateRequest authenticateRequest);
        //IHttpActionResult GetUser(GetUserRequest getUserRequest);

    }
}

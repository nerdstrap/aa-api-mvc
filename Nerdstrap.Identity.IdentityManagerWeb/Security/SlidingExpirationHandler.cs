using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Nerdstrap.Identity.IdentityManagerWeb.Security
{
    public class SlidingExpirationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            // did the request come with a token?
            var authorization = request.Headers.Authorization;
            if (authorization == null || authorization.Scheme != "Bearer" || string.IsNullOrEmpty(authorization.Parameter))
            {
                // no token
                return response;
            }

            // did the token pass authentication?
            var claimsPrincipal = request.GetRequestContext().Principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                // not authorized
                return response;
            }

            // copy the claims to a new jwt
            var userId = claimsPrincipal.Identity.GetUserId();
            var accessLevel = claimsPrincipal.Identity.GetAccessLevel();
            var userToken = TokenExtensions.CreateToken(userId, accessLevel);
            response.Headers.Add("Set-Authorization", userToken);

            var objectContent = response.Content as ObjectContent;
            if (objectContent != null)
            {
                var baseResponse = objectContent.Value as Contracts.BaseResponse;
                if (baseResponse != null)
                {
                    baseResponse.UserToken = userToken;
                }
            }

            return response;
        }
    }
}

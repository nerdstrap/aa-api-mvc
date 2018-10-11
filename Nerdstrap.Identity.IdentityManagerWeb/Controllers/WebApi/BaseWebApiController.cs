using Nerdstrap.Identity.IdentityManagerWeb.Contracts;
using Nerdstrap.Identity.IdentityManagerWeb.Models;
using Nerdstrap.Identity.IdentityManagerWeb.Security;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Nerdstrap.Identity.IdentityManagerWeb.Controllers.WebApi
{
    public class BaseWebApiController : ApiController
    {
        protected virtual string GetAuthenticatedUserId(HttpActionContext actionContext)
        {
            string userId = "";
            var headers = actionContext.Request.Headers;
            var authorization = actionContext.Request.Headers.Authorization;
            BaseRequest baseRequest = actionContext.ActionArguments.First().Value as BaseRequest;
            if (baseRequest != null)
            {
                // overwrite the userId to read from the token
                if (authorization != null && authorization.Scheme == "Bearer")
                {
                    if (!string.IsNullOrEmpty(authorization.Parameter))
                    {
                        var token = authorization.Parameter;
                        var claimsPrincipal = TokenExtensions.ValidateToken(token);
                        if (claimsPrincipal != null && claimsPrincipal.Identity != null)
                        {
                            userId = claimsPrincipal.Identity.GetUserId();
                        }
                    }
                }
            }

            return userId;
        }

        protected virtual string GetAuthenticatedUserId(BaseRequest baseRequest)
        {
            string userId = "";

            if (baseRequest != null && string.IsNullOrEmpty(baseRequest.UserToken) == false)
            {
                var claimsPrincipal = TokenExtensions.ValidateToken(baseRequest.UserToken);
                if (claimsPrincipal != null && claimsPrincipal.Identity != null)
                {
                    userId = claimsPrincipal.Identity.GetUserId();
                }
            }

            return userId;
        }

        protected virtual DeviceRequest GetDeviceRequest(HttpActionContext actionContext)
        {
            var deviceRequest = new DeviceRequest();

            //deviceRequest.HttpAccept = actionContext.Request.SerializeHeaderValues("Accept");
            //deviceRequest.HttpAcceptEncoding = actionContext.Request.SerializeHeaderValues("Accept-Encoding");
            //deviceRequest.httpAcceptLanguage = actionContext.Request.SerializeHeaderValues("Accept-Language");
            //deviceRequest.HttpReferrer = actionContext.Request.Ser`ializeHeaderValues("Referer");
            //// x-forwarded for logic not included
            //deviceRequest.IpAddress = actionContext.Request.GetClientIpAddress();
            //deviceRequest.UserAgent = actionContext.Request.SerializeHeaderValues("User-Agent");

            return deviceRequest;
        }
    }
}

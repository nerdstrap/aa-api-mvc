using Nerdstrap.Identity.IdentityManagerWeb.Contracts;
using Nerdstrap.Identity.IdentityManagerWeb.Enums;
using Nerdstrap.Identity.IdentityManagerWeb.Security;
using System.Web.Http;

namespace Nerdstrap.Identity.IdentityManagerWeb.Controllers.WebApi
{
    public partial class UserController : BaseWebApiController, IUserController
    {
        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(AuthenticateRequest authenticateRequest)
        {
            //var authenticateUserRequestBusinessLogicContract = Mapper.Map<BusinessLogic.Contracts.AuthenticateUserRequest>(authenticateUserRequest);
            //var authenticateUserResponseBusinessLogicContract = UserManagementBusinessLogic.AuthenticateUser(authenticateUserRequestBusinessLogicContract);
            //var authenticateUserResponse = Mapper.Map<AuthenticateResponse>(authenticateUserResponseBusinessLogicContract);

            //if (authenticateUserResponse.CallStatusCode == true && (authenticateUserResponse.AuthStatusCode == AuthenticateResultCodeEnum.Success || authenticateUserResponse.AuthStatusCode == AuthenticateResultCodeEnum.Challenge))
            //{
            //    authenticateUserResponse.Token = TokenExtensions.CreateToken(authenticateUserRequest.UserId, authenticateUserRequest.UserId);
            //}

            var authenticateResponse = new AuthenticateResponse();
            return Ok(authenticateResponse);
        }
    }
}

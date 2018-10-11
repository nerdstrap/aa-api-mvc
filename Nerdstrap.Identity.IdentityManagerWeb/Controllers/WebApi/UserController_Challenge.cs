using Nerdstrap.Identity.IdentityManagerWeb.Constants;
using Nerdstrap.Identity.IdentityManagerWeb.Contracts;
using Nerdstrap.Identity.IdentityManagerWeb.Enums;
using Nerdstrap.Identity.IdentityManagerWeb.Models;
using Nerdstrap.Identity.IdentityManagerWeb.Security;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web.Http;

namespace Nerdstrap.Identity.IdentityManagerWeb.Controllers.WebApi
{
    public partial class UserController : BaseWebApiController, IUserController
    {
        [HttpPost]
        [Route("challenge")]
        public IHttpActionResult Challenge(ChallengeRequest challengeRequest)
        {
            //var challengeUserRequestBusinessLogicContract = Mapper.Map<BusinessLogic.Contracts.ChallengeUserRequest>(challengeUserRequest);
            //var challengeUserResponseBusinessLogicContract = UserManagementBusinessLogic.ChallengeUser(challengeUserRequestBusinessLogicContract);
            //var challengeUserResponse = Mapper.Map<ChallengeResponse>(challengeUserResponseBusinessLogicContract);

            //if (challengeUserResponse.CallStatusCode == true && challengeUserResponse.ChallengeResult == Enums.ChallengeResultCodeEnum.Success)
            //{
            //    challengeUserResponse.Token = TokenExtensions.CreateToken(challengeUserRequest.UserId, challengeUserRequest.UserId);
            //}

            var challengeResponse = new ChallengeResponse();
            return Ok(challengeResponse);
        }
    }
}

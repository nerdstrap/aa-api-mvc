using Nerdstrap.Identity.IdentityManagerWeb.Contracts;
using Nerdstrap.Identity.IdentityManagerWeb.Security;
using System.Web.Http;

namespace Nerdstrap.Identity.IdentityManagerWeb.Controllers.WebApi
{
    public partial class UserController : BaseWebApiController, IUserController
    {
        [HttpPost]
        [Route("analyze")]
        public IHttpActionResult Analyze(AnalyzeRequest analyzeRequest)
        {
            //var analyzeUserRequestBusinessLogicContract = Mapper.Map<BusinessLogic.Contracts.AnalyzeUserRequest>(analyzeRequest);
            //analyzeUserRequestBusinessLogicContract.CustomFact = new BusinessLogic.Models.CustomFact()
            //{
            //    DataType = BusinessLogic.Enums.DataTypeEnum.String,
            //    DataTypeSpecified = true,
            //    Name = "AppProcess",
            //    Value = "FORGOTHAVE"
            //};
            //var analyzeUserResponseBusinessLogicContract = UserManagementBusinessLogic.AnalyzeUser(analyzeUserRequestBusinessLogicContract);
            //var analyzeUserResponse = Mapper.Map<AnalyzeUserResponse>(analyzeUserResponseBusinessLogicContract);

            //if (analyzeUserResponse.CallStatusCode == true && (analyzeUserResponse.ActionCode == Enums.AnalyzeResultCodeEnum.Allow || analyzeUserResponse.ActionCode == Enums.AnalyzeResultCodeEnum.Challenge))
            //{
            //    analyzeUserResponse.Token = TokenExtensions.CreateToken(analyzeUserResponse.UserId, analyzeUserResponse.UserId);
            //}

            var analyzeResponse = new AnalyzeResponse();
            return Ok(analyzeResponse);
        }
    }
}

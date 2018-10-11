using log4net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Nerdstrap.Identity.IdentityManagerWeb.Filters
{
    public class TraceHttpActionAttribute : ActionFilterAttribute
    {
        private ILog _logger;

        public ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger("WebApiTraceLogger")); }
            set { _logger = value; }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            if (Logger.IsDebugEnabled)
            {
                //Logger.Debug();
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (Logger.IsDebugEnabled || ((!actionContext.ModelState.IsValid) && Logger.IsWarnEnabled))
            {
                //Logger.Debug();
            }
            base.OnActionExecuting(actionContext);
        }
    }
}

using System.Web.Http.ExceptionHandling;
using log4net;

namespace Nerdstrap.Identity.IdentityManagerWeb
{
	public class WebApiGlobalExceptionLogger : ExceptionLogger
	{
		private static ILog _logger;

		public ILog Logger
		{
			get { return _logger ?? (_logger = LogManager.GetLogger(typeof(WebApiGlobalExceptionLogger))); }
			set { _logger = value; }
		}

		public override void Log(ExceptionLoggerContext context)
		{
			if (context == null || context.Exception == null)
			{
				return;
			}
			Logger.Error("Unhandled Web Api exception", context.Exception);
		}
	}
}

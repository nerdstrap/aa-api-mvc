using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using log4net;

namespace Nerdstrap.Identity.IdentityManagerWeb
{
	public class ObscureExceptionAttribute : ExceptionFilterAttribute
	{
		private ILog _logger;

		public ILog Logger
		{
			get { return _logger ?? (_logger = LogManager.GetLogger(typeof(ObscureExceptionAttribute))); }
			set { _logger = value; }
		}

		public override void OnException(HttpActionExecutedContext context)
		{
			Logger.Error(context.Exception.Message, context.Exception);

			if (context.Exception is HttpRequestException)
			{
				var exception = new HttpRequestException(context.Exception.Message);
				context.Exception = exception;
				base.OnException(context);
			}
			else if (context.Exception is HttpResponseException)
			{
				var exception = new HttpResponseException(((HttpResponseException)context.Exception).Response);
				context.Exception = exception;
				base.OnException(context);
			}
			else if (context.Exception is ArgumentException)
			{
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.BadRequest,
					Content = new StringContent(context.Exception.Message)
				});
			}
			else if (context.Exception is UnauthorizedAccessException)
			{
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.Forbidden,
					Content = new StringContent(context.Exception.Message)
				});
			}
			else
			{
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError,
					Content = new StringContent(context.Exception.Message)
				});
			}
		}
	}
}

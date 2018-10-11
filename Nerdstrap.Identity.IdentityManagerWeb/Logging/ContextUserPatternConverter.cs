using log4net.Core;
using log4net.Layout.Pattern;
using System.Threading;
using System.Web;

namespace Nerdstrap.Identity.IdentityManagerWeb.Logging
{
    public class ContextUserPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter textWriter, LoggingEvent loggingEvent)
        {
            var logData = loggingEvent.RenderedMessage.Split('|');
            var logUserName = string.Empty;
            if (logData.Length > 1)
            {
                logUserName = logData[0];
            }

            var userName = string.Empty;
            var currentHttpContext = HttpContext.Current;
            if (currentHttpContext != null
                && currentHttpContext.User != null
                && currentHttpContext.User.Identity.IsAuthenticated)
            {
                userName = currentHttpContext.User.Identity.Name;
            }
            else
            {
                var currentThreadPincipal = Thread.CurrentPrincipal;
                if (currentThreadPincipal != null && currentThreadPincipal.Identity.IsAuthenticated)
                {
                    userName = currentThreadPincipal.Identity.Name;
                }
            }
            if (string.IsNullOrEmpty(userName))
            {
                userName = logUserName;
            }
            textWriter.Write(userName);
        }
    }
}

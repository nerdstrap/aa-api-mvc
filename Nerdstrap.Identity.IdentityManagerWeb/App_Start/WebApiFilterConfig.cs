using System.Web.Http.Filters;
using Nerdstrap.Identity.IdentityManagerWeb.Filters;
using Nerdstrap.Identity.IdentityManagerWeb.Security;

namespace Nerdstrap.Identity.IdentityManagerWeb
{
    public class WebApiFilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new BearerAuthenticationFilter());
            filters.Add(new ObscureExceptionAttribute());
            filters.Add(new TraceHttpActionAttribute());
        }
    }
}

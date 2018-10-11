using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Castle.Windsor;
using Nerdstrap.Identity.IdentityManagerWeb.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using Nerdstrap.Identity.IdentityManagerWeb.Security;

namespace Nerdstrap.Identity.IdentityManagerWeb
{
	public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            MapRoutes(config);
            RegisterControllerActivator(container);
            RegisterMessageHandlers(config);
            RegisterDefaultJsonResponse(config);
            RegisterServices(config);
        }

        private static void MapRoutes(HttpConfiguration config)
		{
#if DEBUG
            var cors = new EnableCorsAttribute("*", "*", "*", "*");
            config.EnableCors(cors);
#endif
            config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{Id}",
				defaults: new { id = RouteParameter.Optional }
				);
		}

		private static void RegisterControllerActivator(IWindsorContainer container)
		{
			GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(container));
        }

        private static void RegisterMessageHandlers(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new SlidingExpirationHandler());
        }

        private static void RegisterDefaultJsonResponse(HttpConfiguration config)
		{
			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}

		private static void RegisterServices(HttpConfiguration config)
		{
			config.Services.Add(typeof(IExceptionLogger), new WebApiGlobalExceptionLogger());
		}
	}
}

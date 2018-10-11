using Castle.Windsor.Installer;
using Nerdstrap.Identity.Common;
using Nerdstrap.Identity.Web.Mvc;
using Nerdstrap.Identity.IdentityManagerWeb.DependencyInjection;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nerdstrap.Identity.IdentityManagerWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string APPLICATION_FAILED_TO_START_ERROR_MESSAGE = "Application failed to start.";
        private const string UNHANDLED_APPLICATION_EXCEPTION_MESSAGE = "Unhandled Application Exception";
        private static ILog _logger;

        public ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(typeof(MvcApplication))); }
            set { _logger = value; }
        }

        protected void Application_Start(Object sender, EventArgs e)
        {
            System.Diagnostics.Debugger.Break();
            ConfigureLogging();
            ConfigureSerialization(GlobalConfiguration.Configuration);
            ConfigureViewEngines();
            try
            {
                ConfigureDependencyInjection();
                ConfigureRegistrations();
            }
            catch (Exception genericException)
            {
                Logger.Fatal(APPLICATION_FAILED_TO_START_ERROR_MESSAGE, genericException);
                throw;
            }
        }

        protected void Application_Error()
        {
            Logger.Error(UNHANDLED_APPLICATION_EXCEPTION_MESSAGE, Server.GetLastError());
        }

        private void ConfigureLogging()
        {
            XmlConfigurator.Configure();
        }

        void ConfigureSerialization(HttpConfiguration config)
        {
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
            jsonSetting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings = jsonSetting;
        }

        private static void ConfigureDependencyInjection()
        {
            var controllerFactory = new WindsorControllerFactory(IocContainer.Container.Kernel);
            var resolver = new WindsorDependencyResolver(IocContainer.Container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            DependencyResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            IocContainer.Container.Install(FromAssembly.This());
        }

        private static void ConfigureRegistrations()
        {
            GlobalConfiguration.Configure(configurationCallback =>
            {
                var container = IocContainer.Container;
                WebApiConfig.Register(configurationCallback, container);
                WebApiFilterConfig.RegisterGlobalFilters(configurationCallback.Filters);
            });
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private static void ConfigureViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());
        }
    }
}

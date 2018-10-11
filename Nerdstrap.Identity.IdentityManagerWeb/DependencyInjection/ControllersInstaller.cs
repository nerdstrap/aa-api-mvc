using System.Web.Http;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Nerdstrap.Identity.IdentityManagerWeb.DependencyInjection
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                     .BasedOn<IController>()
                                     .LifestylePerWebRequest());
            container.Register(Classes.FromThisAssembly()
                                      .BasedOn<ApiController>()
                                      .LifestylePerWebRequest());
        }
    }
}

using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using AutoMapper;

namespace Nerdstrap.Identity.IdentityManagerWeb.DependencyInjection
{
    public class MappingEngineInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            container.Register(Component.For<IMapper>().UsingFactoryMethod(c =>
            {
                return AutoMapperConfig.Initialize().CreateMapper();
            }));
        }
    }
}

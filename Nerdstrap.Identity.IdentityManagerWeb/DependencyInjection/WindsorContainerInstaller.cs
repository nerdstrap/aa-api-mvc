using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;

namespace Nerdstrap.Identity.IdentityManagerWeb.DependencyInjection
{
	public class WindsorContainerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}
			container.Register(Component.For<IWindsorContainer>().Instance(container).LifestyleSingleton());
		}
	}
}

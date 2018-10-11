using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nerdstrap.Identity.Services.RiskAnalysis;
using Nerdstrap.Identity.Services.RiskAnalysis.Proxies;
using System;

namespace NerdstrapSoftware.IdentityManagement.IdentityManagerWeb.DependencyInjection
{
	public class ServiceProxyInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
            }

            container.Register(Component.For<AdaptiveAuthenticationInterface>()
                    .ImplementedBy<AdaptiveAuthenticationInterfaceClient>()
                    .DependsOn(Dependency.OnValue("endpointConfigurationName", "AdaptiveAuthentication"))
                    .Named("AdaptiveAuthenticationInterface")
                    .LifestylePerWebRequest()
                );

            container.Register(
                Component.For<IAdaptiveAuthenticationService>()
                .ImplementedBy<AdaptiveAuthenticationService>().DependsOn(
                    Dependency.OnComponent("client", "AdaptiveAuthenticationInterface"),
                    Dependency.OnAppSettingsValue("orgName"),
                    Dependency.OnAppSettingsValue("apiUserName"),
                    Dependency.OnAppSettingsValue("apiCredentials"),
                    Dependency.OnAppSettingsValue("numberOfQuestion")
                    )
                .Named("IAdaptiveAuthenticationService")
                .LifestylePerWebRequest());
        }
	}
}

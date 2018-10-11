using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nerdstrap.Identity.Services.DirectoryAccess;
using System;

namespace NerdstrapSoftware.IdentityManagement.IdentityManagerWeb.DependencyInjection
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Register(
                Component.For<IDirectoryService>()
                .ImplementedBy<DirectoryService>().DependsOn(
                    Dependency.OnAppSettingsValue("directoryServers"),
                    Dependency.OnAppSettingsValue("adminUserDistinguishedName"),
                    Dependency.OnAppSettingsValue("adminUserCredentials"),
                    Dependency.OnAppSettingsValue("usersDistinguishedName"),
                    Dependency.OnAppSettingsValue("groupsDistinguishedName"),
                    Dependency.OnAppSettingsValue("passwordPolicyDistinguishedName"),
                    Dependency.OnAppSettingsValue("authorizedGroups"),
                    Dependency.OnAppSettingsValue("connectionAttempts")
                    )
                .Named("IDirectoryService")
                .LifestylePerWebRequest());
        }
    }
}

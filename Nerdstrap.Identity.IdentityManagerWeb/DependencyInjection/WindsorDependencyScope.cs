using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

namespace Nerdstrap.Identity.IdentityManagerWeb.DependencyInjection
{
    public class WindsorDependencyScope : IDependencyScope
    {
        private readonly IKernel Container;
        private readonly IDisposable Scope;

        public WindsorDependencyScope(IKernel container)
        {
            Container = container;
            Scope = container.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            return Container.HasComponent(serviceType) ? Container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            Scope.Dispose();
        }
    }
}

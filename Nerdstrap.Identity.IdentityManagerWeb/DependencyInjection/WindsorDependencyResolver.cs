using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace Nerdstrap.Identity.IdentityManagerWeb.DependencyInjection
{
	internal class WindsorDependencyResolver : IDependencyResolver, System.Web.Mvc.IDependencyResolver
	{
		private readonly IKernel Container;

		public WindsorDependencyResolver(IKernel container)
		{
			Container = container;
		}

		public IDependencyScope BeginScope()
		{
			return new WindsorDependencyScope(Container);
		}

		public object GetService(Type serviceType)
		{
			return Container.HasComponent(serviceType) ? Container.Resolve(serviceType) : null;
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return Container.ResolveAll(serviceType).Cast<object>();
		}

		public void Dispose() {/* Required by System.Web.Http.Dependencies.IDependencyResolver, which implements IDisposable */ }
	}
}

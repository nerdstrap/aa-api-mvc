using Castle.MicroKernel;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Nerdstrap.Identity.Common
{
    public static class IocContainer
    {
        static IocContainer()
        {
            InitContainer();
        }

        public static IWindsorContainer Container { get; private set; }

        public static IKernel Kernel
        {
            get { return Container.Kernel; }
        }

        private static void InitContainer()
        {
            if (null == Container)
            {
                Container = new WindsorContainer().Install(FromAssembly.This());
            }
        }
    }
}

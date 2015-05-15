using AudioLogger.Services;
using Ini;
using Microsoft.Practices.Unity;

namespace AudioLogger
{
    internal class Bootstrap
    {
        private readonly IUnityContainer _container;

        public Bootstrap()
        {
            _container = new UnityContainer();
            _container.RegisterType<ApplicationForm>();
            _container.RegisterType<IFtpClientService, FtpClientService>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new IniFile(Configuration.Default.IniFilename)));
        }

        public IUnityContainer Container()
        {
            return _container;
        }
    }
}
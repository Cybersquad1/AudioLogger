using System;
using AudioLogger.Aplication;
using AudioLogger.Services;
using Ini;
using log4net;
using Microsoft.Practices.Unity;

namespace AudioLogger
{
    public class Bootstrap
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (Bootstrap));
        private readonly IUnityContainer _container;

        public Bootstrap()
        {
            try
            {
                _container = new UnityContainer();
                _container.RegisterType<ApplicationForm>();
                _container.RegisterType<IFtpClientService, FtpClientService>(
                    new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(new IniFile(Configuration.Default.IniFilename)));
                _container.RegisterType<IRecorderService, RecorderService>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IConverterService, ConverterService>(new ContainerControlledLifetimeManager());
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        public IUnityContainer Container()
        {
            return _container;
        }
    }
}
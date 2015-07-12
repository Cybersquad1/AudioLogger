using System;
using AudioLogger.Services;
using log4net;
using Microsoft.Practices.Unity;

namespace AudioLogger.Application
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
                _container.RegisterType<IRecorderService, RecorderService>(new TransientLifetimeManager());
                _container.RegisterType<IConverterService, ConverterService>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IEncryptionService, EncryptionService>(new ContainerControlledLifetimeManager());
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
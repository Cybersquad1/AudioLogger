using System;
using System.IO;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Microsoft.Practices.Unity;

namespace AudioLogger.Application
{
    internal static class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (Program));

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            CreateProgramDataFolderIfMissing();

            XmlConfigurator.Configure(new FileInfo(Configuration.Default.LogFilename));
            Logger.Info("Starting application");

            try
            {
                var bootstrap = new Bootstrap();
                var container = bootstrap.Container();

                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run(container.Resolve<ApplicationForm>());
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                MessageBox.Show(e.Message);
            }

            Logger.Info("Exiting application");
        }

        private static void CreateProgramDataFolderIfMissing()
        {
            if (!Directory.Exists(Configuration.Default.TemporaryFolder))
            {
                Directory.CreateDirectory(Configuration.Default.TemporaryFolder);
            }

            if (!Directory.Exists(Configuration.Default.LogFolder))
            {
                Directory.CreateDirectory(Configuration.Default.LogFolder);
            }
        }
    }
}
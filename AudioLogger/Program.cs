using System;
using System.Windows.Forms;
using log4net;
using Microsoft.Practices.Unity;

namespace AudioLogger
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
            try
            {
                var bootstrap = new Bootstrap();
                var container = bootstrap.Container();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(container.Resolve<ApplicationForm>());
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }
}
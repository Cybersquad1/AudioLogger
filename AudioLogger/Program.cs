using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;

namespace AudioLogger
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Bootstrap bootstrap = new Bootstrap();
            var container = bootstrap.Container();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<ApplicationForm>());
        }
    }
}
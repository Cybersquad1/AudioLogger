using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using Ini;

namespace Ftp
{
    public class FtpHandler
    {

        public bool Upload(string source, string filename)
        {

            IniFile ini = new IniFile(System.IO.Directory.GetCurrentDirectory() + "/config.ini");
            string host = ini.IniReadValue("ftp", "host");
            string targetDir = ini.IniReadValue("ftp", "targetDir");
            string user = ini.IniReadValue("ftp", "user");
            string pass = ini.IniReadValue("ftp", "pass");

            using (WebClient ftpClient = new WebClient())
            {

                ftpClient.Credentials = new NetworkCredential(user, pass);
                try
                {
                    ftpClient.Proxy = null;
                    ftpClient.UploadFile("ftp://" + host + "/" + targetDir + filename, "STOR", source);

                }
                catch (Exception ftpException)
                {
                    Debug.WriteLine(ftpException);
                    MessageBox.Show(ftpException.Message);
                    return false;
                }
                return true;
            }
        }
    }
}

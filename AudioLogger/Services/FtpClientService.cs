using System;
using System.IO;
using System.Net;
using Ini;
using log4net;

namespace AudioLogger.Services
{
    public class FtpClientService : IFtpClientService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (FtpClientService));
        private readonly WebClient _client;

        public FtpClientService(IniFile iniFile)
        {
            if (iniFile == null) throw new ArgumentException("iniFile");

            Host = iniFile.IniReadValue("ftp", "host");
            TargetDirectory = iniFile.IniReadValue("ftp", "targetDir");
            Username = iniFile.IniReadValue("ftp", "user");
            Password = iniFile.IniReadValue("ftp", "pass");
            _client = new WebClient();
        }

        private string Host { get; set; }
        private string TargetDirectory { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }

        public bool TryUploadFile(string source, string destination)
        {
            _client.Credentials = new NetworkCredential(Username, Password);
            try
            {
                _client.Proxy = null;
                _client.UploadFile(string.Format("ftp://{0}/{1}{2}",
                    Host,
                    TargetDirectory,
                    destination),
                    "STOR", source);
            }
            catch (WebException exception)
            {
                Logger.Warn(exception.Message);
                return false;
            }
            return true;
        }

        public int RemoveFilesOlderThan(DateTime date)
        {
            var webRequest = WebRequest.Create(string.Format("ftp://{0}/{1}", Host, TargetDirectory)) as FtpWebRequest;
            if (webRequest == null) throw new WebException("Failed to create a web request");
            webRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            webRequest.Credentials = new NetworkCredential(Username, Password);

            var webResponse = webRequest.GetResponse() as FtpWebResponse;
            if (webResponse != null)
            {
                using (var stream = webResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            Logger.Info(streamReader.ReadToEnd());
                        }
                    }
                    else throw new WebException("Failed to get the response stream");
                }
            }
            else throw new WebException("Failed to get a response");
        }
    }
}
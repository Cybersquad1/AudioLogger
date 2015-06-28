using System;
using System.IO;
using System.Linq;
using System.Net;
using Ini;
using log4net;

namespace AudioLogger.Services
{
    public class FtpClientService : IFtpClientService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (FtpClientService));
        private WebClient _client;

        private string _host;
        private string _targetDirectory;
        private string _username;
        private string _password;

        public void Setup(string host, string targetDir, string username, string password)
        {
            _host = host;
            _targetDirectory = targetDir;
            _username = username;
            _password = password;
        }

        public bool TryUploadFile(string source)
        {
            _client = new WebClient
            {
                Credentials = new NetworkCredential(_username, _password),
                Proxy = null
            };
            try
            {
                var address = string.Format("ftp://{0}/{1}/{2}",
                    _host,
                    _targetDirectory,
                    source.Split('\\').Last());
                _client.UploadFile(address,
                    "STOR", source);
            }
            catch (Exception exception)
            {
                Logger.Warn(exception.Message);
                return false;
            }
            return true;
        }

        public int RemoveFilesOlderThan(DateTime date)
        {
            var webRequest = WebRequest.Create(string.Format("ftp://{0}/{1}", _host, _targetDirectory)) as FtpWebRequest;
            if (webRequest == null) throw new WebException("Failed to create a web request");
            webRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            webRequest.Credentials = new NetworkCredential(_username, _password);

            var count = 0;
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
                            // This feature is still missing, it only logs what is going to be sent
                        }
                    }
                    else throw new WebException("Failed to get the response stream of directory listing");
                }
            }
            else throw new WebException("Failed to get a response of directory listing");

            // Here we can procede to delete the files
            // webRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            return count;
        }
    }
}
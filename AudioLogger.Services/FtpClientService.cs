using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using log4net;

namespace AudioLogger.Services
{
    public class FtpClientService : IFtpClientService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (FtpClientService));
        private WebClient _client;
        private string _host;
        private string _password;
        private string _targetDirectory;
        private string _username;
        private string _format;

        public void Setup(string host, string targetDir, string username, string password, string format)
        {
            _host = host;
            _targetDirectory = targetDir;
            _username = username;
            _password = password;
            _format = format;
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
            webRequest.Method = WebRequestMethods.Ftp.ListDirectory;
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
                            string line;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                var fullFileName = line.Split('/').Last();
                                var file = fullFileName.Split('.').First();

                                DateTime fileTime;
                                if (!DateTime.TryParseExact(file, _format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out fileTime))
                                {
                                    Logger.Warn(string.Format("Malformed file name {0}", file));
                                    continue;
                                }
                                if (fileTime.CompareTo(date) < 0)
                                {
                                    var deleteRequest =
                                        WebRequest.Create(string.Format("ftp://{0}/{1}", _host, line)) as FtpWebRequest;
                                    if (deleteRequest == null) throw new WebException("Failed to create a web request");
                                    deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                                    deleteRequest.Credentials = new NetworkCredential(_username, _password);
                                    var deleteResponse = deleteRequest.GetResponse();

                                    count++;
                                    Logger.Info(string.Format("Removing file {0}", fullFileName));
                                }
                            }
                        }
                    }
                    else throw new WebException("Failed to get the response stream of directory listing");
                }
            }
            else throw new WebException("Failed to get a response of directory listing");

            return count;
        }
    }
}
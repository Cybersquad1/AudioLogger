using System;
using System.Globalization;
using System.IO;
using System.Linq;
using log4net;

namespace AudioLogger.Services
{
    public class WinDirectoryService : IWinDirectoryService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (WinDirectoryService));
        private string _destinationDirectory;
        private string _format;

        public void Setup(string directory, string format)
        {
            _destinationDirectory = directory;
        }

        public bool TryUploadFile(string source)
        {
            try
            {
                var fullpath = string.Format("{0}\\{1}", _destinationDirectory, source.Split('\\').Last());
                var complete = File.ReadAllBytes(source);
                File.WriteAllBytes(fullpath, complete);
                    
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return false;
            }
            return true;
        }

        public int RemoveFilesOlderThan(DateTime date)
        {
            if (_destinationDirectory == null) return 0;
            var numberOfFilesRemoved = 0;
            var dirInfo = new DirectoryInfo(_destinationDirectory);
            var allFiles = dirInfo.GetFiles("*.mp3", SearchOption.AllDirectories);
            foreach (var file in allFiles)
            {
                DateTime fileTime;
                if (!DateTime.TryParseExact(file.Name, _format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out fileTime))
                {
                    Logger.Warn(string.Format("Malformed file name {0}", file.FullName));
                    continue;
                }
                if (fileTime.CompareTo(date) > 0)
                {
                    file.Delete();
                    numberOfFilesRemoved++;
                    Logger.Info(string.Format("Removing file {0}", file.FullName));
                }
            }
            return numberOfFilesRemoved;
        }
    }
}
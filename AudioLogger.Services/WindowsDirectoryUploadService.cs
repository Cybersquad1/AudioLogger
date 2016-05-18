using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using log4net;
using Microsoft.Practices.ObjectBuilder2;

namespace AudioLogger.Services
{
    public class WindowsDirectoryUploadService : IUploadService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (WindowsDirectoryUploadService));
        private readonly string _destinationDirectory;
        private readonly string _format;

        public WindowsDirectoryUploadService(Parameters parameters)
        {
            _destinationDirectory = parameters.WindowsDirectoryUploadTarget;
            _format = parameters.FileNameFromDateFormat;
        }

        public bool TryUploadFile(AudioLog audioLog)
        {
            try
            {
                var fullpath = $"{GetFolder(audioLog.TimeOfLog)}\\{Path.GetFileName(audioLog.GetMp3())}";
                File.Copy(audioLog.GetMp3(), fullpath);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return false;
            }
            return true;
        }

        private string GetFolder(DateTime time)
        {
            var dir = $"{_destinationDirectory}\\{time.ToString("yyyy-MM-dd")}";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }

        public bool TestConnection()
        {
            return Directory.Exists(_destinationDirectory);
        }

        public bool TryRemoveFiles(IEnumerable<AudioLog> files)
        {
            try
            {
                var dayGroup = files.GroupBy(log => log.TimeOfLog.ToString("yyyy-MM-dd"));
                var folders = dayGroup.Select(logs => GetFolder(logs.First().TimeOfLog));
                folders.ForEach(folder => Directory.Delete(folder, true));
            }
            catch (Exception ex)
            {
                Logger.Warn("Failed to delete files");
                Logger.Warn(ex.Message);
                return false;
            }
            return true;
        }

        public IEnumerable<AudioLog> GetFilesOlderThan(DateTime date)
        {
            if (_destinationDirectory == null) throw new ArgumentException("Destination directory is not set");
            var dirInfo = new DirectoryInfo(_destinationDirectory);
            var allFiles = dirInfo.GetFiles("*.mp3", SearchOption.AllDirectories);
            foreach (var file in allFiles)
            {
                DateTime fileTime;
                if (!DateTime.TryParseExact(file.Name.Split(Char.Parse(".")).First(), _format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out fileTime))
                {
                    Logger.Warn($"Malformed file name {file.Name}");
                    continue;
                }
                if (fileTime.CompareTo(date) < 0)
                {
                    yield return AudioLog.FromPath(file.Name);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using log4net;

namespace AudioLogger.Services
{
    public enum AudioLogFormat
    {
        Mp3,
        Wav
    }

    public class AudioLog
    {
        private readonly string _prefix;
        private DateTime _timeOfLog;

        public static string DateFormat { get; set; } = "yyyyMMdd-HHmmss";

        public AudioLogFormat Format { get; }

        public AudioLog(string prefix, DateTime timeOfLog, AudioLogFormat format)
        {
            _prefix = prefix;
            _timeOfLog = timeOfLog;
            Format = format;
        }

        private string GetFilename()
        {
            return $"{_prefix}\\{_timeOfLog.ToString(DateFormat)}";
        }

        public string GetWav()
        {
            return GetFilename() + ".wav";
        }

        public string GetMp3()
        {
            return GetFilename() + ".mp3";
        }

        public static AudioLog FromPath(string fullName)
        {
            var time = DateTime.ParseExact(fullName, DateFormat, CultureInfo.InvariantCulture);
            var path = Path.GetPathRoot(fullName);
            var extension = Path.GetExtension(fullName);
            return new AudioLog(path, time, GetFormat(extension));
        }

        private static AudioLogFormat GetFormat(string extension)
        {
            switch (extension)
            {
                case "mp3":
                    return AudioLogFormat.Mp3;
                default:
                    return AudioLogFormat.Wav;
            }
        }
    }

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
                var fullpath = $"{_destinationDirectory}\\{Path.GetFileName(audioLog.GetMp3())}";
                File.Copy(audioLog.GetMp3(), fullpath);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return false;
            }
            return true;
        }

        public bool TestConnection()
        {
            return Directory.Exists(_destinationDirectory);
        }

        public bool TryRemoveFiles(IEnumerable<AudioLog> files)
        {
            try
            {
                foreach (var file in files)
                {
                    File.Delete(file.GetMp3());
                }
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
                    Logger.Warn($"Malformed file name {file.FullName}");
                    continue;
                }
                if (fileTime.CompareTo(date) < 0)
                {
                    yield return AudioLog.FromPath(file.FullName);
                }
            }
        }
    }
}
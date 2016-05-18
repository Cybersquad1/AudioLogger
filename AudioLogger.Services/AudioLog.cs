using System;
using System.Globalization;
using System.IO;

namespace AudioLogger.Services
{
    public class AudioLog
    {
        private readonly string _prefix;
        private DateTime _timeOfLog;

        public static string DateFormat { get; set; } = "yyyyMMdd-HHmmss";

        public AudioLogFormat Format { get; }

        public DateTime TimeOfLog => _timeOfLog;

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
            var noExt = Path.GetFileNameWithoutExtension(fullName);
            var time = DateTime.ParseExact(noExt, DateFormat, CultureInfo.InvariantCulture);
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
}
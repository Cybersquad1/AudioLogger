namespace AudioLogger.Services
{
    public class Parameters
    {
        public string FtpHost { get; set; }
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public string FtpTargetDirectory { get; set; }
        public string WindowsDirectoryUploadTarget { get; set; }
        public string UploadType { get; set; }
        public string FileNameFromDateFormat { get; set; }
        public string TemporaryFolder { get; set; }
        public int RecordingDurationInMinutes { get; set; }
        public int RetentionRateInDays { get; set; }
    }
}
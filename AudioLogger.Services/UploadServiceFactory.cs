using System;

namespace AudioLogger.Services
{
    public class UploadServiceFactory
    {
        public static IUploadService CreateUploadService(Parameters appParameters)
        {
            IUploadService uploadService;
            switch (appParameters.UploadType)
            {
                case "FTP":
                    uploadService = new FtpUploadService(appParameters);
                    break;
                case "Windows directory":
                    uploadService = new WindowsDirectoryUploadService(appParameters);
                    break;
                default:
                    uploadService = null;
                    break;
            }

            if (uploadService == null)
                throw new ArgumentException("Failed to construct upload service, type does not exist");
            return uploadService;
        }
    }
}
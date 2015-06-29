using System;

namespace AudioLogger.Services
{
    public interface IFtpClientService
    {
        void Setup(string host, string targetDir, string username, string password, string format);
        bool TryUploadFile(string source);
        int RemoveFilesOlderThan(DateTime date);
    }
}
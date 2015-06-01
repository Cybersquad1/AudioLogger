using System;

namespace AudioLogger.Services
{
    public interface IFtpClientService
    {
        bool TryUploadFile(string source, string destination);
        int RemoveFilesOlderThan(DateTime date);
    }
}
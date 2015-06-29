using System;

namespace AudioLogger.Services
{
    public interface IWinDirectoryService
    {
        void Setup(string directory, string format);
        bool TryUploadFile(string source);
        int RemoveFilesOlderThan(DateTime date);
    }
}
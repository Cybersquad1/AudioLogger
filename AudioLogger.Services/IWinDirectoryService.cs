using System;

namespace AudioLogger.Services
{
    public interface IWinDirectoryService
    {
        void Setup(string directory);
        bool TryUploadFile(string source);
        int RemoveFilesOlderThan(DateTime date);
    }
}
using System;

namespace AudioLogger.Services
{
    public interface IUploadService
    {
        bool TryUploadFile(string source);
        int RemoveFilesOlderThan(DateTime date);
    }
}
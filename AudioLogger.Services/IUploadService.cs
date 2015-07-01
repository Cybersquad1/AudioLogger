using System;

namespace AudioLogger.Services
{
    internal interface IUploadService
    {
        bool TryUploadFile(string source);
        int RemoveFilesOlderThan(DateTime date);
    }
}
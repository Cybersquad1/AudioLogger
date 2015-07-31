using System;
using System.Collections.Generic;

namespace AudioLogger.Services
{
    public interface IUploadService
    {
        bool TryUploadFile(string source);
        bool TestConnection();
        IEnumerable<string> GetFilesOlderThan(DateTime date);
        bool RemoveFiles(IEnumerable<string> files);
    }
}
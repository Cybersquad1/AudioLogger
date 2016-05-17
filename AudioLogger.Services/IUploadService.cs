using System;
using System.Collections.Generic;

namespace AudioLogger.Services
{
    public interface IUploadService
    {
        bool TryUploadFile(AudioLog audioLog);
        bool TestConnection();
        IEnumerable<AudioLog> GetFilesOlderThan(DateTime date);
        bool TryRemoveFiles(IEnumerable<AudioLog> files);
    }
}
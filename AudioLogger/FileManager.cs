using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioLogger.Services;
using log4net;

namespace AudioLogger.Application
{
    public class FileManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FileManager));
        private static readonly Configuration ApplicationConfiguration = Configuration.Default;

        private static void CopyToBackupDir(IEnumerable<AudioLog> files)
        {
            if (!Directory.Exists(ApplicationConfiguration.BackupDir.GetProgramDataSubFolder()))
                Directory.CreateDirectory(ApplicationConfiguration.BackupDir.GetProgramDataSubFolder());
            foreach (var file in files)
                File.Copy(file.GetMp3(), ApplicationConfiguration.BackupDir.GetProgramDataSubFolder());
        }

        public static void TryDeleteOldFiles(IUploadService uploadService, DateTime endOfLifeDate)
        {
            try
            {
                var files =
                    uploadService.GetFilesOlderThan(endOfLifeDate).ToArray();
                if (!uploadService.TryRemoveFiles(files))
                {
                    CopyToBackupDir(files);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }
    }
}
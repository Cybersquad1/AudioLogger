using System;

namespace AudioLogger.Application
{
    public static class Extentions
    {
        public static string GetProgramDataSubFolder(this string folderName)
        {
            return string.Format("{0}\\{1}", Configuration.Default.ProgramDataFolder, folderName);
        }

        public static string GetProgramFolder()
        {
            return Configuration.Default.ProgramDataFolder;
        }
    }
}
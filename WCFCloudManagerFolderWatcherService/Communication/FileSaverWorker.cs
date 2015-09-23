using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFCloudManagerFolderWatcherService.Communication
{
    class FileSaverWorker
    {
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "CloudManagerTEMPLIST.cloud";
        public static string folderWatchPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "FolderWatchPath.cloud";
        public static string logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "CloudManagerLogFile.cloud";
        public static void WriteFile()
        {
            if (!File.Exists(path))
            {
                File.WriteAllLines(path, ListOfManipulatedFiles.UniqueInstance.GetFilesAsStringArr());
            }
            File.AppendAllLines(path, ListOfManipulatedFiles.UniqueInstance.GetFilesAsStringArr());

            ListOfManipulatedFiles.UniqueInstance.ResetList();
        }

        public static string[] ReadFile()
        {
            if (!File.Exists(path))
            {
                return null;
            }

            string[] temp = File.ReadAllLines(path);
            File.WriteAllLines(path, new string[] { });

            return temp;
        }
        
        public static string GetFolderWatcherPath()
        {
            if (!File.Exists(folderWatchPath))
            {
                return null;
            }

            string[] temp = File.ReadAllLines(folderWatchPath);
            //File.WriteAllLines(folderWatchPath, new string[] { });

            return temp[0];
        }

        public static void WriteFolderWatcherPath(string p)
        {
            File.WriteAllText(folderWatchPath, p + Environment.NewLine);
        }
    }
}

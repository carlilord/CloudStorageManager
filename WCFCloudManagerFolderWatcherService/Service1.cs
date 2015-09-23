using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCFCloudManagerFolderWatcherService.Communication;

namespace WCFCloudManagerFolderWatcherService
{
    public partial class Service1 : ServiceBase
    {
        private ListOfManipulatedFiles lomf = null;

        public Service1()
        {
            InitializeComponent();
            lomf = ListOfManipulatedFiles.UniqueInstance;
            string p = FileSaverWorker.GetFolderWatcherPath();

            if(!string.IsNullOrEmpty(p))
            {
                fileSystemWatcher.Path = p;
            }
            System.IO.File.AppendAllText(FileSaverWorker.logFilePath, "[Info] Current MainFolder: " + fileSystemWatcher.Path + Environment.NewLine);
        }

        protected override void OnStart(string[] args)
        {
            (new Thread(() =>
            {
                if (args.Length > 0)
                {
                    if (System.IO.Directory.Exists(args[0]))
                    {
                        fileSystemWatcher.Path = args[0];
                        FileSaverWorker.WriteFolderWatcherPath(args[0]);
                        lomf.ResetList();
                    }
                }
                WCFHost.DoWork();
            })).Start();
        }

        protected override void OnStop()
        {
            (new Thread(() =>
            {
                WCFHost.CloseHost();
            })).Start();
        }

        private void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            lomf.AddFile("ch" + e.FullPath);
        }

        private void fileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            lomf.AddFile("cr" + e.FullPath);
        }

        private void fileSystemWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
            lomf.AddFile("de" + e.FullPath);
        }

        private void fileSystemWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            lomf.AddFile("re" + e.FullPath);
        }
    }
}

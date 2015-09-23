using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CloudManagerUI.Interfaces
{
    public class FileInformation
    {
        public String Path { get;private set; }
        public String Mode { get;private set; }
        public string FileName { get;private set; }
        public string FileID { get;private set; }
        public DateTime LastUpdated { get;private set; }
        public bool isFile { get;private set; }
        public string CloudPath { get; private set; }
        public string CloudName { get; private set; }

        public FileInformation(string path, string filename, string fileid, DateTime lastupdated, bool isFile)
        {
            this.FileName = filename;
            this.FileID = fileid;
            this.LastUpdated = lastupdated;
            this.isFile = isFile;
            //this.CloudPath = cloudpath;

            this.Path = path.Substring(2, path.Length - 2);
            this.Mode = path.Substring(0, 2);       //'ch' changed , 'cr' created , 'de' deleted , 're' renamed
        }

        public FileInformation(string filename, string fileid, DateTime lastupdated, string cloudpath, bool isFile,string mode)
        {
            this.FileName = filename;
            this.FileID = fileid;
            this.LastUpdated = lastupdated;
            this.isFile = isFile;
            this.CloudPath = cloudpath;
            this.Mode = mode;       //'ch' changed , 'cr' created , 'de' deleted , 're' renamed

            string temp = this.CloudPath.Replace("/", @"\\");
            this.Path = MainFolder.MainFolderPath + @"\\" + temp;
            
        }
        public FileInformation(string path) //add constructor for the local built list providing the upload data
        {
            this.Path = path.Substring(2, path.Length - 2);
            if(File.Exists(this.Path))
            {
                string temp = this.Path;
                string[] temps = temp.Split('\\');
                this.FileName = temps[temps.Length - 1];
                this.isFile = true;

            }
            else
            {
                this.isFile = false;
            }
            this.Mode = path.Substring(0, 2);       //'ch' changed , 'cr' created , 'de' deleted , 're' renamed
        }
      
    }

    interface ISynchronizationContract
    {
        Task<bool> UploadSynchronizationAsync(List<FileInformation> fileInfo, object DriveServiceClient);
        Task<bool> DownloadSynchronizationAsync(List<FileInformation> fileInfo, object DriveServiceClient);
        Task<long[]> DetermineAvailableCloudSpace(object DriveServiceClient);
    }
}

using CloudManagerUI.Interfaces;
using CloudManagerUI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudManagerUI.Intelligente_Algorithmen
{
    class IntelligentUpload
    {
        DatabaseEntry dbEntry;
        private CloudManagerViewModel ViewModel;
        public IntelligentUpload(CloudManagerViewModel model)
        {
            this.ViewModel = model;
            this.dbEntry = new DatabaseEntry();
        }
        public async void ReceivedListLocal(string[] list)
        {
            List<FileInformation> fileList = new List<FileInformation>();
            try
            {
                foreach (string item in list)
                {
                    FileInformation fi = new FileInformation(item);
                    fileList.Add(fi);
                    string cloud = await this.IntelligentAlgorithm();
                    this.dbEntry.execEntry(fi.FileName, fi.Path, fi.isFile, cloud);
                    await this.IntelligentUploadFunc(cloud,fi.Path,fi.FileName);
                }
            }
            catch(Exception e)
            {
                string carlminichochonis = e.Message;
            }

            

            await this.ViewModel.OneDriveInterfaceFunctions.UploadSynchronizationAsync(fileList, this.ViewModel.OneDrive.liveConnectClient);

            
        }

        private async Task IntelligentUploadFunc(string cloud,string path,string name)
        {
            if(cloud.Equals("OneDrive"))
            {
                using(FileStream fs = new FileStream(path,FileMode.Open))
                {
                    string cloudpath = path.Substring(MainFolder.MainFolderPath.Length, path.Length - MainFolder.MainFolderPath.Length);
                    string id = String.Empty;
                    if(cloudpath.Equals(name))
                    {
                        id = "me/skydrive";
                    }
                    else
                    {
                        id = await this.ViewModel.OneDriveInterfaceFunctions.GetSkyDriveFolderID(cloudpath, this.ViewModel.OneDrive.liveConnectClient);
                    }
                    
                    await this.ViewModel.OneDrive.liveConnectClient.UploadAsync(id,name,fs,Microsoft.Live.OverwriteOption.Overwrite);
                }
                
            }
            if (cloud.Equals("GoogleDrive"))
            {

            }
            if (cloud.Equals("Dropbox"))
            {

            }
        }

        private async Task<string> IntelligentAlgorithm()
        {
            //if abfragen speicher abfragen intelligenter algorithmus
            int storageGD = 0;
            int storageDB = 0;
            int storageOD = 0;

            try
            {
                long[] storageInfoGD = await this.ViewModel.GoogleDriveInterfaceFunctions.DetermineAvailableCloudSpace(this.ViewModel.GoogleDrive.Service);
                if (storageInfoGD != null)
                {
                    var availableGB = (double)((double)storageInfoGD[1] / (double)(1024 * 1024 * 1024));
                    var totalGB = (double)((double)storageInfoGD[0] / (double)(1024 * 1024 * 1024));

                    float available = (float)(Math.Truncate((double)availableGB * 100.0) / 100.0);
                    float total = (float)(Math.Truncate((double)totalGB * 100.0) / 100.0);

                    storageGD = (int)((double)((availableGB / totalGB) * 100));
                }

                long[] storageInfoDB = await this.ViewModel.DropboxInterfaceFunctions.DetermineAvailableCloudSpace(this.ViewModel.Dropbox.client);   //Speicherstand Dropbox
                if (storageInfoDB != null)
                {
                    var availableGB = (double)((double)storageInfoDB[1] / (double)(1024 * 1024 * 1024));  //verfügbarer Speicher
                    var totalGB = (double)((double)storageInfoDB[0] / (double)(1024 * 1024 * 1024));  //Gesamtspeicher

                    //Typkonvertierung
                    float available = (float)(Math.Truncate((double)availableGB * 100.0) / 100.0);
                    float total = (float)(Math.Truncate((double)totalGB * 100.0) / 100.0);

                    storageDB = (int)((double)((availableGB / totalGB) * 100)); //0%-100% Speicherauslastung
                }

                long[] storageInfoOD = await this.ViewModel.OneDriveInterfaceFunctions.DetermineAvailableCloudSpace(this.ViewModel.OneDrive.liveConnectClient);
                if (storageInfoOD != null)
                {
                    var availableGB = (double)((double)storageInfoOD[1] / (double)(1024 * 1024 * 1024));
                    var totalGB = (double)((double)storageInfoOD[0] / (double)(1024 * 1024 * 1024));

                    float available = (float)(Math.Truncate((double)availableGB * 100.0) / 100.0);
                    float total = (float)(Math.Truncate((double)totalGB * 100.0) / 100.0);

                    storageOD = (int)((double)((availableGB / totalGB) * 100));
                }

                if (storageOD >= storageDB && storageOD >= storageGD)
                {
                    return "OneDrive";
                }
                if (storageDB > storageOD && storageDB > storageGD)
                {
                    return "Dropbox";
                }
                if (storageGD > storageOD && storageGD > storageDB)
                {
                    return "GoogleDrive";
                }


                

            }
            catch (Exception e)
            {
                string hm = e.Message;
            }
            return "";
        }

            
    }
}

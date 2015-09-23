using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudManagerUI;
using Microsoft.Live;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections;
using System.ComponentModel;

namespace CloudManagerUI.OneDrive
{
    class OneDriveInterfaceFunctions : Interfaces.ISynchronizationContract
    {
        DatabaseEntry dbEntry;

        public OneDriveInterfaceFunctions()
        {
            this.dbEntry = new DatabaseEntry();
        }

        public async Task<string> GetSkyDriveFolderID(string folderName, LiveConnectClient client)
        {
            string[] folderNames = folderName.Split('/');
            string result = CloudManagerUI.Properties.Resources.OneDriveRoot;
            int i = 0;
            string cmpValue = String.Empty;

            while (!cmpValue.Equals(folderNames[folderNames.Length - 1]))
            {


                //operationResult = await this.liveConnectClient.GetAsync(result + "/files?filter=folders,albums"); //me/skydrive/files?filter=folders
                LiveOperationResult operationResult = await client.GetAsync(String.Format("{0}/files?filter=folders,albums", result)); //me/skydrive/files?filter=folders


                var iEnum = operationResult.Result.Values.GetEnumerator();
                iEnum.MoveNext();
                var folders = iEnum.Current as IEnumerable;

                foreach (dynamic v in folders)
                {
                    if (v.name == folderNames[i])
                    {
                        i++;
                        result = v.id as string;
                        cmpValue = v.name as string;        //debug test var
                        break;                              //updated time bei values von 'v' für synchronize fürs download


                    }
                }

            }
            if (result != null)
            {

                return result;
            }
            else
            {
                //LogOutput("not found");
                return null;
            }

        }

        private async Task<string> CreateDirectoryAsync(string folderName, string parentFolder, LiveConnectClient client)
        {
            string folderId = null;
            string queryFolder;
            LiveOperationResult opResult;
            //Retrieves all the directories.
            if (!parentFolder.Equals(CloudManagerUI.Properties.Resources.OneDriveRoot))
            {
                queryFolder = await GetSkyDriveFolderID(parentFolder, client) + "/files?filter=folders,albums";
                opResult = await client.GetAsync(queryFolder);
            }
            else
            {
                queryFolder = CloudManagerUI.Properties.Resources.OneDriveRoot + "/files?filter=folders,albums";
                opResult = await client.GetAsync(queryFolder);
            }


            dynamic result = opResult.Result;

            foreach (dynamic folder in result.data)
            {
                //Checks if current folder has the passed name.
                if (folder.name.ToLowerInvariant() == folderName.ToLowerInvariant())
                {
                    folderId = folder.id;
                    break;
                }
            }

            if (folderId == null)
            {
                //Directory hasn't been found, so creates it using the PostAsync method.
                var folderData = new Dictionary<string, object>();
                folderData.Add("name", folderName);

                if (parentFolder.Equals(CloudManagerUI.Properties.Resources.OneDriveRoot))
                {
                    var opResultPostRoot = await client.GetAsync(CloudManagerUI.Properties.Resources.OneDriveRoot);

                    var iEnum = opResultPostRoot.Result.Values.GetEnumerator();
                    iEnum.MoveNext();
                    var id = iEnum.Current.ToString();


                    opResult = await client.PostAsync(id, folderData);
                }
                else
                {
                    var opResultPost = await GetSkyDriveFolderID(parentFolder, client);

                    opResult = await client.PostAsync(opResultPost, folderData);
                }

                result = opResult.Result;

                //Retrieves the id of the created folder.
                folderId = result.id;
            }


            return folderId;
        }

        private async Task<DateTime> GetDateTime(string cloudpath,string filename,LiveConnectClient liveConnectClient)
        {
            DateTime ddt = new DateTime();
            try
            {
                LiveOperationResult lor = await liveConnectClient.GetAsync(cloudpath);

                DirectoryInfo dirin = new DirectoryInfo(@"C:\users\nico\desktop\office365 key.txt");
                DateTime dt = dirin.LastWriteTime;


                var iEnum = lor.Result.Values.GetEnumerator();
                iEnum.MoveNext();
                string temp = "";

                foreach(dynamic var in iEnum.Current as IEnumerable)
                {
                    if(var.name.Equals(filename))
                    {
                        temp = var.updated_time;
                    }
                    
                    
                }


                DateTimeConverter dtc = new DateTimeConverter();
                ddt = (DateTime)dtc.ConvertFromString(temp);


            }
            catch (Exception) { }

            return ddt;
        }


        //DB ENTRIES FÜR DIE FOLDER NOCH MACHEN !!!!!!!!!!!!!
        //public async Task<bool> UploadSynchronizationAsync(List<Interfaces.FileInformation> fileInfo, object DriveServiceClient)
        //{
        //    try
        //    {
        //        Interfaces.FileInformation[] fileArr = fileInfo.ToArray();
        //        LiveConnectClient client = DriveServiceClient as LiveConnectClient;

        //        foreach (var item in fileArr)
        //        {
        //            if (!item.isFile)
        //            {
        //                string cloudPath = item.Path.Substring(CloudManagerUI.Properties.Settings.Default.MainFolderLocation.Length + 1, item.Path.Length - CloudManagerUI.Properties.Settings.Default.MainFolderLocation.Length + 1);
        //                string[] pathFragments = cloudPath.Split('/');
        //                string pathFragment = CloudManagerUI.Properties.Resources.OneDriveRoot;
        //                int i = 0;


        //                switch (item.Mode) //DB ENTRIES NOCH MACHEN FÜR DIE FOLDER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //                {
        //                    case "cr":
        //                        foreach (var fragment in pathFragments)
        //                        {
        //                            if (i == 0)
        //                            {
        //                                i++;
        //                                await CreateDirectoryAsync(fragment, pathFragment, client);
        //                                pathFragment = String.Empty;
        //                            }
        //                            else
        //                            {
        //                                pathFragment += fragment;
        //                                await CreateDirectoryAsync(fragment, pathFragment, client);
        //                                pathFragment += "/";
        //                            }

        //                        }

        //                        break;

        //                    case "ch":
        //                        foreach (var fragment in pathFragments)
        //                        {
        //                            if (i == 0)
        //                            {
        //                                i++;
        //                                await CreateDirectoryAsync(fragment, pathFragment, client);
        //                                pathFragment = String.Empty;
        //                            }
        //                            else
        //                            {
        //                                pathFragment += fragment;
        //                                await CreateDirectoryAsync(fragment, pathFragment, client);
        //                                pathFragment += "/";
        //                            }

        //                        }
        //                        break;

        //                    case "re":
        //                        foreach (var fragment in pathFragments)
        //                        {
        //                            if (i == 0)
        //                            {
        //                                i++;
        //                                await CreateDirectoryAsync(fragment, pathFragment, client);
        //                                pathFragment = String.Empty;
        //                            }
        //                            else
        //                            {
        //                                pathFragment += fragment;
        //                                await CreateDirectoryAsync(fragment, pathFragment, client);
        //                                pathFragment += "/";
        //                            }

        //                        }
        //                        break;

        //                    case "de": await client.DeleteAsync(item.FileID);
        //                        break;
        //                }
        //            }


        //        }


        //        foreach (var item in fileArr)
        //        {
        //            if (item.isFile)
        //            {
        //                string cloudPath = item.Path.Substring(CloudManagerUI.Properties.Settings.Default.MainFolderLocation.Length + 1, item.Path.Length - CloudManagerUI.Properties.Settings.Default.MainFolderLocation.Length + 1);
        //                string id = await GetSkyDriveFolderID(cloudPath, client);
        //                bool root = false;
        //                if (!cloudPath.Contains('/'))
        //                {
        //                    root = true;
        //                }

        //                switch (item.Mode)
        //                {

        //                    case "ch":

        //                        using (FileStream stream = new FileStream(item.Path + "\\" + item.FileName, FileMode.Open))
        //                        {
        //                            if (stream != null)
        //                            {
        //                                await client.UploadAsync(id, item.FileName, stream, OverwriteOption.Overwrite);
        //                            }
        //                        }

        //                        this.dbEntry.execEntry(id, "OneDrive", item.FileName, item.Path + "\\" + item.FileName, item.LastUpdated, false, false, root, cloudPath);



        //                        break;

        //                    case "cr":

        //                        using (FileStream stream = new FileStream(item.Path + "\\" + item.FileName, FileMode.Open))
        //                        {
        //                            if (stream != null)
        //                            {
        //                                await client.UploadAsync(id, item.FileName, stream, OverwriteOption.Overwrite);
        //                            }
        //                        }

        //                        break;

        //                    case "de":

        //                        await client.DeleteAsync(id);
        //                        this.dbEntry.execEntry(id, "OneDrive", item.FileName, item.Path, item.LastUpdated, true, false, root, cloudPath);

        //                        break;

        //                    case "re":

        //                        using (FileStream stream = new FileStream(item.Path + "\\" + item.FileName, FileMode.Open))
        //                        {
        //                            if (stream != null)
        //                            {
        //                                await client.UploadAsync(id, item.FileName, stream, OverwriteOption.Overwrite);
        //                            }
        //                        }

        //                        this.dbEntry.execEntry(id, "OneDrive", item.FileName, item.Path + "\\" + item.FileName, item.LastUpdated, false, false, root, cloudPath);
        //                        break;
        //                }
        //            }
        //        }


        //    }
        //    catch (Exception) { }

        //    return false;
        //}

        public async Task<bool> UploadSynchronizationAsync(List<Interfaces.FileInformation> fileInfo, object DriveServiceClient)
        {
            try
            {
                Interfaces.FileInformation[] fileArr = fileInfo.ToArray();
                LiveConnectClient client = DriveServiceClient as LiveConnectClient;

                foreach (var item in fileArr)
                {
                    if (!item.isFile)
                    {
                        string cloudPath = item.Path.Substring(MainFolder.MainFolderPath.Length + 1, item.Path.Length - MainFolder.MainFolderPath.Length + 1);
                        string[] pathFragments = cloudPath.Split('/');
                        string pathFragment = CloudManagerUI.Properties.Resources.OneDriveRoot;
                        int i = 0;


                        switch (item.Mode) //DB ENTRIES NOCH MACHEN FÜR DIE FOLDER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        {
                            case "cr":
                                foreach (var fragment in pathFragments)
                                {
                                    if (i == 0)
                                    {
                                        i++;
                                        await CreateDirectoryAsync(fragment, pathFragment, client);
                                        pathFragment = String.Empty;
                                    }
                                    else
                                    {
                                        pathFragment += fragment;
                                        await CreateDirectoryAsync(fragment, pathFragment, client);
                                        pathFragment += "/";
                                    }

                                }

                                break;

                            case "ch":
                                foreach (var fragment in pathFragments)
                                {
                                    if (i == 0)
                                    {
                                        i++;
                                        await CreateDirectoryAsync(fragment, pathFragment, client);
                                        pathFragment = String.Empty;
                                    }
                                    else
                                    {
                                        pathFragment += fragment;
                                        await CreateDirectoryAsync(fragment, pathFragment, client);
                                        pathFragment += "/";
                                    }

                                }
                                break;

                            case "re":
                                foreach (var fragment in pathFragments)
                                {
                                    if (i == 0)
                                    {
                                        i++;
                                        await CreateDirectoryAsync(fragment, pathFragment, client);
                                        pathFragment = String.Empty;
                                    }
                                    else
                                    {
                                        pathFragment += fragment;
                                        await CreateDirectoryAsync(fragment, pathFragment, client);
                                        pathFragment += "/";
                                    }

                                }
                                break;

                            case "de": await client.DeleteAsync(item.FileID);
                                break;
                        }
                    }


                }


                foreach (var item in fileArr)
                {
                    DirectoryInfo di = new DirectoryInfo(item.Path);
                    DateTime dt = di.LastWriteTime;


                    if (item.isFile)
                    {
                        string cloudPath = item.Path.Substring(MainFolder.MainFolderPath.Length + 1, item.Path.Length - MainFolder.MainFolderPath.Length + 1);
                        string id = await GetSkyDriveFolderID(cloudPath, client);
                        bool root = false;
                        if (!cloudPath.Contains('/'))
                        {
                            root = true;
                        }

                        switch (item.Mode)
                        {

                            case "ch":

                                using (FileStream stream = new FileStream(item.Path + "\\" + item.FileName, FileMode.Open))
                                {
                                    if (stream != null)
                                    {
                                        await client.UploadAsync(id, item.FileName, stream, OverwriteOption.Overwrite);
                                    }
                                }

                                this.dbEntry.execEntry(id, "OneDrive", item.FileName, item.Path + "\\" + item.FileName, dt, false, false, root, cloudPath);



                                break;

                            case "cr":

                                using (FileStream stream = new FileStream(item.Path + "\\" + item.FileName, FileMode.Open))
                                {
                                    if (stream != null)
                                    {
                                        await client.UploadAsync(id, item.FileName, stream, OverwriteOption.Overwrite);
                                    }
                                }

                                break;

                            case "de":

                                await client.DeleteAsync(id);
                                this.dbEntry.execEntry(id, "OneDrive", item.FileName, item.Path, dt, true, false, root, cloudPath);

                                break;

                            case "re":

                                using (FileStream stream = new FileStream(item.Path + "\\" + item.FileName, FileMode.Open))
                                {
                                    if (stream != null)
                                    {
                                        await client.UploadAsync(id, item.FileName, stream, OverwriteOption.Overwrite);
                                    }
                                }

                                this.dbEntry.execEntry(id, "OneDrive", item.FileName, item.Path + "\\" + item.FileName, dt, false, false, root, cloudPath);
                                break;
                        }
                    }
                }


            }
            catch (Exception) { }

            return false;
        }

        public async Task<bool> DownloadSynchronizationAsync(List<Interfaces.FileInformation> fileInfo, object DriveServiceClient)
        {
            try
            {
                Interfaces.FileInformation[] fileArr = fileInfo.ToArray();
                LiveConnectClient client = DriveServiceClient as LiveConnectClient;

                foreach (var item in fileArr)
                {
                    if (!item.isFile)
                    {
                        switch (item.Mode)
                        {
                            case "cr":
                                break;

                            case "ch":
                                break;

                            case "re":
                                break;

                            case "de":
                                break;
                        }
                    }


                }


                foreach (var item in fileArr)
                {
                    switch (item.Mode)
                    {
                        case "ch": //database entries
                            if (item.isFile)
                            {
                                using (FileStream stream = new FileStream(item.Path + item.FileName, FileMode.Open))
                                {
                                    if (stream != null)
                                    {
                                        LiveDownloadOperationResult result = await client.DownloadAsync(String.Format("{0}/content", item.FileID));
                                        await result.Stream.CopyToAsync(stream);


                                    }
                                }

                            }


                            break;
                        case "cr": //database entries
                            if (item.isFile)
                            {
                                //using (FileStream stream = new FileStream(item.Path + item.FileName, FileMode.Open))
                                //{
                                //    if (stream != null)
                                //    {
                                //        await client.UploadAsync(item.Path + "\\" + item.FileName, item.FileName, stream, OverwriteOption.Overwrite);
                                //    }
                                //}
                                FileStream stream = null;
                                if ((stream = new FileStream(item.Path + @"\" + item.FileName, FileMode.Create)) == null)
                                {

                                    throw new Exception("Unable to open the file selected to upload.");
                                }


                                //to download the file we need to use the id + "/content"
                                LiveDownloadOperationResult result = await client.DownloadAsync(string.Format("{0}/content", item.FileID));

                                await result.Stream.CopyToAsync(stream);

                            }

                            break;
                        case "de": //database entries
                            //await client.DeleteAsync(item.FileID);
                            break;
                        case "re": //database entries
                            if (item.isFile)
                            {
                                using (FileStream stream = new FileStream(item.Path + item.FileName, FileMode.Open))
                                {
                                    if (stream != null)
                                    {
                                        await client.UploadAsync(item.Path + "\\" + item.FileName, item.FileName, stream, OverwriteOption.Overwrite);
                                    }
                                }

                            }

                            break;
                    }
                }
            }
            catch (Exception) { }

            //return TaskEx.FromResult(false);

            return false;
        }


        public async Task<long[]> DetermineAvailableCloudSpace(object DriveServiceClient)
        {
            LiveConnectClient client = DriveServiceClient as LiveConnectClient;
            long[] space = new long[2];
            string url = String.Format("https://apis.live.net/v5.0/me/skydrive/quota?access_token={0}", client.Session.AccessToken);
            using (WebClient wc = new WebClient())
            {
                var response = wc.DownloadData(url);

                string json = System.Text.Encoding.UTF8.GetString(response);

                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

                dynamic jsonData = serializer.Deserialize(json, typeof(object));

                long total = jsonData.quota;
                long available = jsonData.available;

                space[0] = total;
                space[1] = available;

                //percentage = (available /total) * 100;

            }

            return space;
        }
    }
}

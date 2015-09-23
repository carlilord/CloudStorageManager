using CloudManagerUI.Interfaces;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudManagerUI.GoogleDrive
{
    class GoogleDriveInterfaceFunctions : ISynchronizationContract
    {
        private const int KB = 0x400;
        private const int DownloadChunkSize = 256 * KB;

        public async Task<bool> UploadSynchronizationAsync(List<FileInformation> fileInfo, object DriveServiceClient)
        {
            DriveService service = DriveServiceClient as DriveService;
            IList<File> fl = await this.RetrieveAllFilesAsList(service);
            About about = await service.About.Get().ExecuteAsync();
            String RootID = about.RootFolderId;
            String mainFolderPath = MainFolder.MainFolderPath;
            string cloudPath;


            foreach (var file in fileInfo)
            {
                cloudPath = cloudPath = file.Path.Substring(mainFolderPath.Length + 1, 
                                        file.Path.Length - mainFolderPath.Length + 1);
                string[] pathFragments = cloudPath.Split('/');
                string id;

                switch (file.Mode)
                {
                    case "ch":
                        if (file.isFile)
                        {
                            await this.PostFolders(service, pathFragments, mainFolderPath);
                            if (pathFragments.Length < 2)
                            {
                                id = RootID;
                            }
                            else
                            {
                                id = await this.getIdFromTitle(service, fl, pathFragments[pathFragments.Length - 2]);
                            }
                            await this.UploadFileAsync(service, file.Path, id);
                        }
                        break;

                    case "cr":
                        // uploads all folders
                        await this.PostFolders(service, pathFragments, mainFolderPath);

                        if (file.isFile)
                        {
                            if (pathFragments.Length < 2)
                            {
                                id = RootID;
                            }
                            else
                            {
                                id = await this.getIdFromTitle(service, fl, pathFragments[pathFragments.Length - 2]);
                            }
                            await this.UploadFileAsync(service, file.Path, id);
                        }
                        break;

                    case "de":
                        service.Files.Delete(file.FileID);
                        break;

                    case "re":
                        await this.RenameFile(service, file.FileName, file.FileID);
                        break;
                }
            }
            return true;
        }

        public async Task<bool> DownloadSynchronizationAsync(List<FileInformation> fileInfo, object DriveServiceClient)
        {
            DriveService service = DriveServiceClient as DriveService;
            IList<File> fl;
            About about = await service.About.Get().ExecuteAsync();
            String RootID = about.RootFolderId;
            String mainFolderPath = MainFolder.MainFolderPath;
            String path;

            foreach (var file in fileInfo)
            {
                fl = await this.RetrieveAllFilesAsList(service);

                // current file
                File f = await service.Files.Get(file.FileID).ExecuteAsync();
                path = this.getPathOfSingleFile(fl, f);

                switch (file.Mode)
                {
                    case "ch":
                        if (file.isFile)
                        {
                            string pathToFile = path.Remove(path.LastIndexOf('\\'));

                            if (!System.IO.Directory.Exists(path + pathToFile))
                            {
                                System.IO.Directory.CreateDirectory(path + pathToFile);
                            }
                            await DownloadFileAsync(service, f, mainFolderPath + path);
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(mainFolderPath + path))
                            {
                                System.IO.Directory.CreateDirectory(mainFolderPath + path);
                            }

                        }
                        break;

                    case "cr":
                        if (file.isFile)
                        {
                            string pathToFile = path.Remove(path.LastIndexOf('\\'));

                            if (!System.IO.Directory.Exists(path + pathToFile))
                            {
                                System.IO.Directory.CreateDirectory(path + pathToFile);
                            }
                            await DownloadFileAsync(service, f, mainFolderPath + path);
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(mainFolderPath + path))
                            { 
                                System.IO.Directory.CreateDirectory(mainFolderPath + path);
                            }
                            
                        }
                        break;

                    case "de":
                        if(file.isFile)
                        {
                            System.IO.File.Delete(mainFolderPath + path);
                        }
                        else
                        {
                            System.IO.Directory.Delete(mainFolderPath + path, true);
                        }
                        break;

                    case "re":
                        if (file.isFile)
                        {
                            System.IO.File.Move(mainFolderPath + file.Path, mainFolderPath + path);
                        }
                        else
                        {
                            System.IO.Directory.Move(mainFolderPath + file.Path, mainFolderPath + path);
                        }
                        break;
                }
            }
            return true;
        }

        public async Task<long[]> DetermineAvailableCloudSpace(object DriveServiceClient)
        {
            DriveService service = DriveServiceClient as DriveService;
            About about = await service.About.Get().ExecuteAsync();

            long[] l = new long[2];
            l[0] = (long)about.QuotaBytesTotal;
            l[1] = l[0] - (long)about.QuotaBytesUsed;

            return l;
        }

        //##############################################################################
        // Upload
        public async Task<IUploadProgress> UploadFileAsync(DriveService service, string path, string idOfParent)
        {
            dynamic task;
            IList<File> fl = await this.RetrieveAllFilesAsList(service);
            File file = new File();

            var title = path;

            if (title.LastIndexOf('\\') != -1)
            {
                title = title.Substring(title.LastIndexOf('\\') + 1);
            }
            else
            {
                throw new Exception("Path is wrong!");
            }

            using (var uploadStream = new System.IO.FileStream(path, System.IO.FileMode.Open,
                System.IO.FileAccess.Read))
            {
                file.Title = title;
                file.Parents = new List<ParentReference>() { new ParentReference() { Id = idOfParent } };
                bool fileExists = await this.FileExists(service, title, idOfParent);

                if (fileExists)
                {
                    Console.WriteLine("exists");
                    var update = service.Files.Update(file, await getIdFromTitle(service, fl, title), uploadStream, @"application/octet-stream");
                    update.ChunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize * 2;
                    task = await update.UploadAsync();
                }
                else
                {
                    Console.WriteLine(" not exists");
                    var insert = service.Files.Insert(file, uploadStream, @"application/octet-stream");
                    insert.ChunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize * 2;
                    task = await insert.UploadAsync();
                }

            }

            return task;
        }
        private async Task PostFolders(DriveService service, string[] files, string path)
        {
            int count = 0;
            bool bichl = false;
            File file = new File();
            IList<File> fl = await this.RetrieveAllFilesAsList(service);
            About about = await service.About.Get().ExecuteAsync();
            String RootID = about.RootFolderId;

            for (int i = 0; i < files.Length; i++)
            {
                FileAndFolderInformation fafi = new FileAndFolderInformation(files[i], path);
                bichl = false;

                foreach (string s in fafi.RelPathToData)
                {
                    if (!fafi.IsFile && !String.IsNullOrEmpty(s))
                    {
                        fl = await this.RetrieveAllFilesAsList(service);

                        if (bichl == false)
                        {
                            await CreateFolderGD(service, s, RootID);
                            bichl = true;
                        }
                        else
                        {
                            String id = await this.getIdFromTitle(service, fl, fafi.RelPathToData[count - 1]);
                            await CreateFolderGD(service, s, id);
                        }

                    }
                    count++;

                }
                count = 0;
            }

        }

        public async Task CreateFolderGD(DriveService service, string title, string idOfParent)
        {
            bool exists = await FolderExists(service, title, idOfParent);

            if (!exists)
            {
                File body = new File();
                body.Title = title;
                body.MimeType = "application/vnd.google-apps.folder";
                body.Parents = new List<ParentReference>() { new ParentReference() { Id = idOfParent } };

                // service is an authorized Drive API service instance
                File x = await service.Files.Insert(body).ExecuteAsync();
            }

        }

        //##############################################################################
        // Download

        private int retries = 0;
        public async Task DownloadFileAsync(DriveService service, File f, string path)
        {
            var downloader = new MediaDownloader(service);
            System.IO.FileStream fileStream = null;
            downloader.ChunkSize = DownloadChunkSize;

            try
            {
                fileStream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                var progress = await downloader.DownloadAsync(f.SelfLink, fileStream);

                // updating the modified date
                await service.Files.Touch(f.Id).ExecuteAsync();

                if (progress.Status == DownloadStatus.Completed)
                {
                    Console.WriteLine(path + " was downloaded successfully");
                }
                else
                {
                    Console.WriteLine(Environment.NewLine + "Download {0} was interpreted in the middle. Only {1} were downloaded. ",
                        path, progress.BytesDownloaded);
                }
            }
            catch (GoogleApiException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.Unauthorized)
                {
                    //GoogleWebAuthorizationBroker.Folder = "Drive.Sample";

                    //credential.RefreshTokenAsync(CancellationToken.None).Wait();

                    /*using (var stream = new System.IO.FileStream("client_secrets.json",
                        System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        GoogleWebAuthorizationBroker.ReauthorizeAsync(credential, CancellationToken.None).Wait();

                    }

                    // Create the service.
                    service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "CloudManagerGD",
                    });*/
                    /*if(retries <= 4)
                    {
                        Thread.Sleep(retries * 1000);
                        this.DownloadFileAsync(service, f, path).Wait();
                        retries++;
                    }
                    else
                    {
                        retries = 0;
                        return;
                    }*/
                    
                }
            }
            catch (System.IO.IOException e)
            {
                if (retries <= 4)
                {
                    Thread.Sleep(retries * 1000);
                    this.DownloadFileAsync(service, f, path).Wait();
                    retries++;
                }
                else
                {
                    retries = 0;
                    return;
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
        }

        private String getPathOfSingleFile(IList<File> fl, File child, string data = null)
        {
            string temp;

            if (child.Parents.Count < 1)
            {
                return null;
            }

            if (String.IsNullOrEmpty(data))
            {
                temp = @"\" + child.Title;
            }
            else
            {
                temp = @"\" + data;
            }


            try
            {
                foreach (File f in fl)
                {
                    if (f.Id.Equals(child.Parents[0].Id))
                    {
                        temp = f.Title + temp;
                        temp = getPathOfSingleFile(fl, f, temp);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\t" + child.Title + "\t");
            }

            return temp;
        }

        //##############################################################################
        // Misc

        public string[] ListToArray(List<FileInformation> fileInfo)
        {
            string[] a = new string[fileInfo.Count];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = fileInfo[i].Path;
            }

            return a;
        }

        private async Task<String> getIdFromTitle(DriveService service, IList<File> fl, String Title)
        {
            About about = await service.About.Get().ExecuteAsync();
            String RootID = about.RootFolderId;

            foreach (File f in fl)
            {
                if (f.Title.Equals(Title))
                {
                    return f.Id;
                }
            }
            return RootID;
        }

        public async Task<List<File>> RetrieveAllFilesAsList(DriveService service, string query = null)
        {
            List<File> result = new List<File>();
            FilesResource.ListRequest request = service.Files.List();
            request.MaxResults = 1000;

            if (query != null)
            {
                request.Q = query + " and trashed = false and hidden = false";
            }
            else
            {
                request.Q = "trashed = false and hidden=false";
            }

            do
            {
                try
                {
                    FileList files = await request.ExecuteAsync();

                    result.AddRange(files.Items);
                    request.PageToken = files.NextPageToken;
                    //files.Items.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred (from RetrieveAllFilesAsList): " + e.Message);
                    request.PageToken = null;
                }
            }
            while (!String.IsNullOrEmpty(request.PageToken));

            return result;

        }

        private async Task<bool> FolderExists(DriveService service, String title, String parentId)
        {

            try
            {
                //var request = service.Files.List();
                String query = "mimeType='application/vnd.google-apps.folder' AND trashed=false AND title='" + title + "' AND '" + parentId + "' in parents";
                //request.Q = query;
                //var files = await request.ExecuteAsync();
                var files = await this.RetrieveAllFilesAsList(service, query);


                if (files.Count == 0 || files == null) //if the size is zero, then the folder doesn't exist
                {
                    return false;
                }
                else
                {
                    //since google drive allows to have multiple folders with the same title (name)
                    //we select the first file in the list to return
                    return true;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("ya gat no swag!");
                return false;
            }
        }

        private async Task<bool> FileExists(DriveService service, String title, String parentId)
        {

            try
            {
                //var request = service.Files.List();
                String query = "trashed=false AND title='" + title + "' AND '" + parentId + "' in parents";
                //request.Q = query;
                //var files = await request.ExecuteAsync();
                var files = await this.RetrieveAllFilesAsList(service, query);


                if (files.Count == 0 || files == null) //if the size is zero, then the folder doesn't exist
                {
                    return false;
                }
                else
                {
                    //since google drive allows to have multiple folders with the same title (name)
                    //we select the first file in the list to return
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    
        private async Task RenameFile(DriveService service, String newTitle, String fileId)
        {
            File file = new File();
            file.Title = newTitle;
            // Rename the file.
            FilesResource.PatchRequest request = service.Files.Patch(file, fileId);
            File updatedFile = await request.ExecuteAsync();
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudManagerUI.Interfaces;
using DropNet;
using DropNet.Exceptions;
using DropNet.Models;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace CloudManagerUI.Dropbox
{
    class DropboxInterfaceFunctions : ISynchronizationContract
    {
        public async Task<bool> UploadSynchronizationAsync(List<FileInformation> fileInfo, object DriveServiceClient)
        {
            try
            {
                FileInformation[] fileArr = fileInfo.ToArray();
                DropNetClient client = DriveServiceClient as DropNetClient;


                foreach (var item in fileArr)
                {
                    if (!item.isFile) //File/true....Folder/false
                    {
                        switch (item.Mode)
                        {
                            case "cr":
                                this.UploadFolderCreate(item.FileName, item.CloudPath, client);  //create folder
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
                                //DIE FRAGE IST WAS STEHT IN CLOUDPATH????? WENN ROOTPATH DAS FILE IST STEHT "/" DARIN ODER NULL? WENN NULL MÜSSTE SONST IF OB NULL USW....CARL FRAGEN!!
                                await this.FileUploadDB(item.CloudPath, item.FileName, item.Path, null, client);
                            }


                            break;
                        case "cr": //database entries
                            if (item.isFile)
                            {
                                await this.FileUploadDB(item.CloudPath, item.FileName, item.Path, null, client);
                            }

                            break;
                        case "de": //database entries
                            await client.DeleteTask(item.Path);
                            break;
                        case "re": //database entries
                            if (item.isFile)
                            {
                                await this.FileUploadDB(item.CloudPath, item.FileName, item.Path, null, client);
                            }

                            break;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;

        }


        public async Task<bool> DownloadSynchronizationAsync(List<FileInformation> fileInfo, object DriveServiceClient)
        {
            try
            {
                FileInformation[] fileArr = fileInfo.ToArray();
                DropNetClient client = DriveServiceClient as DropNetClient;

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
                                await FileDownloadDB(item.CloudPath + item.FileName, item.Path + item.FileName, client);
                            }
                            break;

                        case "cr": //database entries
                            if (item.isFile)
                            {
                                await FileDownloadDB(item.CloudPath + item.FileName, item.Path + item.FileName, client);
                            }
                            break;

                        case "de": //database entries
                            if (item.isFile)
                            {
                                File.Delete(item.Path + item.FileName);
                            }
                            break;

                        case "re": //database entries
                            if (item.isFile)
                            {
                                await FileDownloadDB(item.CloudPath + item.FileName, item.Path + item.FileName, client);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //return TaskEx.FromResult(false);
            return false;
        }

        public async Task<long[]> DetermineAvailableCloudSpace(object DriveServiceClient)
        {
            long[] space = new long[2];
            DropNetClient client = DriveServiceClient as DropNetClient;

            try
            {
                //Das ganze ist aber erst möglich nach der fix fertigen anmeldung!!!
                var info = client.AccountInfo();

                long totalSpace = info.quota_info.quota;   //Gesamtverfügbare Speicher in byte
                long neededSpace = info.quota_info.normal; //belegter Speichermomentan in byte

                //string str2 = info.quota_info.shared.ToString(); //freigegebener SPeicher(geteilt mit anderen usern) in byte

                long availableSpace = totalSpace - neededSpace;

                space[0] = totalSpace;   //totalspace
                space[1] = availableSpace;    //availablespace
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return space;
        }

        public void UploadFolderCreate(string folder, string path, DropNetClient client)
        {
            //client = new DropNetClient(apiKey, appSecret, GetUserLoginDB(tokendb), GetUserLoginDB(secretdb));
            try
            {   //Create a Folder
                if (path == null)
                {
                    var metaData = client.GetMetaData("/",null,false);
                    if (!(metaData.Contents.Any(c => c.Is_Dir && c.Path == "/" + folder)))
                    {
                        client.CreateFolder("/" + folder);
                    }
                }
                else
                {
                    var metaData = client.GetMetaData(path,null,false);
                    if (!(metaData.Contents.Any(c => c.Is_Dir && c.Path == folder)))
                    {
                        client.CreateFolder(path + "/" + folder);
                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public async Task FileUploadDB(string dropboxpath, string filename, string filepath, FileStream fs, DropNetClient client)
        {
            //client = new DropNetClient(apiKey, appSecret, GetUserLoginDB(tokendb), GetUserLoginDB(secretdb));

            //******************NORMALASYCNMETHODE
            /*client.UploadFileAsync(dropboxpath, filename, File.ReadAllBytes(filepath),
                (response) =>
                {
                    if(response != null)
                    {
                        string t = response.Bytes.ToString();

                        byte[] bytes = new byte[t.Length * sizeof(char)];
                        System.Buffer.BlockCopy(t.ToCharArray(), 0, bytes, 0, bytes.Length);
                    }         
                },
                (error) =>
                {
                    MessageBox.Show("error uploading");
                });*/
            //***************TASKMETHODE

            try
            {
                byte[] DataArray = FileToByteArray(filepath);

                //using (fs = new FileStream(filepath, FileMode.Open))
                //{
                //    await client.UploadFileTask(dropboxpath, filename, fs);   //Methode mit FileStream funktioniert nicht :/
                //}

                await client.UploadFileTask(dropboxpath, filename, DataArray);  //Methode mit byte array funktioniert gut
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public byte[] FileToByteArray(string filepath)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(filepath,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(filepath).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }
        public async Task FileDownloadDB(string dropboxpath, string Zielpath, DropNetClient client)
        {
            //client = new DropNetClient(apiKey, appSecret, GetUserLoginDB(tokendb), GetUserLoginDB(secretdb));

            //NORMALEASYNCDOWNLOAD
            /*
            try
            {
                client.GetFileAsync(dropboxpath,
                    (response) =>
                    {
                        MessageBox.Show("DO");

                        using (FileStream fs = new FileStream(Zielpath, FileMode.Create))
                        {
                            for (int i = 0; i < response.RawBytes.Length; i++)
                            {
                                fs.WriteByte(response.RawBytes[i]);
                            }
                        }
                    },
                    (error) =>
                    {
                        MessageBox.Show("error downloading");
                    });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/

            //TASK DOWNLOAD

            try
            {
                using (FileStream fs = new FileStream(Zielpath, FileMode.Create))
                {
                    RestSharp.IRestResponse ir = await client.GetFileTask(dropboxpath) as RestSharp.IRestResponse;

                    for (int i = 0; i < ir.RawBytes.Length; i++)
                    {
                        fs.WriteByte(ir.RawBytes[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}

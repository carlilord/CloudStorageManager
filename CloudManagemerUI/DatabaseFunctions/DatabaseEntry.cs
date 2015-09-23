using CloudManagerUI.Database;
using CloudManagerUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CloudManagerUI
{
    class DatabaseEntry : IDatabaseDeputyEntries
    {
        public void execEntry(string fileID, string cloudID, string fileName, string filePath, DateTime lastUpdated, bool isDeleted, bool isFolder, bool isInRoot, string cloudPath)
        {
            DatabaseEntities _database = null;
            

            try
            {
                _database = new DatabaseEntities();

                _database.Database.Connection.Open();

                CloudManager entry = new CloudManager();
                var data = _database.CloudManagers;

                foreach(CloudManager cm in data)
                {
                    if(cm.File_Path.Equals(filePath))
                    {
                        _database.CloudManagers.Remove(cm);
                    }
                }

                
                entry.File_ID = fileID;
                entry.Cloud_ID = cloudID;
                entry.File_Name = fileName;
                entry.File_Path = filePath;
                entry.File_LastChanged = lastUpdated;
                entry.File_IsDeleted = isDeleted;
                entry.IsFolder = isFolder;
                entry.File_IsInRoot = isInRoot;
                entry.Cloud_Path = cloudPath;

                _database.CloudManagers.Add(entry);

                _database.SaveChanges();
            }
            catch (Exception)
            {
                _database.Database.Connection.Close();
            }
            finally
            {
                _database.Database.Connection.Close();
            }

            
        }

        public void execEntry(string fileName, string filePath, bool isFolder,string cloudname)
        {
            DatabaseEntities _database = null;


            try
            {
                _database = new DatabaseEntities();

                _database.Database.Connection.Open();

                CloudManager entry = new CloudManager();
                var data = _database.CloudManagers;

                foreach (CloudManager cm in data)
                {
                    if (cm.File_Path.Equals(filePath))
                    {
                        _database.CloudManagers.Remove(cm);
                    }
                }


                entry.File_ID = String.Empty;
                entry.Cloud_ID = cloudname;
                entry.File_Name = fileName;
                entry.File_Path = filePath;
                entry.File_LastChanged = null;
                entry.IsFolder = isFolder;
                entry.Cloud_Path = String.Empty;

                _database.CloudManagers.Add(entry);

                _database.SaveChanges();
            }
            catch (Exception)
            {
                _database.Database.Connection.Close();
            }
            finally
            {
                _database.Database.Connection.Close();
            }


        }
    }
}

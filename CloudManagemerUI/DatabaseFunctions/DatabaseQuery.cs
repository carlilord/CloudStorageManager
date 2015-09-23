using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudManagerUI.Database;
using CloudManagerUI.Interfaces;

namespace CloudManagerUI
{
    class DatabaseQuery : IDatabaseDeputyQueries
    {

        public bool exists(string fileID)
        {
            DatabaseEntities _database = null;

            try
            {
                _database = new DatabaseEntities();

                _database.Database.Connection.Open();

                var file = _database.CloudManagers;

                foreach (CloudManager item in file)
                {
                    if (item.File_ID.Equals(fileID))        //path ( da jetzt primary key? )
                    {
                        return true;
                    }
                }

                
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                _database.Database.Connection.Close();
            }

            return false;

            
        }
    }
}

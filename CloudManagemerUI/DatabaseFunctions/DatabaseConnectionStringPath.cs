using CloudManagerUI.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudManagerUI
{
    class DatabaseConnectionStringPath
    {
        public static void RefreshDatabasePathSettings()
        {
            string filePath = "attachdbfilename=" + Directory.GetCurrentDirectory().ToString() + @"Bin\Debug\Database\Database.mdf";


            //SqlConnection cn = new SqlConnection(global::DBTestCM.Properties.Settings.Default.DatabaseConnectionString);
            try
            {

                DatabaseEntities _database = new DatabaseEntities();


                string s = ConfigurationManager.ConnectionStrings["DatabaseEntities"].ConnectionString;
                if (!s.Contains(filePath))
                {
                    int index = -1;
                    if (s.Contains("attachdbfilename"))
                    {
                        index = s.IndexOf("attachdbfilename");
                    }
                    else if (!s.Contains("attachdbfilename"))
                    {
                        string path = Environment.CurrentDirectory;

                        string[] splits = path.Split('\\');
                        string totalResult = "";
                        for (int i = 0; i < splits.Length - 2; i++)
                        {
                            totalResult += splits[i] + "\\";
                        }
                        totalResult += "Database.mdf";

                        index = s.ToLower().IndexOf(totalResult.ToLower());
                    }

                    string _result = s.Replace(s.Substring(index, s.IndexOf(";", index) - index), filePath);

                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.ConnectionStrings.ConnectionStrings["DatabaseEntities"].ConnectionString = _result;
                    config.Save(ConfigurationSaveMode.Modified, true);
                    ConfigurationManager.RefreshSection("connectionStrings");

                    Console.WriteLine(_result);
                }
            }
            catch (Exception) { }


        }
    }
            
}

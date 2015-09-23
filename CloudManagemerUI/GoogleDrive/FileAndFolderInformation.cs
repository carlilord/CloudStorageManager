using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudManagerUI.GoogleDrive
{
    class FileAndFolderInformation
    {
        public Boolean IsFile { get; private set; }
        public String Title { get; private set; }
        public String ParentFolder { get; private set; }
        public String[] RelPathToData { get; private set; }

        public FileAndFolderInformation(Boolean IsFile, String Title, String ParentFolder)
        {
            this.IsFile = IsFile;
            this.Title = Title;
            this.ParentFolder = ParentFolder;
        }

        public FileAndFolderInformation(string file, string MainPath)
        {

            string mainFolderName = MainPath.Split('\\')[MainPath.Split('\\').Length - 1];

            string[] temp = file.Split('\\');


            if (file.EndsWith("FOLDER"))
            {
                this.IsFile = false;
                this.Title = temp[temp.Length - 1].Substring(0, temp[temp.Length - 1].LastIndexOf("FOLDER"));
            }
            else
            {
                this.IsFile = true;
                this.Title = temp[temp.Length - 1];
            }



            if (temp[temp.Length - 2].Equals(mainFolderName))
            {
                this.ParentFolder = null;
            }
            else
            {
                this.ParentFolder = temp[temp.Length - 2];
            }

            if (file.Contains("FOLDER"))
            {
                file = file.Substring(0, file.LastIndexOf("FOLDER"));
            }
            this.RelPathToData = file.Remove(0, MainPath.Length).Split('\\');
        }

        public override string ToString()
        {
            return "\tTitle: " + this.Title + "t\tParentFolder: " + this.ParentFolder + "\tIsFile: " + this.IsFile;
        }
    }
}

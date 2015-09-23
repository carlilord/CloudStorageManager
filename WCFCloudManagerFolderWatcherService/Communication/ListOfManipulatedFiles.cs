using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFCloudManagerFolderWatcherService.Communication
{
    class ListOfManipulatedFiles
    {
        // Private Constructor
        private ListOfManipulatedFiles() { }

        // Private object instantiated with private constructor
        private static readonly ListOfManipulatedFiles instance = new ListOfManipulatedFiles();

        // Public static property to get the object
        public static ListOfManipulatedFiles UniqueInstance
        {
            get { return instance; }
        }

        private List<string> files = new List<string>();

        public string[] GetFilesAsStringArr()
        {
            return this.files.ToArray();
        }

        public void AddFile(string file)
        {
            this.files.Add(file);
        }

        public void ResetList()
        {
            this.files.Clear();
        }
    }
}

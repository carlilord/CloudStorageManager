using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudManagerUI.Interfaces
{
    interface IDatabaseDeputyQueries
    {
        bool exists(string fileID);

    }

    interface IDatabaseDeputyEntries
    {
        void execEntry(string fileID, string cloudID, string fileName, string filePath, DateTime lastUpdated, bool isDeleted, bool isFolder, bool isInRoot, string cloudPath);

    }
}

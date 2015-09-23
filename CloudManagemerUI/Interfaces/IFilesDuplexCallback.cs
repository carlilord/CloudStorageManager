using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFCloudManagerFolderWatcherService.Interfaces
{
    interface IFilesDuplexCallback
    {
        [OperationContract(IsOneWay = true)]
        void Equals(string[] result);
    }
}

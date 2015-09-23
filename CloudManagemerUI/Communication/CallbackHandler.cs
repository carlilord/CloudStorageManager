using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCloudManagerFolderWatcherService.Interfaces;

namespace CloudManagerUI.Communication
{
    class CallbackHandler : IFilesDuplexCallback
    {
        public event Action<string[]> ReceivedList = delegate { };//hallo

        public void Equals(string[] result)
        {
            this.ReceivedList(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCloudManagerFolderWatcherService.Interfaces;

namespace CloudManagerUI.Communication
{
    class FilesDuplexClient : DuplexClientBase<IFilesDuplex>, IFilesDuplex
    {
        public FilesDuplexClient(InstanceContext callbackCntx)
            : base(callbackCntx)
        {            
        }

        public void Update()
        {
            base.Channel.Update();
        }
    }
}

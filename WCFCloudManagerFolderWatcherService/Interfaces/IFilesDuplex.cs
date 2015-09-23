using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFCloudManagerFolderWatcherService.Interfaces
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples", SessionMode = SessionMode.Required,
                CallbackContract = typeof(IFilesDuplexCallback))]
    public interface IFilesDuplex
    {
        [OperationContract(IsOneWay = true)]
        void Update();
    }
}

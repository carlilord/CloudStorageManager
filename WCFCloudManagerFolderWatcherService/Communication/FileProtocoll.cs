using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCloudManagerFolderWatcherService.Interfaces;

namespace WCFCloudManagerFolderWatcherService.Communication
{
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FileProtocoll : IFilesDuplex
    {
        IFilesDuplexCallback Callback
        { get { return OperationContext.Current.GetCallbackChannel<IFilesDuplexCallback>(); } }

        void IFilesDuplex.Update()
        {
            ListOfManipulatedFiles lomf = ListOfManipulatedFiles.UniqueInstance;
            String[] array1 = lomf.GetFilesAsStringArr();
            String[] array2 = FileSaverWorker.ReadFile();
            String[] newArray;

            if (array2 != null || array2.Length > 0)
            {
                newArray = new String[array1.Length + array2.Length];
                Array.Copy(array1, newArray, array1.Length);
                Array.Copy(array2, 0, newArray, array1.Length, array2.Length);
            }

            else
            {
                newArray = array1;
            }


            Callback.Equals(newArray);
            lomf.ResetList();
        }

    }
    

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using WCFCloudManagerFolderWatcherService.Interfaces;

namespace WCFCloudManagerFolderWatcherService.Communication
{
    class WCFHost
    {
        public static ServiceHost selfHost = null;
        public static void DoWork()
        {
            ListOfManipulatedFiles lomf = ListOfManipulatedFiles.UniqueInstance;
            try
            {
                string[] temp = FileSaverWorker.ReadFile();

            
                if(temp != null)
                {
                    foreach (string s in temp)
                    {
                        lomf.AddFile(s);
                    }
                }

                // Step 1 Create a URI to serve as the base address.
                Uri baseAddress = new Uri("http://localhost:8899/CloudManager/CommunicationChannel1");

                // Step 2 Create a ServiceHost instance
                if (selfHost != null)
                {
                    selfHost.Close();
                }

                selfHost = new ServiceHost(typeof(FileProtocoll), baseAddress);

                selfHost.Open();
            }
            catch (CommunicationException)
            {
                selfHost.Abort();
            }
            catch(Exception e)
            {
                System.IO.File.AppendAllText(FileSaverWorker.logFilePath, "Error: " + e.Message + Environment.NewLine);
            }
            
        }

        public static void CloseHost()
        {
            try
            {
                if (selfHost != null)
                {
                    if (selfHost.State != CommunicationState.Closed)
                    {                 
                            selfHost.Close();
                    }
                
                    selfHost = null;
                }
                FileSaverWorker.WriteFile();
            }
            catch(Exception e)
            {
                System.IO.File.AppendAllText(FileSaverWorker.logFilePath, "Error: " + e.Message + Environment.NewLine);
            }
        }
    }

    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CloudManagerUI.Communication
{
    class WCFConnectionWorker
    {
        private CallbackHandler ch = null;
        private ServiceController sc = null;
        InstanceContext instanceContext = null;
        FilesDuplexClient client = null;

        public WCFConnectionWorker(Action<string[]> result)
        {
            ch = new CallbackHandler();
            sc = new ServiceController("CloudManagerWCFConnectionService");
            ch.ReceivedList += result;

            instanceContext = new InstanceContext(ch);

            client = new FilesDuplexClient(instanceContext);
        }

        /// <summary>
        /// Starts the WCF Service
        /// </summary>
        /// <param name="path">To set a new location for the WatchFolderService</param>
        public void StartService(string path = null)
        {

            try
            {
                if (sc.Status == ServiceControllerStatus.StartPending || sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                }

                sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 10));

                if (String.IsNullOrEmpty(path))
                {
                    sc.Start();
                }
                else
                {
                    sc.Start(new String[] { path });
                }
                sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 0, 10));
            }
            catch (Exception e)
            {
                string temp = e.Message;
            }

            
            
        }

        public void StopService()
        {
            if (sc.Status != ServiceControllerStatus.Stopped || sc.Status != ServiceControllerStatus.StopPending)
            {
                sc.Stop();
            }

            while (sc.Status != ServiceControllerStatus.Stopped);
        }

        public void RequestFileList()
        {
            try
            {
                client.Update();
            }
            catch(EndpointNotFoundException e)
            {
                this.StartService();
                client.Update();
            }
            
        }
    }
}

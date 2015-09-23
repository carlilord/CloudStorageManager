using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudManagerUI
{
    public class RefreshTokenHandler : IRefreshTokenHandler
    {
        

        public RefreshTokenHandler()
        {
            
        }

        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\oneDrive\\RefreshTokenHandler\\";
        public Task<RefreshTokenInfo> RetrieveRefreshTokenAsync()
        {
            return Task.Factory.StartNew<RefreshTokenInfo>(() =>
            {
                if (File.Exists(path + "OneDrive.cloudmanager"))
                {
                    return new RefreshTokenInfo(File.ReadAllText(path+"OneDrive.cloudmanager"));
                }
                return null;
            });
        }

        public Task SaveRefreshTokenAsync(RefreshTokenInfo tokenInfo)
        {
            // Note: 
            // 1) In order to receive refresh token, wl.offline_access scope is needed.
            // 2) Alternatively, we can persist the refresh token.
            return Task.Factory.StartNew(() =>
            {
                if (File.Exists(path+"OneDrive.cloudmanager")) File.Delete(path+"OneDrive.cloudmanager");
                if (!Directory.Exists(Path.GetDirectoryName(path+"OneDrive.cloudmanager"))) Directory.CreateDirectory(Path.GetDirectoryName(path+"OneDrive.cloudmanager"));
                File.AppendAllText(path+"OneDrive.cloudmanager", tokenInfo.RefreshToken);
            });
        }
        public void DeleteToken()
        {
            if (File.Exists(path + "OneDrive.cloudmanager"))
            {
                File.Delete(path + "OneDrive.cloudmanager");
            }
        }
    }
}

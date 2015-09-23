using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace CloudManagerUI.GoogleDrive
{
    class GoogleDriveClass
    {
        private static readonly string[] Scopes = new[] { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
        public event Action loginStatusChanged = delegate { };

        private DriveService service = null;
        private UserCredential credential = null;
        private string dir = null;


        public bool SignedIn = false;

        public DriveService Service
        {
            get
            {
                if (service == null)
                {
                    service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "CloudManagerGD1",
                    });
                }

                return service;
            }
            set
            {
                service = value;
            }
        }

        public GoogleDriveClass()
        {
            GoogleWebAuthorizationBroker.Folder = "CloudManager.GoogleDrive.OAuth2";

            this.dir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GoogleWebAuthorizationBroker.Folder);

            /*this.InitializeSignIn().Wait(new CancellationTokenSource(14000).Token);

            if (service == null)
            {
                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "CloudManagerGD1",
                });
            }*/
        }

        public async Task<string> ReceiveAccountEmail()
        {
            if (!SignedIn)
                return null;

            return service.About.Get().Execute().User.EmailAddress;
        }

        public async Task<string> ReceiveAccountName()
        {
            if (!SignedIn)
                return null;

            return service.About.Get().Execute().User.DisplayName;
        }

        public async Task InitializeSignIn(string UID = "CloudManager2015")
        {
            using (var stream = new System.IO.FileStream("client_secrets.json",
                System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, Scopes, UID, CancellationToken.None);
            }

            if (service == null)
            {
                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "CloudManagerGD1",
                });
            
            }

            SignedIn = true;

            this.loginStatusChanged();
        }

        public async Task InitializeSignOut()
        {
            await credential.RevokeTokenAsync(CancellationToken.None);

            service.Dispose();

            service = null;
            credential = null;

            SignedIn = false;

            this.loginStatusChanged();
        }

        public async Task<bool> AutoLogin()
        {
            try
            {
                if (credential == null)
                {
                    using (var stream = new System.IO.FileStream("client_secrets.json",
                        System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        if (System.IO.Directory.GetFiles(this.dir).Length > 0)
                        {
                            this.credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                                GoogleClientSecrets.Load(stream).Secrets, Scopes, "CloudManager2015", CancellationToken.None);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                if (!(await credential.RefreshTokenAsync(CancellationToken.None)))
                {
                    return false;
                }
                //await credential.RefreshTokenAsync(CancellationToken.None);

                if (service == null)
                {
                    service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "CloudManagerGD1",
                    });
                }
            }
            catch(Exception)
            {
                SignedIn = false;

                this.loginStatusChanged();

                return false;
            }

            SignedIn = true;

            this.loginStatusChanged();

            return true;
        }
    }
}

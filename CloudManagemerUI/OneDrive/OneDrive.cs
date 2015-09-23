using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CloudManagerUI
{
    class OneDriveClass
    {
        public delegate void LoginFinishedDelegate(AuthResult result);
        public event LoginFinishedDelegate loginFinished;

        public delegate void LogoutFinishedDelegate();
        public event LogoutFinishedDelegate logoutFinished;


        private readonly string ClientID = "000000004012B29D";
        public AuthWindow authForm;
        public LiveAuthClient liveAuthClient;
        public LiveConnectClient liveConnectClient;
        public RefreshTokenHandler handler;

        public void clearClients()
        {
            this.LiveAuthClient = null;
            this.liveConnectClient = null;
            this.handler = null;
        }

        public LiveAuthClient LiveAuthClient
        {
            get
            {
                if (this.liveAuthClient == null)
                {

                    this.LiveAuthClient = new LiveAuthClient(ClientID);



                }

                return this.liveAuthClient;
            }

            set
            {


                this.liveAuthClient = value;


                this.liveConnectClient = null;
            }
        }

        private string[] GetAuthScopes()
        {
            string[] scopes = new string[10];
            scopes[0] = "wl.offline_access";
            scopes[1] = "wl.skydrive";
            scopes[2] = "wl.signin";
            scopes[3] = "wl.skydrive_update";
            scopes[4] = "wl.photos";
            scopes[5] = "wl.emails";
            return scopes;

        }

        public async Task<string> ReceiveAccountEmail(string accesstoken)
        {
            string data;
            string url = "https://apis.live.net/v5.0/me?access_token=" + accesstoken;
            using (WebClient wc = new WebClient())
            {
                var response = wc.DownloadData(url);

                string json = System.Text.Encoding.UTF8.GetString(response);

                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

                dynamic jsonData = serializer.Deserialize(json, typeof(object));

                data = jsonData.emails.account;

            }
            return data;
        }
        public async Task<string> ReceiveAccountName(string accesstoken)
        {
            string data;
            string url = "https://apis.live.net/v5.0/me?access_token=" + accesstoken;
            using (WebClient wc = new WebClient())
            {
                var response = wc.DownloadData(url);

                string json = System.Text.Encoding.UTF8.GetString(response);

                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

                dynamic jsonData = serializer.Deserialize(json, typeof(object));

                data = jsonData.name;

            }
            return data;
        }
        private async void OnAuthCompleted(AuthResult result)
        {
            if (this.loginFinished != null)
            {
                this.loginFinished(result);
            }


        }

        public async Task InitializeSignIn()
        {
            if (CloudManagerUI.AuthWindow.IsActiveProperty != null)
            {
                string startUrl = this.LiveAuthClient.GetLoginUrl(this.GetAuthScopes());
                string endUrl = "https://login.live.com/oauth20_desktop.srf";
                this.authForm = new AuthWindow(
                    startUrl,
                    endUrl,
                    this.OnAuthCompleted);
                this.authForm.Closed += authForm_Closed;
                this.authForm.ShowDialog();


            }


        }

        public async Task InitializeSignOut()
        {
            string endUrl = this.LiveAuthClient.GetLogoutUrl(); ;
            string startUrl = "https://login.live.com/oauth20_logout.srf?client_id=000000004012B29D&redirect_uri=http://www.htl-braunau.at/";
            this.authForm = new AuthWindow(
                startUrl,
                endUrl,
                this.OnLogoutCompleted);
            this.handler.DeleteToken();
            this.authForm.Closed += authForm_Closed;
            this.authForm.ShowDialog();

            if (this.logoutFinished != null)
            {
                this.logoutFinished();
            }
        }

        public async Task<bool> AutoLogin()
        {
            try
            {
                LiveConnectSession Session;
                this.handler = new RefreshTokenHandler();
                this.LiveAuthClient = new LiveAuthClient(this.ClientID, this.handler);
                LiveConnectSession s = this.LiveAuthClient.InitializeAsync().Result.Session;

                if (this.handler != null)
                {

                    Session = s;
                    this.liveConnectClient = new LiveConnectClient(Session);
                    return true;
                }
                else
                {
                    Session = null;
                    return false;
                }
            }
            catch (Exception) { }


            return false;


        }

        void authForm_Closed(object sender, EventArgs e)
        {
            this.authForm.Webbrowser.Dispose();
            this.authForm.Close();
        }

        private void OnLogoutCompleted()
        {
            this.authForm.Close();
        }
    }
}

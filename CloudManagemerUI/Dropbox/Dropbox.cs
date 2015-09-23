using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DropNet;
using DropNet.Models;
using System.Windows.Threading;
using System.Threading;

namespace CloudManagerUI.Dropbox
{
    class DropboxClass
    {
        public delegate void LoginFinishedDelegate();
        public event LoginFinishedDelegate loginFinished;

        public UserLogin _userLogin;
        public DropNetClient client = null;
        public AuthWindowDB authForm;

        public bool SignedIn = false;

        private string apiKey = "lmnqa15cmfqqw0c";
        private string appSecret = "tz51cw8rueublrl";

        private string tokendb = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData), "CloudManagerDB\\UserLoginTokenDB.txt");
        private string secretdb = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData), "CloudManagerDB\\UserLoginSecretDB.txt");

        private string appdatapath = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData), "CloudManagerDB");   //zb:"C:\\Users\\Michael\\AppData\\Roaming\\CloudManagerDB

        private async void OnAuthCompleted()
        {
            Thread t = null;
            client.GetAccessTokenAsync((userLogin) =>
            {
                //Save the Token and Secret we get here to save future logins
                _userLogin = userLogin;

                //_client = new DropNetClient(apiKey, appSecret, _userLogin.Token, _userLogin.Secret, null);
                t = Thread.CurrentThread;
                this.SaveSecrets();
                //SignedIn = true;
            },
                     (error) =>
                     {
                         /////////////////////
                     });

            Thread.Sleep(5000);     //Thread dauaert zu lange daher noch stoppen.
            //while (t.ThreadState == ThreadState.Running);
            
            
            if(this.loginFinished != null)
            {
                this.loginFinished();
            }
            
        }

        public async Task<string> ReceiveAccountEmail()
        {
            if (!SignedIn)
                return null;
            else
            {
                var info = client.AccountInfo();

                string emailname = info.email;

                return emailname;
            }
        }

        public async Task<string> ReceiveAccountName()
        {
            if (!SignedIn)
                return null;
            else
            {
                var info = client.AccountInfo();

                string accountname = info.display_name;

                return accountname;
            }
        }

        public async Task InitializeSignIn()
        {
            client = new DropNetClient(apiKey, appSecret);

            client.GetTokenAsync(UserLoginParam,
                    (error) =>
                    {

                    });
        }

        void UserLoginParam(UserLogin userLogin)
        {
            //Dont need to do anything here with userLogin if we keep the same instance of DropNetClient

            //Now generate the url for the user to authorize the app

            App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,new Action(DoIt));
            
   
        }

        void DoIt()
        {
            try
            {
                var tokenUrl = client.BuildAuthorizeUrl(); //Use your own callback Url here

                if (CloudManagerUI.AuthWindowDB.IsActiveProperty != null)
                {
                    string startUrl = tokenUrl.ToString();
                    string endUrl = "authorize_submit";
                    this.authForm = new AuthWindowDB(
                        startUrl,
                        endUrl,
                        OnAuthCompleted);
                    this.authForm.Closed += authForm_Closed;
                    this.authForm.ShowDialog();
                }
            }
            catch (Exception e)
            {
                string temp = e.Message;
            }
        }
        void authForm_Closed(object sender, EventArgs e)
        {
            this.authForm.Webbrowser.Dispose();
            this.authForm.Close();
        }

        public async Task InitializeSignOut()
        {
            if (File.Exists(tokendb) && File.Exists(secretdb))
            {
                File.Delete(tokendb);
                File.Delete(secretdb);

                SignedIn = false;
            }
        }

        public async Task<bool> AutoLogin()
        {
            try
            {
                if (File.Exists(tokendb) && File.Exists(secretdb))
                {
                    client = new DropNetClient(apiKey, appSecret, GetUserLoginDB(tokendb), GetUserLoginDB(secretdb));
                    if (client != null)
                    {
                        SignedIn = true;
                        return true;
                    }
                    else
                    {
                        SignedIn = false;
                        return false;
                    }
                }
                else
                {
                    SignedIn = false;
                    return false;
                }
            }
            catch (Exception)
            {
                SignedIn = false;
                return false;
            }         
        }

        private void SaveSecrets()
        {
            DirectoryInfo di = new DirectoryInfo(appdatapath);
            if (!di.Exists)
            {
                Directory.CreateDirectory(appdatapath);

                SaveAccessTokenDB(_userLogin.Token, tokendb);
                SaveAccessTokenDB(_userLogin.Secret, secretdb);
            }
            else
            {

                SaveAccessTokenDB(_userLogin.Token, tokendb);
                SaveAccessTokenDB(_userLogin.Secret, secretdb);
            }
        }

        public void SaveAccessTokenDB(string accessToken, string accesstokenpath)
        {
            using (FileStream fs = File.Create(accesstokenpath))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(accessToken);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        public string GetUserLoginDB(string accesstokenpath)
        {
            using (StreamReader sr = File.OpenText(accesstokenpath))
            {
                string token = null; ;
                string s = null;
                while ((s = sr.ReadLine()) != null)
                {
                    token += s;
                }

                return token;
            }
        }
    }
}

using CloudManagerUI.Communication;
using CloudManagerUI.Dropbox;
using CloudManagerUI.GoogleDrive;
using CloudManagerUI.Intelligente_Algorithmen;
using CloudManagerUI.Model;
using CloudManagerUI.OneDrive;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace CloudManagerUI.ViewModel
{
    class CloudManagerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private WCFConnectionWorker Wcf { get; set; }

        private System.Timers.Timer timer;
        private IntelligentUpload IntelligentUpload { get; set;}

        private ModelBindings Bindings { get; set; }        //Model

        private bool dialogIsChecked = false;

        public OneDriveClass OneDrive { get; set; }
        public OneDriveInterfaceFunctions OneDriveInterfaceFunctions { get; set; }

        public GoogleDriveClass GoogleDrive { get; set; }
        public GoogleDriveInterfaceFunctions GoogleDriveInterfaceFunctions { get; set; }

        //DropBox
        public DropboxClass Dropbox { get; set; }
        public DropboxInterfaceFunctions DropboxInterfaceFunctions { get; set; }



        public string Tick
        {
            get
            {
                return @"./Resources/Tick.png";
            }
        }
        public string Cross
        {
            get
            {
                return @"./Resources/Cross.png";
            }
        }

        public int ProgressMax
        {
            get
            {
                return this.Bindings.ProgressMax;
            }
            set
            {
                this.Bindings.ProgressMax = value;
                this.OnPropertyChanged("ProgressMax");
            }
        }
        public int ProgressMin
        {
            get
            {
                return this.Bindings.ProgressMin;
            }
            set
            {
                this.Bindings.ProgressMin = value;
                this.OnPropertyChanged("ProgressMin");
            }
        }

        public Brush ProgressBarBorderBrushOneDrive
        {
            get
            {
                return this.Bindings.ProgressBarBorderBrushOneDrive;
            }
            set
            {
                this.Bindings.ProgressBarBorderBrushOneDrive = value;
                this.OnPropertyChanged("ProgressBarBorderBrushOneDrive");
            }
        }

        public Brush ProgressBarForegroundOneDrive
        {
            get
            {
                return this.Bindings.ProgressBarForegroundOneDrive;
            }
            set
            {
                this.Bindings.ProgressBarForegroundOneDrive = value;
                this.OnPropertyChanged("ProgressBarForegroundOneDrive");
            }
        }

        public string BtnTextOneDrive
        {
            get
            {
                return this.Bindings.BtnTextOneDrive;
            }
            set
            {
                this.Bindings.BtnTextOneDrive = value;
                this.OnPropertyChanged("BtnTextOneDrive");
            }
        }
        public int ProgressValueOneDrive
        {
            get
            {
                return this.Bindings.ProgressValueOneDrive;
            }
            set
            {
                this.Bindings.ProgressValueOneDrive = value;
                this.OnPropertyChanged("ProgressValueOneDrive");
            }
        }
        public string ImageSourceOneDrive
        {
            get
            {
                return this.Bindings.ImageSourceOneDrive;
            }
            set
            {
                this.Bindings.ImageSourceOneDrive = value;
                this.OnPropertyChanged("ImageSourceOneDrive");
            }
        }
        public Brush ProgressBarBorderBrushDropbox
        {
            get
            {
                return this.Bindings.ProgressBarBorderBrushDropbox;
            }
            set
            {
                this.Bindings.ProgressBarBorderBrushDropbox = value;
                this.OnPropertyChanged("ProgressBarBorderBrushDropbox");
            }
        }

        public Brush ProgressBarForegroundDropbox
        {
            get
            {
                return this.Bindings.ProgressBarForegroundDropbox;
            }
            set
            {
                this.Bindings.ProgressBarForegroundDropbox = value;
                this.OnPropertyChanged("ProgressBarForegroundDropbox");
            }
        }

        public string BtnTextDropbox
        {
            get
            {
                return this.Bindings.BtnTextDropbox;
            }
            set
            {
                this.Bindings.BtnTextDropbox = value;
                this.OnPropertyChanged("BtnTextDropbox");
            }
        }
        public int ProgressValueDropbox
        {
            get
            {
                return this.Bindings.ProgressValueDropbox;
            }
            set
            {
                this.Bindings.ProgressValueDropbox = value;
                this.OnPropertyChanged("ProgressValueDropbox");
            }
        }
        public string ImageSourceDropbox
        {
            get
            {
                return this.Bindings.ImageSourceDropbox;
            }
            set
            {
                this.Bindings.ImageSourceDropbox = value;
                this.OnPropertyChanged("ImageSourceDropbox");
            }
        }
        public Brush ProgressBarBorderBrushGoogleDrive
        {
            get
            {
                return this.Bindings.ProgressBarBorderBrushGoogleDrive;
            }
            set
            {
                this.Bindings.ProgressBarBorderBrushGoogleDrive = value;
                this.OnPropertyChanged("ProgressBarBorderBrushGoogleDrive");
            }
        }

        public Brush ProgressBarForegroundGoogleDrive
        {
            get
            {
                return this.Bindings.ProgressBarForegroundGoogleDrive;
            }
            set
            {
                this.Bindings.ProgressBarForegroundGoogleDrive = value;
                this.OnPropertyChanged("ProgressBarForegroundGoogleDrive");
            }
        }

        public string BtnTextGoogleDrive
        {
            get
            {
                return this.Bindings.BtnTextGoogleDrive;
            }
            set
            {
                this.Bindings.BtnTextGoogleDrive = value;
                this.OnPropertyChanged("BtnTextGoogleDrive");
            }
        }
        public int ProgressValueGoogleDrive
        {
            get
            {
                return this.Bindings.ProgressValueGoogleDrive;
            }
            set
            {
                this.Bindings.ProgressValueGoogleDrive = value;
                this.OnPropertyChanged("ProgressValueGoogleDrive");
            }
        }
        public string ImageSourceGoogleDrive
        {
            get
            {
                return this.Bindings.ImageSourceGoogleDrive;
            }
            set
            {
                this.Bindings.ImageSourceGoogleDrive = value;
                this.OnPropertyChanged("ImageSourceGoogleDrive");
            }
        }
        public string OneDriveStatus
        {
            get
            {
                return this.Bindings.OneDriveStatus;
            }
            set
            {
                this.Bindings.OneDriveStatus = value;
                this.OnPropertyChanged("OneDriveStatus");
            }
        }
        public string GoogleDriveStatus
        {
            get
            {
                return this.Bindings.GoogleDriveStatus;
            }
            set
            {
                this.Bindings.GoogleDriveStatus = value;
                this.OnPropertyChanged("GoogleDriveStatus");
            }
        }
        public string DropboxStatus
        {
            get
            {
                return this.Bindings.DropboxStatus;
            }
            set
            {
                this.Bindings.DropboxStatus = value;
                this.OnPropertyChanged("DropboxStatus");
            }
        }

        public string OneDriveAccountName
        {
            get
            {
                return this.Bindings.OneDriveAccountName;
            }
            set
            {
                this.Bindings.OneDriveAccountName = value;
                this.OnPropertyChanged("OneDriveAccountName");
            }
        }
        public bool OneDriveHasAccountName
        {
            get
            {
                return this.Bindings.OneDriveHasAccountName;
            }
            set
            {
                this.Bindings.OneDriveHasAccountName = value;
                this.OnPropertyChanged("OneDriveHasAccountName");
            }
        }
        public string OneDriveEmail
        {
            get
            {
                return this.Bindings.OneDriveEmail;
            }
            set
            {
                this.Bindings.OneDriveEmail = value;
                this.OnPropertyChanged("OneDriveEmail");
            }
        }
        public string OneDriveStorage
        {
            get
            {
                return this.Bindings.OneDriveStorage;

            }
            set
            {
                this.Bindings.OneDriveStorage = value;
                this.OnPropertyChanged("OneDriveStorage");
            }
        }

        public string GoogleDriveAccountName
        {
            get
            {
                return this.Bindings.GoogleDriveAccountName;
            }
            set
            {
                this.Bindings.GoogleDriveAccountName = value;
                this.OnPropertyChanged("GoogleDriveAccountName");
            }
        }
        public bool GoogleDriveHasAccountName
        {
            get
            {
                return this.Bindings.GoogleDriveHasAccountName;
            }
            set
            {
                this.Bindings.GoogleDriveHasAccountName = value;
                this.OnPropertyChanged("GoogleDriveHasAccountName");
            }
        }
        public string GoogleDriveEmail
        {
            get
            {
                return this.Bindings.GoogleDriveEmail;
            }
            set
            {
                this.Bindings.GoogleDriveEmail = value;
                this.OnPropertyChanged("GoogleDriveEmail");
            }
        }
        public string GoogleDriveStorage
        {
            get
            {
                return this.Bindings.GoogleDriveStorage;
            }
            set
            {
                this.Bindings.GoogleDriveStorage = value;
                this.OnPropertyChanged("GoogleDriveStorage");
            }
        }
        public string DropboxAccountName
        {
            get
            {
                return this.Bindings.DropboxAccountName;
            }
            set
            {
                this.Bindings.DropboxAccountName = value;
                this.OnPropertyChanged("DropboxAccountName");
            }
        }
        public bool DropboxHasAccountName
        {
            get
            {
                return this.Bindings.DropboxHasAccountName;
            }
            set
            {
                this.Bindings.DropboxHasAccountName = value;
                this.OnPropertyChanged("DropboxHasAccountName");
            }
        }
        public string DropboxEmail
        {
            get
            {
                return this.Bindings.DropboxEmail;
            }
            set
            {
                this.Bindings.DropboxEmail = value;
                this.OnPropertyChanged("DropboxEmail");
            }
        }
        public string DropboxStorage
        {
            get
            {
                return this.Bindings.DropboxStorage;
            }
            set
            {
                this.Bindings.DropboxStorage = value;
                this.OnPropertyChanged("DropboxStorage");
            }
        }
        public bool OneDriveSignInStatus
        {
            get
            {
                return this.Bindings.OneDriveSignInStatus;
            }
            set
            {
                this.Bindings.OneDriveSignInStatus = value;
                this.OnPropertyChanged("OneDriveSignInStatus");
            }
        }
        public bool DropboxSignInStatus
        {
            get
            {
                return this.Bindings.DropboxSignInStatus;
            }
            set
            {
                this.Bindings.DropboxSignInStatus = value;
                this.OnPropertyChanged("DropboxSignInStatus");
            }
        }
        public bool GoogleDriveSignInStatus
        {
            get
            {
                return this.Bindings.GoogleDriveSignInStatus;
            }
            set
            {
                this.Bindings.GoogleDriveSignInStatus = value;
                this.OnPropertyChanged("GoogleDriveSignInStatus");
            }
        }


        public CloudManagerViewModel()
        {
            //Model
            this.Bindings = new ModelBindings();

            //OneDrive
            this.OneDriveInterfaceFunctions = new OneDriveInterfaceFunctions();
            this.OneDrive = new OneDriveClass();
            this.OneDrive.loginFinished += OneDrive_loginFinished;
            this.OneDrive.logoutFinished += OneDrive_logoutFinished;
            this.Btn_LogInOutOneDrive = new ActionCommand(this.LogInOut, this.canLogin);

            //GoogleDrive
            this.GoogleDriveInterfaceFunctions = new GoogleDriveInterfaceFunctions();
            this.GoogleDrive = new GoogleDriveClass();
            this.GoogleDrive.loginStatusChanged += GoogleDrive_loginStatusChanged;
            this.Btn_LogInOutGoogleDrive = new ActionCommand(this.GoogleDrive_LogInOut, this.GoogleDrive_CanLogin);

            //Dropbox
            this.DropboxInterfaceFunctions = new DropboxInterfaceFunctions();
            this.Dropbox = new DropboxClass();
            this.Btn_LogInOutDropbox = new ActionCommand(this.Dropbox_LogInOut, this.Dropbox_CanLogin);
            this.Dropbox_DoOnStartup();
            this.Dropbox.loginFinished += Dropbox_loginFinished;

          
            //Intelligenter Upload
            this.IntelligentUpload = new IntelligentUpload(this);

            //WCF
             this.Wcf = new WCFConnectionWorker(this.IntelligentUpload.ReceivedListLocal);
            
            //Timer
             this.timer = new System.Timers.Timer(1000 * 30);        //30 sekundos
             this.timer.Elapsed += timer_Elapsed;

             
            
        }

        

        internal void MainWindow_Loaded(object sender, EventArgs e)
        {
            //OneDrive
            this.OneDrive_DoOnStartUp();
            //GoogleDrive
            this.GoogleDrive_DoOnStartup();
            //dropbox

            //WCF Service-Start
            this.Wcf.StartService(@"C:\Users\Nico\Desktop");

            //Timer start
            this.timer.Start();
            
        }


        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Wcf.RequestFileList();
        }

        void OneDrive_logoutFinished()
        {
            if (this.OneDrive.authForm.IsActive == false)
            {
                this.OneDrive.clearClients();
                this.setBindings();
                this.Btn_LogInOutOneDrive.raiseCanExecuteChanged();

            }
        }       //miche auch

        async void OneDrive_loginFinished(AuthResult result)
        {
            this.OneDrive.authForm.Close();
            if (result.AuthorizeCode != null)
            {

                try
                {

                    LiveConnectSession session = await this.OneDrive.LiveAuthClient.ExchangeAuthCodeAsync(result.AuthorizeCode);
                    this.OneDrive.liveConnectClient = new LiveConnectClient(session);


                    this.OneDrive.handler = new RefreshTokenHandler();
                    await this.OneDrive.handler.SaveRefreshTokenAsync(new RefreshTokenInfo(session.RefreshToken));


                }
                catch (Exception) { }

            }

            this.setBindings();
            this.Btn_LogInOutOneDrive.raiseCanExecuteChanged();

        }   //miche auch

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void CheckFolderDirectory()
        {
            string buf = File.ReadAllText("MainFolder.txt");
            if (buf.Equals(String.Empty) || buf == null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Bitte wählen Sie einen Ort für den CloudManager-Ordner aus.";
                while (fbd.ShowDialog() == DialogResult.None) ;
                
                DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);

                if (di != null)
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(di.FullName, "CloudManager"));
                    File.WriteAllText("MainFolder.txt", System.IO.Path.Combine(di.FullName, "CloudManager"));
                    MainFolder.MainFolderPath = System.IO.Path.Combine(di.FullName, "CloudManager");


                }
            }
        }

        private async void setBindings()
        {
            if (this.OneDrive.liveConnectClient != null)        //if und else kopieren danach einfügen und ändern   "Dropbox" "GoogleDrive"
            {
                this.BtnTextOneDrive = "Logout";
                this.ProgressMax = 100;
                this.ProgressMin = 0;
                this.OneDriveStatus = "Logged In";
                this.OneDriveSignInStatus = true;
                string tempName = await this.OneDrive.ReceiveAccountName(this.OneDrive.liveConnectClient.Session.AccessToken);
                string tempMail = await this.OneDrive.ReceiveAccountEmail(this.OneDrive.liveConnectClient.Session.AccessToken);
                if (tempName != null)
                {
                    this.OneDriveAccountName = tempName;
                    this.OneDriveHasAccountName = true;
                }
                else
                {
                    this.OneDriveAccountName = "No account name specified";
                    this.OneDriveHasAccountName = false;

                }
                if (tempMail == null)
                {
                    this.OneDriveEmail = "Error retrieving email";
                }
                else
                {
                    this.OneDriveEmail = tempMail;
                }
                long[] storageInfo = await this.OneDriveInterfaceFunctions.DetermineAvailableCloudSpace(this.OneDrive.liveConnectClient);
                if (storageInfo != null)
                {
                    var availableGB = (double)((double)storageInfo[1] / (double)(1024 * 1024 * 1024));
                    var totalGB = (double)((double)storageInfo[0] / (double)(1024 * 1024 * 1024));
                    var usedGB = (double)((double)totalGB - (double)availableGB);

                    float available = (float)(Math.Truncate((double)availableGB * 100.0) / 100.0);
                    float total = (float)(Math.Truncate((double)totalGB * 100.0) / 100.0);
                    float used = (float)(Math.Truncate((double)usedGB * 100.0) / 100.0);

                    string temp = used + "/" + total + " GB";
                    this.OneDriveStorage = temp;

                    var storageOk = (int)((double)((availableGB / totalGB) * 100));

                    if (storageOk >= 40)
                    {
                        this.ProgressBarBorderBrushOneDrive = new SolidColorBrush(Colors.Green);
                        this.ProgressBarForegroundOneDrive = new SolidColorBrush(Colors.Green);
                    }
                    if (storageOk >= 20 && storageOk < 40)
                    {
                        this.ProgressBarBorderBrushOneDrive = new SolidColorBrush(Colors.Orange);
                        this.ProgressBarForegroundOneDrive = new SolidColorBrush(Colors.Orange);

                    }
                    if (storageOk < 20)
                    {
                        this.ProgressBarBorderBrushOneDrive = new SolidColorBrush(Colors.Red);
                        this.ProgressBarForegroundOneDrive = new SolidColorBrush(Colors.Red);

                    }

                    this.ProgressValueOneDrive = 100 - storageOk;
                }

            }
            else
            {
                this.OneDriveStatus = "Logged out";
                this.OneDriveSignInStatus = false;
                this.OneDriveAccountName = String.Empty;
                this.OneDriveHasAccountName = false;
                this.OneDriveEmail = String.Empty;
                this.OneDriveStorage = String.Empty;
                this.BtnTextOneDrive = "Login";
                this.ProgressBarBorderBrushOneDrive = new SolidColorBrush(Colors.Black);
                this.ProgressBarForegroundOneDrive = new SolidColorBrush(Colors.White);
                this.ProgressValueOneDrive = 0;
                this.ProgressMax = 100;
                this.ProgressMin = 0;

            }

        }

        private async void OneDrive_DoOnStartUp()
        {
            if (this.dialogIsChecked == false)
            {
                //CloudManagemerUI.Properties.Settings.Default.MainFolderLocation = String.Empty;
                //CloudManagemerUI.Properties.Settings.Default.Save();
                this.dialogIsChecked = true;
                this.CheckFolderDirectory();
            }

            bool isSuccessful = await this.OneDrive.AutoLogin();
            if (isSuccessful)
            {
                this.setBindings();
                this.Btn_LogInOutOneDrive.raiseCanExecuteChanged();
            }
            else
            {
                this.setBindings();
            }
        }

        public bool canLogin()
        {
            if (this.OneDrive.liveConnectClient != null)
            {
                this.ImageSourceOneDrive = this.Tick;
                this.BtnTextOneDrive = "Logout";
            }
            else
            {
                this.ImageSourceOneDrive = this.Cross;
                this.BtnTextOneDrive = "Login";
            }
            return true;

        }

        public async Task LogInOut()
        {
            if (this.OneDriveSignInStatus == true)
            {
                await this.OneDrive.InitializeSignOut();
            }
            else
            {
                await this.OneDrive.InitializeSignIn();
            }
        }

        //google drive

        void GoogleDrive_loginStatusChanged()
        {
            this.GoogleDrive_SetBindings();
            this.Btn_LogInOutGoogleDrive.raiseCanExecuteChanged();
        }

        public bool GoogleDrive_CanLogin()
        {
            if (this.GoogleDrive.SignedIn)
            {
                this.ImageSourceGoogleDrive = this.Tick;
                this.BtnTextGoogleDrive = "Logout";
            }
            else
            {
                this.ImageSourceGoogleDrive = this.Cross;
                this.BtnTextGoogleDrive = "Login";
            }
            return true;

        }

        public async Task GoogleDrive_LogInOut()
        {
            if (this.GoogleDrive.SignedIn)
            {
                await this.GoogleDrive.InitializeSignOut();
                this.GoogleDrive_SetBindings();
            }
            else
            {
                await this.GoogleDrive.InitializeSignIn();
                this.GoogleDrive_SetBindings();
            }
        }

        private async void GoogleDrive_DoOnStartup()
        {

            bool isSuccessful = await this.GoogleDrive.AutoLogin();

            if (isSuccessful)
            {
                this.GoogleDrive_SetBindings();
                this.Btn_LogInOutGoogleDrive.raiseCanExecuteChanged();
            }
            else
            {
                this.GoogleDrive_SetBindings();
            }
        }

        private async void GoogleDrive_SetBindings()
        {
            this.Btn_LogInOutGoogleDrive.raiseCanExecuteChanged();

            if (this.GoogleDrive.SignedIn)
            {
                this.BtnTextGoogleDrive = "Logout";
                this.ProgressMax = 100;
                this.ProgressMin = 0;
                this.GoogleDriveStatus = "Logged In";
                this.GoogleDriveSignInStatus = true;
                string tempName = await this.GoogleDrive.ReceiveAccountName();
                string tempMail = await this.GoogleDrive.ReceiveAccountEmail();
                if (tempName != null)
                {
                    this.GoogleDriveAccountName = tempName;
                    this.GoogleDriveHasAccountName = true;
                }
                else
                {
                    this.GoogleDriveAccountName = "No account name specified";
                    this.GoogleDriveHasAccountName = false;

                }
                if (tempMail == null)
                {
                    this.GoogleDriveEmail = "Error retrieving email";
                }
                else
                {
                    this.GoogleDriveEmail = tempMail;
                }
                long[] storageInfo = await this.GoogleDriveInterfaceFunctions.DetermineAvailableCloudSpace(this.GoogleDrive.Service);
                if (storageInfo != null)
                {
                    var availableGB = (double)((double)storageInfo[1] / (double)(1024 * 1024 * 1024));
                    var totalGB = (double)((double)storageInfo[0] / (double)(1024 * 1024 * 1024));
                    var usedGB = (double)((double)totalGB - (double)availableGB);

                    float available = (float)(Math.Truncate((double)availableGB * 100.0) / 100.0);
                    float total = (float)(Math.Truncate((double)totalGB * 100.0) / 100.0);
                    float used = (float)(Math.Truncate((double)usedGB * 100.0) / 100.0);

                    string temp = used + "/" + total + " GB";
                    this.GoogleDriveStorage = temp;

                    var storageOk = (int)((double)((availableGB / totalGB) * 100));

                    if (storageOk >= 40)
                    {
                        this.ProgressBarBorderBrushGoogleDrive = new SolidColorBrush(Colors.Green);
                        this.ProgressBarForegroundGoogleDrive = new SolidColorBrush(Colors.Green);
                    }
                    if (storageOk >= 20 && storageOk < 40)
                    {
                        this.ProgressBarBorderBrushGoogleDrive = new SolidColorBrush(Colors.Orange);
                        this.ProgressBarForegroundGoogleDrive = new SolidColorBrush(Colors.Orange);

                    }
                    if (storageOk < 20)
                    {
                        this.ProgressBarBorderBrushGoogleDrive = new SolidColorBrush(Colors.Red);
                        this.ProgressBarForegroundGoogleDrive = new SolidColorBrush(Colors.Red);

                    }

                    this.ProgressValueGoogleDrive = 100 - storageOk;
                }

            }
            else
            {
                this.GoogleDriveStatus = "Logged out";
                this.GoogleDriveSignInStatus = false;
                this.GoogleDriveAccountName = String.Empty;
                this.GoogleDriveHasAccountName = false;
                this.GoogleDriveEmail = String.Empty;
                this.GoogleDriveStorage = String.Empty;
                this.BtnTextGoogleDrive = "Login";
                this.ProgressBarBorderBrushGoogleDrive = new SolidColorBrush(Colors.Black);
                this.ProgressBarForegroundGoogleDrive = new SolidColorBrush(Colors.White);
                this.ProgressValueGoogleDrive = 0;
                this.ProgressMax = 100;
                this.ProgressMin = 0;

                this.GoogleDriveHasAccountName = false;

            }

        }

        //DropBox
        public bool Dropbox_CanLogin()
        {
            if (this.Dropbox.SignedIn)  //Function ob angemeldet oder nicht ob button login oder logout ist!
            {
                this.ImageSourceDropbox = this.Tick;    //Hackerl für eingeloggt
                this.BtnTextDropbox = "Logout";
            }
            else
            {
                this.ImageSourceDropbox = this.Cross;   // X für ausgeloggt
                this.BtnTextDropbox = "Login";
            }
            return true;

        }
        public async Task Dropbox_LogInOut()
        {
            if (this.Dropbox.SignedIn)
            {
                await this.Dropbox.InitializeSignOut(); //Logout
                this.Dropbox_SetBindings();
            }
            else
            {
                await this.Dropbox.InitializeSignIn();  //Login
                this.Dropbox_SetBindings();
            }
        }
        private async void Dropbox_DoOnStartup()
        {


            bool isSuccessful = await this.Dropbox.AutoLogin(); //Automatisches Anmelden
            if (isSuccessful)
            {
                this.Dropbox_SetBindings();
                this.Btn_LogInOutDropbox.raiseCanExecuteChanged();
            }
            else
            {
                this.Dropbox_SetBindings();
            }
        }
        private async void Dropbox_SetBindings()
        {
            this.Btn_LogInOutDropbox.raiseCanExecuteChanged();

            if (this.Dropbox.SignedIn)
            {
                this.BtnTextDropbox = "Logout";
                this.ProgressMax = 100;
                this.ProgressMin = 0;
                this.DropboxStatus = "Logged In";
                this.DropboxSignInStatus = true;
                string tempName = await this.Dropbox.ReceiveAccountName();  //accountname als string bekommen
                string tempMail = await this.Dropbox.ReceiveAccountEmail(); //account email als string bekommen
                if (tempName != null)   //wenn namen bekommen hat dann soll es namen im interface anzeigen
                {
                    this.DropboxAccountName = tempName; //namen anzeigen in UI
                    this.DropboxHasAccountName = true;
                }
                else
                {
                    this.DropboxAccountName = "No account name specified";  //Kein Name bekommen
                    this.DropboxHasAccountName = false;

                }
                if (tempMail == null)   //Wenn keine email bekommen des angemeldeten accounts
                {
                    this.DropboxEmail = "Error retrieving email";
                }
                else
                {
                    this.DropboxEmail = tempMail;   //email anzeigen auf UI
                }
                long[] storageInfo = await this.DropboxInterfaceFunctions.DetermineAvailableCloudSpace(this.Dropbox.client);   //Speicherstand Dropbox
                if (storageInfo != null)
                {
                    var availableGB = (double)((double)storageInfo[1] / (double)(1024 * 1024 * 1024));  //verfügbarer Speicher
                    var totalGB = (double)((double)storageInfo[0] / (double)(1024 * 1024 * 1024));  //Gesamtspeicher
                    var usedGB = (double)((double)totalGB - (double)availableGB);   //verwendeter Speicher

                    //Typkonvertierung
                    float available = (float)(Math.Truncate((double)availableGB * 100.0) / 100.0);
                    float total = (float)(Math.Truncate((double)totalGB * 100.0) / 100.0);
                    float used = (float)(Math.Truncate((double)usedGB * 100.0) / 100.0);

                    string temp = used + "/" + total + " GB";
                    this.DropboxStorage = temp; // UI Info über Speicherstand

                    var storageOk = (int)((double)((availableGB / totalGB) * 100)); //0%-100% Speicherauslastung

                    if (storageOk >= 40)    //>40% Grün
                    {
                        this.ProgressBarBorderBrushDropbox = new SolidColorBrush(Colors.Green);
                        this.ProgressBarForegroundDropbox = new SolidColorBrush(Colors.Green);
                    }
                    if (storageOk >= 20 && storageOk < 40)  //20-40% Orange
                    {
                        this.ProgressBarBorderBrushDropbox = new SolidColorBrush(Colors.Orange);
                        this.ProgressBarForegroundDropbox = new SolidColorBrush(Colors.Orange);

                    }
                    if (storageOk < 20) //<20% Red
                    {
                        this.ProgressBarBorderBrushDropbox = new SolidColorBrush(Colors.Red);
                        this.ProgressBarForegroundDropbox = new SolidColorBrush(Colors.Red);

                    }

                    this.ProgressValueDropbox = 100 - storageOk;    //UI Speicherstand anzeigen
                }

            }
            else
            {   //Anzeige wenn ausgelogged
                this.DropboxStatus = "Logged out";
                this.DropboxSignInStatus = false;
                this.DropboxAccountName = String.Empty;
                this.DropboxHasAccountName = false;
                this.DropboxEmail = String.Empty;
                this.DropboxStorage = String.Empty;
                this.BtnTextDropbox = "Login";
                this.ProgressBarBorderBrushDropbox = new SolidColorBrush(Colors.Black);
                this.ProgressBarForegroundDropbox = new SolidColorBrush(Colors.White);
                this.ProgressValueDropbox = 0;
                this.ProgressMax = 100;
                this.ProgressMin = 0;

                this.DropboxHasAccountName = false;

            }

        }

        void Dropbox_loginFinished()
        {
            this.Dropbox.authForm.Close();
            this.Dropbox.SignedIn = true;
            this.Dropbox_SetBindings();
            this.Btn_LogInOutDropbox.raiseCanExecuteChanged();
        }

        public ActionCommand Btn_LogInOutOneDrive { get; set; }
        public ActionCommand Btn_LogInOutDropbox { get; set; }
        public ActionCommand Btn_LogInOutGoogleDrive { get; set; }


        public delegate Task OneDriveDelegate();


        public class ActionCommand : ICommand
        {
            private readonly OneDriveDelegate execute;
            private readonly Func<bool> canExecute;

            public ActionCommand(OneDriveDelegate execute, Func<bool> canExecute)
            {
                this.execute = execute;
                this.canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return this.canExecute();
            }

            public event EventHandler CanExecuteChanged = delegate { };

            public void Execute(object parameter)
            {

                this.execute();
            }

            public void raiseCanExecuteChanged()
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }


    }
}

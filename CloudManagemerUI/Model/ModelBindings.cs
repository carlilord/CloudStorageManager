using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CloudManagerUI.Model
{
    class ModelBindings
    {

        private string oneDriveStatus;
        private string googleDriveStatus;
        private string dropboxStatus;
        private string oneDriveAccountName;
        private bool oneDriveHasAccountName;
        private string oneDriveEmail;
        private string oneDriveStorage;
        private string googleDriveAccountName;
        private bool googleDriveHasAccountName;
        private string googleDriveEmail;
        private string googleDriveStorage;
        private string dropboxAccountName;
        private bool dropboxHasAccountName;
        private string dropboxEmail;
        private string dropboxStorage;
        private bool googleDriveSignInStatus;
        private bool oneDriveSignInStatus;
        private bool dropboxSignInStatus;
        private string imageSourceOneDrive;
        private string btnTextOneDrive;
        private int progressValueOneDrive;
        private Brush progressBarBorderBrushOneDrive;
        private Brush progressBarForegroundOneDrive;
        private string imageSourceDropbox;
        private string btnTextDropbox;
        private int progressValueDropbox;
        private Brush progressBarBorderBrushDropbox;
        private Brush progressBarForegroundDropbox;
        private string imageSourceGoogleDrive;
        private string btnTextGoogleDrive;
        private int progressValueGoogleDrive;
        private Brush progressBarBorderBrushGoogleDrive;
        private Brush progressBarForegroundGoogleDrive;
        private int progressMax;
        private int progressMin;

        public int ProgressMax
        {
            get
            {
                return this.progressMax;
            }
            set
            {
                this.progressMax = value;
            }
        }
        public int ProgressMin
        {
            get
            {
                return this.progressMin;
            }
            set
            {
                this.progressMin = value;
            }
        }
        public Brush ProgressBarBorderBrushOneDrive
        {
            get
            {
                return this.progressBarBorderBrushOneDrive;
            }
            set
            {
                this.progressBarBorderBrushOneDrive = value;
            }
        }


        public Brush ProgressBarForegroundOneDrive
        {
            get
            {
                return this.progressBarForegroundOneDrive;
            }
            set
            {
                this.progressBarForegroundOneDrive = value;
            }
        }


        public int ProgressValueOneDrive
        {
            get
            {
                return this.progressValueOneDrive;
            }
            set
            {
                this.progressValueOneDrive = value;
            }
        }
        public string BtnTextOneDrive
        {
            get
            {
                return this.btnTextOneDrive;
            }
            set
            {
                this.btnTextOneDrive = value;
            }
        }

        public string ImageSourceOneDrive
        {
            get
            {
                return this.imageSourceOneDrive;
            }
            set
            {
                this.imageSourceOneDrive = value;
            }
        }
        public Brush ProgressBarBorderBrushDropbox
        {
            get
            {
                return this.progressBarBorderBrushDropbox;
            }
            set
            {
                this.progressBarBorderBrushDropbox = value;
            }
        }


        public Brush ProgressBarForegroundDropbox
        {
            get
            {
                return this.progressBarForegroundDropbox;
            }
            set
            {
                this.progressBarForegroundDropbox = value;
            }
        }


        public int ProgressValueDropbox
        {
            get
            {
                return this.progressValueDropbox;
            }
            set
            {
                this.progressValueDropbox = value;
            }
        }
        public string BtnTextDropbox
        {
            get
            {
                return this.btnTextDropbox;
            }
            set
            {
                this.btnTextDropbox = value;
            }
        }

        public string ImageSourceDropbox
        {
            get
            {
                return this.imageSourceDropbox;
            }
            set
            {
                this.imageSourceDropbox = value;
            }
        }
        public Brush ProgressBarBorderBrushGoogleDrive
        {
            get
            {
                return this.progressBarBorderBrushGoogleDrive;
            }
            set
            {
                this.progressBarBorderBrushGoogleDrive = value;
            }
        }


        public Brush ProgressBarForegroundGoogleDrive
        {
            get
            {
                return this.progressBarForegroundGoogleDrive;
            }
            set
            {
                this.progressBarForegroundGoogleDrive = value;
            }
        }


        public int ProgressValueGoogleDrive
        {
            get
            {
                return this.progressValueGoogleDrive;
            }
            set
            {
                this.progressValueGoogleDrive = value;
            }
        }
        public string BtnTextGoogleDrive
        {
            get
            {
                return this.btnTextGoogleDrive;
            }
            set
            {
                this.btnTextGoogleDrive = value;
            }
        }

        public string ImageSourceGoogleDrive
        {
            get
            {
                return this.imageSourceGoogleDrive;
            }
            set
            {
                this.imageSourceGoogleDrive = value;
            }
        }

        public string OneDriveStatus
        {
            get
            {
                return this.oneDriveStatus;
            }
            set
            {
                this.oneDriveStatus = value;
            }
        }
        public string GoogleDriveStatus
        {
            get
            {
                return this.googleDriveStatus;
            }
            set
            {
                this.googleDriveStatus = value;
            }
        }
        public string DropboxStatus
        {
            get
            {
                return this.dropboxStatus;
            }
            set
            {
                this.dropboxStatus = value;
            }
        }


        public string OneDriveAccountName
        {
            get
            {
                return this.oneDriveAccountName;
            }
            set
            {
                this.oneDriveAccountName = value;
            }
        }
        public bool OneDriveHasAccountName
        {
            get
            {
                return this.oneDriveHasAccountName;
            }
            set
            {
                this.oneDriveHasAccountName = value;
            }
        }
        public string OneDriveEmail
        {
            get
            {
                return this.oneDriveEmail;
            }
            set
            {
                this.oneDriveEmail = value;
            }
        }

        public string OneDriveStorage
        {
            get
            {
                return this.oneDriveStorage;
            }
            set
            {
                this.oneDriveStorage = value;
            }
        }
        public string GoogleDriveAccountName
        {
            get
            {
                return this.googleDriveAccountName;
            }
            set
            {
                this.googleDriveAccountName = value;
            }
        }
        public bool GoogleDriveHasAccountName
        {
            get
            {
                return this.googleDriveHasAccountName;
            }
            set
            {
                this.googleDriveHasAccountName = value;
            }
        }
        public string GoogleDriveEmail
        {
            get
            {
                return this.googleDriveEmail;
            }
            set
            {
                this.googleDriveEmail = value;
            }
        }

        public string GoogleDriveStorage
        {
            get
            {
                return this.googleDriveStorage;
            }
            set
            {
                this.googleDriveStorage = value;
            }
        }
        public string DropboxAccountName
        {
            get
            {
                return this.dropboxAccountName;
            }
            set
            {
                this.dropboxAccountName = value;
            }
        }
        public bool DropboxHasAccountName
        {
            get
            {
                return this.dropboxHasAccountName;
            }
            set
            {
                this.dropboxHasAccountName = value;
            }
        }
        public string DropboxEmail
        {
            get
            {
                return this.dropboxEmail;
            }
            set
            {
                this.dropboxEmail = value;
            }
        }

        public string DropboxStorage
        {
            get
            {
                return this.dropboxStorage;
            }
            set
            {
                this.dropboxStorage = value;
            }
        }
        public bool GoogleDriveSignInStatus
        {
            get
            {
                return this.googleDriveSignInStatus;
            }
            set
            {
                this.googleDriveSignInStatus = value;
            }
        }
        public bool OneDriveSignInStatus
        {
            get
            {
                return this.oneDriveSignInStatus;
            }
            set
            {
                this.oneDriveSignInStatus = value;
            }
        }
        public bool DropboxSignInStatus
        {
            get
            {
                return this.dropboxSignInStatus;
            }
            set
            {
                this.dropboxSignInStatus = value;
            }
        }
    }
}

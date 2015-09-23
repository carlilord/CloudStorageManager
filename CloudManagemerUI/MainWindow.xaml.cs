using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro;
using Microsoft.Live;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.IO;
using CloudManagerUI.OneDrive;
using CloudManagerUI.Model;
using System.ComponentModel;
using CloudManagerUI.ViewModel;
using CloudManagerUI.Communication;
using System.ServiceModel;
using System.ServiceProcess;
using CloudManagerUI.Intelligente_Algorithmen;




namespace CloudManagerUI
{


    public partial class MainWindow : MetroWindow
    {
        private CloudManagerViewModel cmv;
        
        public MainWindow()
        {
            InitializeComponent();
            cmv = new CloudManagerViewModel();
            this.Loaded += cmv.MainWindow_Loaded;
            this.DataContext = cmv;        //model-class
            this.setElements();

            
            //WCFConnectionWorker wcw = new WCFConnectionWorker(iu.ReceivedListLocal); //funktion von intelligentenupload einhängen und in der funktion mit stringarray filelist machen und datenbankeintrag macchen
            //wcw.StartService();
            //wcw.RequestFileList();
        }

        private void setElements()
        {
            Color color = new Color();
            color.A = 0x00;
            color.R = 0x11;
            color.G = 0x9E;
            color.B = 0xDA;

            Color spec_color = new Color();
            spec_color.A = 0xCC;
            spec_color.R = 0x11;
            spec_color.G = 0x9E;
            spec_color.B = 0xDA;

            Color spec_white = new Color();
            spec_white.A = 0x00;
            spec_white.R = 0xFF;
            spec_white.G = 0xFF;
            spec_white.B = 0xFF;

            SolidColorBrush brush = new SolidColorBrush(color);
            SolidColorBrush white = new SolidColorBrush(Colors.White);
            SolidColorBrush spec_blue = new SolidColorBrush(spec_color);
            SolidColorBrush spec_white_brush = new SolidColorBrush(spec_white);

            SolidColorBrush black = new SolidColorBrush(Colors.Black);



            this.btnAbout.Background = brush;
            this.btnAbout.Foreground = spec_white_brush;


            this.Flyout.Theme = FlyoutTheme.Accent;

            this.TitleCaps = false;
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            this.Flyout.IsOpen = !this.Flyout.IsOpen;
            
        }
    }
}

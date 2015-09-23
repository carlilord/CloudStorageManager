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



namespace CloudManagemerUI
{
    
    
    public partial class MainWindow : MetroWindow
    {
        

        public MainWindow()
        {
            InitializeComponent();

            
            
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            this.Flyout.IsOpen = !this.Flyout.IsOpen;
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}

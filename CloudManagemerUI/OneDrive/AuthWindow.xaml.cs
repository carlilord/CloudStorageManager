using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace CloudManagerUI
{
    /// <summary>
    /// Interaktionslogik für AuthWindow.xaml
    /// </summary>
    /// 
    public delegate void AuthCompletedCallback(AuthResult result);
    public delegate void LogoutCompletedCallback();
    public partial class AuthWindow : MetroWindow
    {
        private string startUrl;
        private string endUrl;
        private LogoutCompletedCallback callback_l;
        private AuthCompletedCallback callback;

        public AuthWindow(string startUrl, string endUrl, LogoutCompletedCallback callback)
        {
            this.startUrl = startUrl;
            this.endUrl = endUrl;
            this.callback_l = callback;
            InitializeComponent();
        }
        public AuthWindow(string startUrl, string endUrl, AuthCompletedCallback callback)
        {
            this.startUrl = startUrl;
            this.endUrl = endUrl;
            this.callback = callback;
            InitializeComponent();
        }

        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Webbrowser.Navigated += Webbrowser_Navigated;
            this.Webbrowser.Navigate(this.startUrl);
            
        }

        void Webbrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.StartsWith(this.endUrl))
            {

                if (this.callback != null)
                {
                    this.callback(new AuthResult(e.Uri));

                }

            }
            if (this.callback_l != null)
            {
                this.callback_l();
            }

        }
    }
}

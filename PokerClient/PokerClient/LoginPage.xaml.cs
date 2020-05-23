using PokerClient.PokerServiceRef;
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

namespace PokerClient
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private PokerServiceClient client;
        public LoginPage()
        {
            InitializeComponent();
            client = MainWindow.client;
        }
        
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = userNameTB.Text;
            string password = passwordBox.Password;
            var serverResponse = client.Login(username, password);
            if (serverResponse.ErrorMsg == null)
            {
                MessageBox.Show("Login successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                TableMenuPage p = new TableMenuPage(username);
                this.NavigationService.Navigate(p,UriKind.Relative);
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SignupBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = userNameTB.Text;
            string password = passwordBox.Password;
            var serverResponse = client.SignUp(username, password);
            if (serverResponse.ErrorMsg == null)
            {
                MessageBox.Show("Signup successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                TableMenuPage p = new TableMenuPage(username);
                this.NavigationService.Navigate(p, UriKind.Relative);
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

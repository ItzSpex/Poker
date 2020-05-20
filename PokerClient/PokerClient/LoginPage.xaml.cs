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
                TableMenuPage t = new TableMenuPage(username);
                this.NavigationService.Navigate(t,UriKind.Relative);
            }
            else
            {
                ExceptionBox.Text = serverResponse.ErrorMsg.ToString();
            }
        }

        private void SignupBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = userNameTB.Text;
            string password = passwordBox.Password;
            var serverRespone = client.SignUp(username, password);
            if (serverRespone.ErrorMsg == null)
                ExceptionBox.Text = "User Id: " + serverRespone.Result.ToString();
            else
                ExceptionBox.Text = serverRespone.ErrorMsg.ToString();
        }
    }
}

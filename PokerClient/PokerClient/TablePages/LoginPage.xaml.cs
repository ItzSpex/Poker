﻿using PokerClient.PokerServiceRef;
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
        private int Wallet = 0;
        public LoginPage()
        {
            InitializeComponent();
            client = MainWindow.client;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = userNameTB.Text;
            string password = passwordBox.Password;
            if (username == "" || password == "")
            {
                MessageBox.Show("An unhandled exception just occurred: Please enter credentials correctly", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var serverResponse = client.Login(username, password);
                if (serverResponse.ErrorMsg == null)
                {
                    if (InitWallet())
                    {
                        MessageBox.Show("Login successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        TableMenuPage p = new TableMenuPage(username, Wallet);
                        this.NavigationService.Navigate(p, UriKind.Relative);
                    }
                }
                else
                {
                    MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SignupBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = userNameTB.Text;
            string password = passwordBox.Password;
            if (username == "" || password == "")
            {
                MessageBox.Show("An unhandled exception just occurred: Please enter credentials correctly", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var serverResponse = client.SignUp(username, password);
                if (serverResponse.ErrorMsg == null)
                {
                    if (InitWallet())
                    {
                        MessageBox.Show("Signup successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        TableMenuPage p = new TableMenuPage(username, Wallet);
                        this.NavigationService.Navigate(p, UriKind.Relative);
                    }
                }
                else
                {
                    MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private bool InitWallet()
        {
            var serverResponse = client.GetWallet();
            if (serverResponse.ErrorMsg == null)
            {
                Wallet = serverResponse.Result;
                return true;
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }
    }
}

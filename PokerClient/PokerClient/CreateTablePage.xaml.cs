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
    /// Interaction logic for CreateTablePage.xaml
    /// </summary>
    public partial class CreateTablePage : Page
    {
        private string Username = "";
        private PokerTableBL currTable;
        public CreateTablePage(string username)
        {
            InitializeComponent();
            Username = username;
        }

        private void CreateTableBtn_Click(object sender, RoutedEventArgs e)
        {
            string Name = "";
            int Players = 0, MinBet = 0;
            Name = TableNameTB.Text;
            Players = (int)playerSlider.Value;
            MinBet = (int)BetSlider.Value;
            if (Name == "")
            {
                MessageBox.Show("An unhandled exception just occurred: Please enter a table name", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var serverResponse = MainWindow.client.CreateTable(Name, Players, MinBet);
                if (serverResponse.ErrorMsg == null)
                {
                    currTable = serverResponse.Result;
                    MessageBox.Show("Table created successfully", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    GameLobbyPage p = new GameLobbyPage(currTable, Username);
                    this.NavigationService.Navigate(p, UriKind.Relative);
                }
                else
                {
                    MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            TableMenuPage t = new TableMenuPage(Username);
            this.NavigationService.Navigate(t, UriKind.Relative);
        }

        private bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}

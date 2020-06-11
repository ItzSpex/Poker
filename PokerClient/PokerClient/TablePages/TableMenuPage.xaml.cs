using PokerClient.HistoryPages;
using PokerClient.PokerServiceRef;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PokerClient
{
    /// <summary>
    /// Interaction logic for RoomMenuWindow.xaml
    /// </summary>
    public partial class TableMenuPage : Page
    {
        private string Username = "";
        private int Wallet;
        private PokerTableBL currTable;
        public ObservableCollection<PokerTableBL> PokerTables { get; set; }

        public TableMenuPage(string username, int wallet)
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            PokerTables = new ObservableCollection<PokerTableBL>(MainWindow.client.GetExistingTables().Result);
            TableList.ItemsSource = PokerTables;
            Username = username;
            UsernameTB.Text = username;
            Wallet = wallet;
            WalletTB.Text = wallet.ToString();
        }

        private void CreateTable_Btn_Click(object sender, RoutedEventArgs e)
        {
            CreateTablePage p = new CreateTablePage(Username, Wallet);
            this.NavigationService.Navigate(p, UriKind.Relative);
        }

        private void RefreshList_Btn_Click(object sender, RoutedEventArgs e)
        {
            PokerTables = new ObservableCollection<PokerTableBL>(MainWindow.client.GetExistingTables().Result);
            TableList.ItemsSource = PokerTables;
        }

        private void JoinTable_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.JoinTable(currTable.PokerTableId);
            if (serverResponse.ErrorMsg == null)
            {
                currTable = serverResponse.Result;
                MessageBox.Show("Join table request successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                GameLobbyPage p = new GameLobbyPage(currTable, Username, Wallet);
                this.NavigationService.Navigate(p, UriKind.Relative);
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Logout_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.Logout();
            if(serverResponse.ErrorMsg == null)
            {
                MessageBox.Show("Logout request successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginPage p = new LoginPage();
                this.NavigationService.Navigate(p, UriKind.Relative);
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                currTable = (PokerTableBL)TableList.SelectedItems[0];
                JoinTable_Btn.IsEnabled = true;
            }
            catch
            {
                TableList.UnselectAll();
            }
            
        }

        private void History_Btn_Click(object sender, RoutedEventArgs e)
        {
            HistoryPage p = new HistoryPage(Username, Wallet);
            this.NavigationService.Navigate(p, UriKind.Relative);
        }

    }
}

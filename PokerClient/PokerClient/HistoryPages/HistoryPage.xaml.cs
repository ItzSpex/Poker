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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokerClient.HistoryPages
{
    /// <summary>
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        private string Username = "";
        private int Wallet = 0;
        public ObservableCollection<PokerTable> HistoryTables;
        private PokerTable currTable;
        public HistoryPage(string username, int wallet)
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Username = username;
            Wallet = wallet;
            DataContext = this;
            try
            {
                HistoryTables = new ObservableCollection<PokerTable>(MainWindow.client.GetHistoryTables().Result);
            }
            catch (Exception e)
            {
                MessageBox.Show("An unhandled exception just occurred: " + e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            TableHistoryList.ItemsSource = HistoryTables;
        }

        private void TableHistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                currTable = (PokerTable)TableHistoryList.SelectedItems[0];
                Replay_Btn.IsEnabled = true;
            }
            catch
            {
                TableHistoryList.UnselectAll();
            }
        }

        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            TableMenuPage p = new TableMenuPage(Username, Wallet);
            this.NavigationService.Navigate(p, UriKind.Relative);
        }

        private void Refresh_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HistoryTables = new ObservableCollection<PokerTable>(MainWindow.client.GetHistoryTables().Result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unhandled exception just occurred: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            TableHistoryList.ItemsSource = HistoryTables;
        }

        private void Replay_Btn_Click(object sender, RoutedEventArgs e)
        {
            PokerTableBL pokerTable;
            var serverResponse = MainWindow.client.GetTableByHistory(currTable);
            if(serverResponse.ErrorMsg == null)
            {
                pokerTable = serverResponse.Result;
                GamePage p = new GamePage(pokerTable.NumOfPlayers, pokerTable.Players.ToList(), pokerTable.MinBet, Wallet, true, currTable.Id);
                this.NavigationService.Navigate(p, UriKind.Relative);
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}

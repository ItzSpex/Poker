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
        private PokerTableBL currTable;
        public ObservableCollection<PokerTableBL> pokerTables { get; set; }

        public TableMenuPage(string username)
        {
            InitializeComponent();
            pokerTables = new ObservableCollection<PokerTableBL>(MainWindow.client.GetExistingTables().Result);
            TableList.ItemsSource = pokerTables;
            Username = username;
            UsernameTB.Text = username;
        }

        private void CreateTable_Btn_Click(object sender, RoutedEventArgs e)
        {
            CreateTablePage p = new CreateTablePage(Username);
            this.NavigationService.Navigate(p, UriKind.Relative);
        }

        private void RefreshList_Btn_Click(object sender, RoutedEventArgs e)
        {
            pokerTables = new ObservableCollection<PokerTableBL>(MainWindow.client.GetExistingTables().Result);
            TableList.ItemsSource = pokerTables;
        }

        private void JoinTable_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.JoinTable(currTable.PokerTableId);
            if (serverResponse.ErrorMsg == null)
            {
                MessageBox.Show("Join table request successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                GameLobbyPage p = new GameLobbyPage(currTable);
                this.NavigationService.Navigate(p, UriKind.Relative);
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Logout_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currTable = (PokerTableBL)TableList.SelectedItems[0];
            JoinTable_Btn.IsEnabled = true;
            
        }
    }
}

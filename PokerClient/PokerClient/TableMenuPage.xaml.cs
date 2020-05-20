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
        public ObservableCollection<PokerTableBL> pokerTables { get; set; }

        public TableMenuPage(string username)
        {
            InitializeComponent();
            pokerTables = new ObservableCollection<PokerTableBL>(MainWindow.client.GetExistingTables().Result);
            TableList.ItemsSource = pokerTables;
            UsernameTB.Text = username;
        }

        private void CreateTable_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.client.CreateTable("A", 2, 200);
        }
    }
}

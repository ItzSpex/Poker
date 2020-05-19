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
        public TableMenuPage()
        {
            InitializeComponent();
            //ObservableCollection<PokerTableBL> pokerTables = new ObservableCollection<PokerTableBL>(List<PokerTableBL>);
            //TableList.ItemsSource = pokerTables

        }
    }
}

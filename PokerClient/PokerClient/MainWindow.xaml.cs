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
using PokerClient.PokerServiceRef;
namespace PokerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static PokerServiceClient client = new PokerServiceClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                var serverResponse = MainWindow.client.Logout();
                if (serverResponse.ErrorMsg == null)
                {
                    MessageBox.Show("Close client request successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                //Server crashed
            }
        }
    }
}

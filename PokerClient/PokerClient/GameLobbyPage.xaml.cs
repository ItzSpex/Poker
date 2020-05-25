using PokerClient.PokerServiceRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace PokerClient
{
    /// <summary>
    /// Interaction logic for GameLobbyPage.xaml
    /// </summary>
    public partial class GameLobbyPage : Page
    {
        DispatcherTimer myTimer = new DispatcherTimer();
        public GameLobbyPage(PokerTableBL pokerTable)
        {
            InitializeComponent();
            NameLabel.Content = pokerTable.PokerTableName;
            MaxPlayersLabel.Content = pokerTable.NumOfPlayers.ToString();
            MinBetLabel.Content = pokerTable.MinBet.ToString();
            myTimer.Tick += new EventHandler(GetPlayersNames);
            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Start();
        }
        public GameLobbyPage(string Name, int MaxPlayers, int MinBet)
        {
            InitializeComponent();
            NameLabel.Content = Name;
            MaxPlayersLabel.Content = MaxPlayers.ToString();
            MinBetLabel.Content = MinBet.ToString();
            myTimer.Tick += new EventHandler(GetPlayersNames);
            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Start();
        }
        public void GetPlayersNames(Object sender, EventArgs e)
        {
            List<string> playerNames;
            var serverResponse = MainWindow.client.GetCurrPlayerNames();
            string currPlayers = "";
            if (serverResponse.ErrorMsg == null)
            {
                playerNames = serverResponse.Result.ToList();
                foreach (string playerName in playerNames)
                {
                    currPlayers += playerName + ", ";
                }
                currPlayers = currPlayers.Substring(0, currPlayers.Length - 2);
                CurrPlayersLabel.Content = currPlayers;
            }
            
        }
    }
}

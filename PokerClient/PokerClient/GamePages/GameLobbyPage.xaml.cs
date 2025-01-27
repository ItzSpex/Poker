﻿using PokerClient.PokerServiceRef;
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
        int MinBet = 0;
        string Username = "";
        int NumOfPlayers = 0;
        int Wallet = 0;
        List<PlayerBL> Players;
        DispatcherTimer myTimer = new DispatcherTimer();
        public GameLobbyPage(PokerTableBL pokerTable, string username, int wallet)
        {
            InitializeComponent();
            NameLabel.Content = pokerTable.PokerTableName;
            NumOfPlayers = pokerTable.NumOfPlayers;
            MaxPlayersLabel.Content = NumOfPlayers.ToString();
            MinBetLabel.Content = pokerTable.MinBet.ToString();
            myTimer.Tick += new EventHandler(GetPlayersNamesAndGameStatus);
            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Start();
            Username = username;
            Wallet = wallet;
            Players = pokerTable.Players.ToList();
            MinBet = pokerTable.MinBet;
        }
        public void GetPlayersNamesAndGameStatus(Object sender, EventArgs e)
        {
            string PlayerNames = "";
            var serverResponse2 = MainWindow.client.UpdatePlayers();
            if (serverResponse2.ErrorMsg == null)
            {
                Players = serverResponse2.Result.ToList();
                foreach (PlayerBL player in Players)
                {
                    PlayerNames += player.PlayerName + ", ";
                }
                if (PlayerNames.Length > 0)
                {
                    PlayerNames = PlayerNames.Substring(0, PlayerNames.Length - 2);
                }
                CurrPlayersLabel.Content = PlayerNames;
            }
            var serverResponse = MainWindow.client.HasGameStarted();
            if (serverResponse.Result)
            {
                GamePage p = new GamePage(NumOfPlayers, Players, MinBet, Wallet, false, 0);
                this.NavigationService.Navigate(p, UriKind.Relative);
                myTimer.Stop();
            }
        }

        private void LeaveTable_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.LeaveTable();
            if (serverResponse.ErrorMsg == null)
            {
                MessageBox.Show("Leave table request successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                TableMenuPage p = new TableMenuPage(Username, Wallet);
                this.NavigationService.Navigate(p, UriKind.Relative);
                myTimer.Stop();
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

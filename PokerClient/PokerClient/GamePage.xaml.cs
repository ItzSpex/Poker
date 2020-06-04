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
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        int NumOfPlayers, myId;
        Dictionary<int, OpponentControl> PlayerControls;
        List<PlayerBL> Players;
        List<PlayerBL> SortedPlayers;
        PlayerBL myPlayer;
        List<OpponentControl> Opponents;
        public GamePage(int NumOfPlayers, List<PlayerBL> Players)
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            PlayerControls = new Dictionary<int, OpponentControl>();
            Opponents = new List<OpponentControl>();
            this.NumOfPlayers = NumOfPlayers;
            this.Players = Players;
            SortedPlayers = new List<PlayerBL>();
            InitGame();
        }
        public void InitGame()
        {
            var serverResponse = MainWindow.client.StartGame();
            if (serverResponse.ErrorMsg == null)
            {
                myId = serverResponse.Result.PlayerId;
                InitControlsList();
                SortOpponents();
                InitPlayers(serverResponse.Result.PlayerCard1, serverResponse.Result.PlayerCard2);
                InitPreFlop(serverResponse.Result);
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void InitControlsList()
        {
            switch (NumOfPlayers)
            {
                case 2:
                    Opponents.Add(FourthPlayer);
                    break;
                case 3:
                    Opponents.Add(ThirdPlayer);
                    Opponents.Add(FifthPlayer);
                    break;
                case 4:
                    Opponents.Add(ThirdPlayer);
                    Opponents.Add(FourthPlayer);
                    Opponents.Add(FifthPlayer);
                    break;
                case 5:
                    Opponents.Add(SecondPlayer);
                    Opponents.Add(ThirdPlayer);
                    Opponents.Add(FifthPlayer);
                    Opponents.Add(SixthPlayer);
                    break;
                case 6:
                    Opponents.Add(SecondPlayer);
                    Opponents.Add(ThirdPlayer);
                    Opponents.Add(FourthPlayer);
                    Opponents.Add(FifthPlayer);
                    Opponents.Add(SixthPlayer);
                    break;
            }
            foreach (OpponentControl Opponent in Opponents)
            {
                Opponent.Visibility = Visibility.Visible;
            }
        }
        public void SortOpponents()
        {
            int myIndex = 0;
            //Finding my Index:
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].PlayerId == myId)
                {
                    myIndex = i;
                    break;
                }
            }
            //Organizing Opponents:
            for (int i = 0; i < myIndex; i++)
            {
                PlayerControls.Add(Players[i].PlayerId, Opponents[i]);
                SortedPlayers.Add(Players[i]);
            }
            for (int i = myIndex + 1; i < Players.Count; i++)
            {
                PlayerControls.Add(Players[i].PlayerId, Opponents[i - 1]);
                SortedPlayers.Add(Players[i]);
            }
            myPlayer = Players[myIndex];
        }

        public void InitPlayers(Card firstCard, Card secondCard)
        {
            UpdateMyPlayer(FirstPlayer, firstCard, secondCard);
            for (int i = 0; i < PlayerControls.Keys.Count; i++)
            {
                UpdatePlayerName(PlayerControls[SortedPlayers[i].PlayerId], SortedPlayers[i].PlayerName);
            }
        }
        public void UpdateMyPlayer(MyControl playerControl, Card firstCard, Card secondCard)
        {
            playerControl.PlayerNameLabel.Content = myPlayer.PlayerName;
            playerControl.FirstCardImage.Source = GetImage(ToPath(firstCard));
            playerControl.SecondCardImage.Source = GetImage(ToPath(secondCard));
        }
        public void UpdatePlayerName(OpponentControl opponent, string PlayerName)
        {
            opponent.PlayerNameLabel.Content = PlayerName;
        }
        public string ToPath(Card card)
        {
            return "Resources/" + card.Suit + "," + card.Face + ".png";
        }
        private static BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        public void InitPreFlop(StartGameStatus GameStatus)
        {
            //Checking if the this Player is the Dealer/BigBlind/SmallBlind
            if (myPlayer.PlayerId == GameStatus.DealerId)
            {
                FirstPlayer.Dealer_Button.Visibility = Visibility.Visible;
            }
            if (myPlayer.PlayerId == GameStatus.SmallBlind.PlayerId)
            {
                FirstPlayer.SmallBlind_Button.Visibility = Visibility.Visible;
                FirstPlayer.BetAmount = GameStatus.SmallBlind.BidAmount;
                FirstPlayer.BetLabel.Content = FirstPlayer.BetAmountStr;
            }
            if (myPlayer.PlayerId == GameStatus.BigBlind.PlayerId)
            {
                FirstPlayer.BigBlind_Button.Visibility = Visibility.Visible;
                FirstPlayer.BetAmount = GameStatus.SmallBlind.BidAmount;
                FirstPlayer.BetLabel.Content = FirstPlayer.BetAmountStr;
            }
            //Checking if the opponents are the Dealer/BigBlind/SmallBlind 
            for (int i = 0; i < PlayerControls.Keys.Count; i++)
            {
                UpdatePlayerPreFlopStatus(PlayerControls[SortedPlayers[i].PlayerId], SortedPlayers[i].PlayerId, GameStatus);
            }
        }
        public void UpdatePlayerPreFlopStatus(OpponentControl opponent, int OpponentId, StartGameStatus GameStatus)
        {
            if (OpponentId == GameStatus.DealerId)
            {
                opponent.Dealer_Button.Visibility = Visibility.Visible;
            }
            if (OpponentId == GameStatus.SmallBlind.PlayerId)
            {
                opponent.SmallBlind_Button.Visibility = Visibility.Visible;
                opponent.BetAmount = GameStatus.SmallBlind.BidAmount;
                opponent.BetLabel.Content = opponent.BetAmountStr;
            }
            if (OpponentId == GameStatus.BigBlind.PlayerId)
            {
                opponent.BigBlind_Button.Visibility = Visibility.Visible;
                opponent.BetAmount = GameStatus.BigBlind.BidAmount;
                opponent.BetLabel.Content = opponent.BetAmountStr;
            }
        }
    }
}

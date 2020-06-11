using PokerClient.PokerServiceRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using System.Windows.Threading;

namespace PokerClient
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        int NumOfPlayers, myId, MinBet, MaxBet, Wallet, TableId;
        Dictionary<int, OpponentControl> PlayerControls;
        List<PlayerBL> Players;
        List<PlayerBL> SortedPlayers;
        PlayerBL myPlayer;
        List<OpponentControl> Opponents;
        DispatcherTimer myTimer = new DispatcherTimer();
        bool IsReplay;
        public GamePage(int NumOfPlayers, List<PlayerBL> Players, int MinBet, int wallet, bool isreplay, int TableId)
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            PlayerControls = new Dictionary<int, OpponentControl>();
            Opponents = new List<OpponentControl>();
            this.NumOfPlayers = NumOfPlayers;
            this.Players = Players;
            SortedPlayers = new List<PlayerBL>();
            this.MinBet = MinBet;
            this.MaxBet = MinBet * 20;
            this.Wallet = wallet;
            this.IsReplay = isreplay;
            this.TableId = TableId;
            myTimer.Tick += new EventHandler(UpdateTable);
            myTimer.Interval = new TimeSpan(0, 0, 1);
            InitGame();
            
        }
        public void InitGame()
        {
            if(IsReplay)
            {
                var serverResponse1 = MainWindow.client.InitHistoryMoves(TableId);
                if(serverResponse1.ErrorMsg != null)
                {
                    MessageBox.Show("An unhandled exception just occurred: " + serverResponse1.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            var serverResponse = MainWindow.client.StartGame();
            if (serverResponse.ErrorMsg == null)
            {
                myId = serverResponse.Result.PlayerId;
                InitControlsList();
                SortOpponents();
                InitRaiseSlider();
                InitPlayers(serverResponse.Result.PlayerCard1, serverResponse.Result.PlayerCard2);
                UpdateTableCards(null);
                if(IsReplay)
                {
                    Call_Btn.Visibility = Visibility.Hidden;
                    Raise_Btn.Visibility = Visibility.Hidden;
                    Raise_Slider.Visibility = Visibility.Hidden;
                    Fold_Btn.Visibility = Visibility.Hidden;
                    Check_Btn.Visibility = Visibility.Hidden;
                    All_In_Btn.Visibility = Visibility.Hidden;
                    Raise_Label.Visibility = Visibility.Hidden;
                    Raise.Visibility = Visibility.Hidden;
                }
                else
                {
                    Next_Move_Btn.Visibility = Visibility.Hidden;
                }
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
        public void UpdateTableCards(List<Card> cards)
        {
            Card dummyCard = new Card()
            {
                Suit = "Card",
                Face = "Back"
            };
            if (cards != null)
            {
                switch(cards.Count)
                {
                    case 3:
                        myTableControl.TableFirstCardImage.Source = GetImage(ToPath(cards[0]));
                        myTableControl.TableSecondCardImage.Source = GetImage(ToPath(cards[1]));
                        myTableControl.TableThirdCardImage.Source = GetImage(ToPath(cards[2]));
                        myTableControl.TableFourthCardImage.Source = GetImage(ToPath(dummyCard));
                        myTableControl.TableFifthCardImage.Source = GetImage(ToPath(dummyCard));
                        break;
                    case 4:
                        myTableControl.TableFirstCardImage.Source = GetImage(ToPath(cards[0]));
                        myTableControl.TableSecondCardImage.Source = GetImage(ToPath(cards[1]));
                        myTableControl.TableThirdCardImage.Source = GetImage(ToPath(cards[2]));
                        myTableControl.TableFourthCardImage.Source = GetImage(ToPath(cards[3]));
                        myTableControl.TableFifthCardImage.Source = GetImage(ToPath(dummyCard));
                        break;
                    case 5:
                        myTableControl.TableFirstCardImage.Source = GetImage(ToPath(cards[0]));
                        myTableControl.TableSecondCardImage.Source = GetImage(ToPath(cards[1]));
                        myTableControl.TableThirdCardImage.Source = GetImage(ToPath(cards[2]));
                        myTableControl.TableFourthCardImage.Source = GetImage(ToPath(cards[3]));
                        myTableControl.TableFifthCardImage.Source = GetImage(ToPath(cards[4]));
                        break;
                }
            }
            else
            {
                myTableControl.TableFirstCardImage.Source = GetImage(ToPath(dummyCard));
                myTableControl.TableSecondCardImage.Source = GetImage(ToPath(dummyCard));
                myTableControl.TableThirdCardImage.Source = GetImage(ToPath(dummyCard));
                myTableControl.TableFourthCardImage.Source = GetImage(ToPath(dummyCard));
                myTableControl.TableFifthCardImage.Source = GetImage(ToPath(dummyCard));
            }
        }
        public void UpdatePlayerName(OpponentControl opponent, string PlayerName)
        {
            opponent.PlayerNameLabel.Content = PlayerName;
        }
        public string ToPath(Card card)
        {
            return "../Resources/" + card.Suit + "," + card.Face + ".png";
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
                FirstPlayer.BetAmount = GameStatus.BigBlind.BidAmount;
                FirstPlayer.BetLabel.Content = FirstPlayer.BetAmountStr;
            }
            //Checking if the opponents are the Dealer/BigBlind/SmallBlind 
            for (int i = 0; i < PlayerControls.Keys.Count; i++)
            {
                UpdatePlayerPreFlopStatus(PlayerControls[SortedPlayers[i].PlayerId], SortedPlayers[i].PlayerId, GameStatus);
            }
            myTimer.Start();
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
        public void InitRaiseSlider()
        {
            Raise_Slider.Minimum = MinBet;
            Raise_Slider.Maximum = MaxBet;
            Raise_Slider.TickFrequency = MinBet / 2;
        }
        private void Call_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.ExecuteMove(Operation.Call, Opponents[Opponents.Count-1].BetAmount); 
            if (serverResponse.ErrorMsg == null)
            {
                DisableAllButtons();
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Raise_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.ExecuteMove(Operation.Raise, Convert.ToInt32(Raise_Slider.Value));
            if (serverResponse.ErrorMsg == null)
            {
                DisableAllButtons();
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Fold_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.ExecuteMove(Operation.Fold, 0);
            if (serverResponse.ErrorMsg == null)
            {
                DisableAllButtons();
                FirstPlayer.IsEnabled = false;
                MessageBox.Show("Folded, Wait for game to end.", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void All_In_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.ExecuteMove(Operation.AllIn, 0);
            if (serverResponse.ErrorMsg == null)
            {
                DisableAllButtons();
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Next_Move_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.ExecuteHistoryMove();
            if(serverResponse.ErrorMsg == null)
            {

            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Check_Btn_Click(object sender, RoutedEventArgs e)
        {
            var serverResponse = MainWindow.client.ExecuteMove(Operation.Check, FirstPlayer.BetAmount);
            if (serverResponse.ErrorMsg == null)
            {
                DisableAllButtons();
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateTable(Object sender, EventArgs e)
        {
            bool HasWon = false;
            TableStatus tableStatus;
            try
            {
                var serverResponse = MainWindow.client.GetTableStatus();
                if (serverResponse.ErrorMsg == null)
                {
                    tableStatus = serverResponse.Result;
                    if (!tableStatus.HasGameFinished)
                    {
                        UpdateLastMove(tableStatus.LastMove);
                        UpdateTableCards(tableStatus.TableCards.ToList());
                        if (tableStatus.IsMyTurn)
                        {
                            if(!IsReplay)
                            {
                                if (tableStatus.LastMove.BidAmount == FirstPlayer.BetAmount)
                                {
                                    All_In_Btn.IsEnabled = true;
                                    Raise_Btn.IsEnabled = true;
                                    Check_Btn.IsEnabled = true;
                                    Raise_Slider.IsEnabled = true;
                                }
                                else
                                {
                                    Call_Btn.IsEnabled = true;
                                    Raise_Btn.IsEnabled = true;
                                    Fold_Btn.IsEnabled = true;
                                    Raise_Slider.IsEnabled = true;
                                }
                            }
                        }
                        else
                        {
                            DisableAllButtons();
                        }
                    }
                    else
                    {
                        myTimer.Stop();
                        if(!IsReplay)
                        {
                            foreach (int id in tableStatus.WinnerIds.ToList())
                            {
                                if (id == myId)
                                {
                                    HasWon = true;
                                }
                            }
                            if (HasWon)
                            {
                                MessageBox.Show("Congratulations, You won and your winnings will be transferred to your account", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("You lost, Better luck next time!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            UpdateTableCards(tableStatus.TableCards.ToList());
                            MessageBox.Show("Game Ended!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        var serverResponse2 = MainWindow.client.LeaveTable();
                        if (serverResponse2.ErrorMsg == null)
                        {
                            var serverResponse3 = MainWindow.client.GetWallet();
                            if(serverResponse3.ErrorMsg ==null)
                            {
                                MessageBox.Show("Leave table request successful", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                                TableMenuPage p = new TableMenuPage(myPlayer.PlayerName, serverResponse3.Result);
                                this.NavigationService.Navigate(p, UriKind.Relative);
                            }
                            else
                            {
                                MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("An unhandled exception just occurred: " + serverResponse.ErrorMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
            }
        }

        public void UpdateLastMove(Move lastMove)
        {
            //Checking if the user is the opponent or the curr client:
            if(lastMove.PlayerId == myId)
            {
                FirstPlayer.BetAmount = lastMove.BidAmount;
                FirstPlayer.BetLabel.Content = FirstPlayer.BetAmountStr;
                if(lastMove.Operation == Operation.Fold)
                {
                    FirstPlayer.IsEnabled = false;
                }
            }
            else
            {
                OpponentControl opponent = PlayerControls[lastMove.PlayerId];
                opponent.BetAmount = lastMove.BidAmount;
                opponent.BetLabel.Content = opponent.BetAmountStr;
                if (lastMove.Operation == Operation.Fold)
                {
                    opponent.IsEnabled = false;
                }
            }
        }
        public void DisableAllButtons()
        {
            Call_Btn.IsEnabled = false;
            Raise_Btn.IsEnabled = false;
            Raise_Slider.IsEnabled = false;
            Fold_Btn.IsEnabled = false;
            Check_Btn.IsEnabled = false;
            All_In_Btn.IsEnabled = false;
        }
    }
}

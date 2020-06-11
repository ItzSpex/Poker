using PokerBL.ORM;
using PokerDL.Mapping;
using PokerDL.Models;
using PokerHandEvaluator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static PokerHandEvaluator.HandEvaluator;

namespace PokerBL.Models
{
    public class ServiceHandler
    {
        public enum PlayerStates { InMenu, InTable, InGame };
        public static List<UserInfo> LoggedInUsers;
        public static List<PokerTableBL> Tables;
        public static List<int> PlayerIds;
        public static List<int> WinnerIds;
        public List<Move> HistoryMoves;
        int HistoryMovesCounter;
        public static bool IsAlone;
        bool HasFolded;
        UserInfo myUser;
        PokerTableBL myTable;
        PlayerBL myPlayer;
        PlayerStates myState;
        public ServiceHandler()
        {
            if (Tables == null)
            {
                Tables = new List<PokerTableBL>();
            }
            if (LoggedInUsers == null)
            {
                LoggedInUsers = new List<UserInfo>();
            }
            if (PlayerIds == null)
            {
                PlayerIds = new List<int>();
            }
            if (WinnerIds == null)
            {
                WinnerIds = new List<int>();
            }

        }
        public int Login(string username, string password)
        {
            myUser = new UserInfo
            {
                Username = username,
                Password = password
            };
            if (!LoggedInUsers.Contains(myUser))
            {
                myUser = Database.GetUserByCredentials(myUser);
                LoggedInUsers.Add(myUser);
                myState = PlayerStates.InMenu;
                return myUser.Id;
            }
            throw new Exception("User already Logged in");
        }

        public int Signup(string username, string password)
        {
            myUser = new UserInfo
            {
                Username = username,
                Password = password,
                Wallet = 1000000000
            };
            Database.InsertUser(myUser);
            LoggedInUsers.Add(myUser);
            myState = PlayerStates.InMenu;
            return myUser.Id;
        }

        public bool Logout()
        {
            if (myUser != null)
            {
                UserCheck();
                if (myState == PlayerStates.InGame)
                {
                    if (myTable.Players[myTable.CurrTurn].PlayerId == myPlayer.PlayerId)
                    {
                        SwitchTurns();
                    }
                    myPlayer.IsPlayingThisGame = false;
                    LeaveGame();
                }
                else if (myState == PlayerStates.InTable)
                {
                    LeaveTable();
                }
                LoggedInUsers.Remove(myUser);
            }
            return true;
        }

        public PokerTableBL CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            UserCheck();
            myTable = new PokerTableBL(PokerTableName, NumOfPlayers, MinBetAmount);
            myPlayer = new PlayerBL(myUser);
            myTable.Players.Add(myPlayer);
            Tables.Add(myTable);
            myState = PlayerStates.InTable;
            myPlayer.IsPlayingThisGame = true;
            return myTable;
        }

        public int GetWallet()
        {
            UserCheck();
            return myUser.Wallet;
        }

        public List<PlayerBL> GetPlayers()
        {
            if (myTable != null)
            {
                return myTable.Players;
            }
            else
            {
                throw new Exception("The requested table is closed");
            }
        }
        public PokerTableBL JoinTable(int TableId)
        {
            UserCheck();
            foreach (PokerTableBL table in Tables)
            {
                if (table.Id == TableId)
                {
                    if (table.NumOfPlayers != table.Players.Count)
                    {
                        if (table.MinBet * 20 < myUser.Wallet)
                        {
                            myTable = table;
                            myPlayer = new PlayerBL(myUser);
                            table.Players.Add(myPlayer);
                            myState = PlayerStates.InTable;
                            myPlayer.IsPlayingThisGame = true;
                            return table;
                        }
                        throw new Exception("Inorder to join this table you need atleast: " + table.MinBet.ToString() + "$");

                    }
                    throw new Exception("The table is full");
                }
            }
            throw new Exception("An error occured while joining this table");
        }
        public void LeaveGame()
        {
            UserCheck();
            ExecuteMove(Operation.Fold, 0);
            LeaveTable();
        }
        public bool LeaveTable()
        {
            UserCheck();
            myPlayer.IsPlayingThisGame = false;
            if (myTable != null)
            {
                if (myTable.Players.Count == 1)
                {
                    //Closing Table:
                    Tables.Remove(myTable);
                }
                myTable.Players.Remove(myPlayer);
                myState = PlayerStates.InMenu;
                myTable = null;
            }
            return true;
        }
        public TableStatus GetTableStatus()
        {
            bool IsMyTurn;
            UserCheck();
            if (myTable != null)
            {
                if (!HasFolded)
                {
                    if (IsAlone)
                    {
                        myTable.TableStatus.WinnerIds.Add(myPlayer.PlayerId);
                        IsMyTurn = true;
                        UpdateOneWinnerInDB();
                        UpdateHistoryInDB();
                    }
                    else
                    {
                        IsMyTurn = myTable.Players[myTable.CurrTurn].PlayerId == myPlayer.PlayerId;
                    }
                }
                else
                {
                    IsMyTurn = false;
                }
                if (HistoryMoves == null)
                {
                    Move lastMove = myTable.Moves[myTable.Moves.Count - 1];
                    myTable.TableStatus.LastMove = lastMove;
                    myTable.TableStatus.IsMyTurn = IsMyTurn;
                }
                else
                {
                    if (HistoryMovesCounter < HistoryMoves.Count + 1)
                    {
                        myTable.TableStatus.LastMove = HistoryMoves[HistoryMovesCounter - 1];
                    }
                    else
                    {
                        myTable.TableStatus.HasGameFinished = true;
                    }
                }

            }
            else
            {
                return new TableStatus()
                {
                    IsMyTurn = false,
                    HasGameFinished = true,
                    LastMove = null,
                    WinnerIds = null
                };
            }
            return myTable.TableStatus;
        }

        public List<PokerTableBL> GetExistingTables()
        {
            UserCheck();
            return Tables;
        }
        public List<PokerTable> GetHistoryTables()
        {
            UserCheck();
            return Database.GetAllTables(myUser.Id);
        }
        public bool ExecuteMove(Operation op, int bidAmount)
        {
            int RaiseAmount;
            UserCheck();
            InitBlindsMoney();
            HasRoundEnded();
            if (!myTable.TableStatus.HasGameFinished)
            {
                if (myPlayer.CurrWallet < bidAmount)
                {
                    throw new Exception("You don't have enough money to raise");
                }
                if (myTable.LastBid == 0 && HistoryMoves == null)
                {
                    myTable.LastBid = myTable.Moves[myTable.Moves.Count - 1].BidAmount;
                }
                else if (HistoryMoves != null)
                {
                    myTable.LastBid = HistoryMoves[HistoryMovesCounter].BidAmount;
                }
                if (myTable.LastBid == myPlayer.ChipsOnTable || op == Operation.Check)
                {
                    switch (op)
                    {
                        case Operation.Raise:
                            RaiseAmount = bidAmount;
                            myPlayer.ChipsOnTable += RaiseAmount;
                            myTable.TablePot += RaiseAmount;
                            break;
                        case Operation.AllIn:
                            myTable.TablePot += myTable.MinBet * 20 - myPlayer.ChipsOnTable;
                            myPlayer.ChipsOnTable = myTable.MinBet * 20;
                            break;
                        case Operation.Check:
                            myPlayer.ChipsOnTable = bidAmount;
                            break;
                    }
                }
                else
                {
                    switch (op)
                    {
                        case Operation.Call:
                            if (myTable.LastBid == 0)
                            {
                                myTable.LastBid = myTable.Moves[myTable.Moves.Count - 1].BidAmount;
                            }
                            int CallAmount = myTable.LastBid - myPlayer.ChipsOnTable;
                            myPlayer.ChipsOnTable += CallAmount;
                            myTable.TablePot += CallAmount;
                            break;
                        case Operation.Raise:
                            if (myTable.LastBid == 0)
                            {
                                myTable.LastBid = myTable.Moves[myTable.Moves.Count - 1].BidAmount;
                            }
                            if (myPlayer.ChipsOnTable + bidAmount >= myTable.LastBid)
                            {
                                myPlayer.ChipsOnTable += bidAmount;
                                myTable.TablePot += bidAmount;
                            }
                            else
                            {
                                myTable.TablePot = myTable.LastBid + bidAmount - myPlayer.ChipsOnTable;
                                myPlayer.ChipsOnTable = myTable.LastBid + bidAmount;
                            }
                            break;
                        case Operation.Fold:
                            HasFolded = true;
                            break;
                    }
                }
                if (!HasFolded)
                {
                    myTable.Moves.Add(new Move(op, myPlayer.ChipsOnTable, myUser.Id, myTable.Id));
                    myPlayer.CurrWallet -= myPlayer.ChipsOnTable;
                    if (HistoryMoves == null)
                    {
                        myTable.TableStatus.LastMove = myTable.Moves[myTable.Moves.Count - 1];
                    }
                    else
                    {
                        myTable.TableStatus.LastMove = HistoryMoves[HistoryMovesCounter];
                    }
                    myTable.LastBid = myPlayer.ChipsOnTable;
                    SwitchTurns();
                    if (op == Operation.Raise || op == Operation.AllIn)
                    {
                        myTable.FirstPlayerId = myPlayer.PlayerId;
                    }
                }
                else
                {
                    myTable.Moves.Add(new Move(op, 0, myUser.Id, myTable.Id));
                    if (myPlayer.PlayerId == myTable.FirstPlayerId)
                    {
                        myTable.FirstPlayerId = myTable.TableStatus.LastMove.PlayerId;
                    }
                    myPlayer.IsPlayingThisGame = false;
                    UpdateFoldingUserWalletInDB();
                    myTable.Players.Remove(myPlayer);
                    myTable.NumOfPlayers--;
                    SwitchTurns();
                    if (myTable.NumOfPlayers == 1)
                    {
                        EndGame();
                    }
                }
            }
            return true;
        }

        public StartGameStatus StartGame()
        {
            myState = PlayerStates.InGame;
            StartGameStatus startGameStatus = new StartGameStatus();
            UserCheck();
            if (HistoryMoves == null)
            {
                Move smallBlind, bigBlind;
                myPlayer = new PlayerBL(myUser);
                myTable = myPlayer.GeneratePersonalCards(myTable);
                UpdatePersonalCardsInStaticList();
                myTable.GenerateDealerIndex();
                if (myTable.Moves.Count < 2)
                {
                    if (myTable.NumOfPlayers == 2)
                    {
                        if (myTable.DealerIndex == 0)
                        {
                            smallBlind = new Move(Operation.Raise, myTable.MinBet / 2, myTable.Players[1].PlayerId, myTable.Id);
                            myTable.CurrTurn = 1;
                            myTable.Players[myTable.CurrTurn].ChipsOnTable = myTable.MinBet / 2;
                            myTable.Moves.Add(smallBlind);
                            myTable.CurrTurn = 0;
                        }
                        else
                        {
                            smallBlind = new Move(Operation.Raise, myTable.MinBet / 2, myTable.Players[0].PlayerId, myTable.Id);
                            myTable.CurrTurn = 0;
                            myTable.Players[myTable.CurrTurn].ChipsOnTable = myTable.MinBet / 2;
                            myTable.Moves.Add(smallBlind);
                            myTable.CurrTurn = 1;
                        }
                        bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.DealerId, myTable.Id);
                        myTable.Moves.Add(bigBlind);
                        myTable.Players[myTable.CurrTurn].ChipsOnTable = myTable.MinBet;
                        myTable.CurrRound = Round.PreFlop;
                        myTable.FirstPlayerId = myPlayer.PlayerId;
                        HasRoundEnded();
                        SwitchTurns();
                    }
                    else if (myTable.DealerIndex == myTable.NumOfPlayers - 1)
                    {
                        smallBlind = new Move(Operation.Raise, myTable.MinBet / 2, myTable.Players[0].PlayerId, myTable.Id);
                        myTable.CurrTurn = 0;
                        myTable.Players[myTable.CurrTurn].ChipsOnTable = myTable.MinBet / 2;
                        myTable.CurrTurn = 1;
                        myTable.Moves.Add(smallBlind);
                        bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[1].PlayerId, myTable.Id);
                        myTable.Players[myTable.CurrTurn].ChipsOnTable = myTable.MinBet;
                        myTable.Moves.Add(bigBlind);
                        HasRoundEnded();
                        SwitchTurns();
                    }
                    else if (myTable.DealerIndex == myTable.NumOfPlayers - 2)
                    {
                        smallBlind = new Move(Operation.Raise, myTable.MinBet / 2, myTable.Players[myTable.NumOfPlayers - 1].PlayerId, myTable.Id);
                        myTable.CurrTurn = myTable.NumOfPlayers - 1;
                        myTable.Players[myTable.CurrTurn].ChipsOnTable = myTable.MinBet / 2;
                        myTable.CurrTurn = 0;
                        myTable.Moves.Add(smallBlind);
                        bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[0].PlayerId, myTable.Id);
                        myTable.Moves.Add(bigBlind);
                        HasRoundEnded();
                        SwitchTurns();
                    }
                    else
                    {
                        smallBlind = new Move(Operation.Raise, myTable.MinBet / 2, myTable.Players[myTable.DealerIndex + 1].PlayerId, myTable.Id);
                        myTable.CurrTurn = myTable.DealerIndex + 1;
                        myTable.Players[myTable.CurrTurn].ChipsOnTable = myTable.MinBet / 2;
                        myTable.CurrTurn = myTable.DealerIndex + 2;
                        myTable.Moves.Add(smallBlind);
                        bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[myTable.DealerIndex + 2].PlayerId, myTable.Id);
                        myTable.Players[myTable.CurrTurn].ChipsOnTable = myTable.MinBet;
                        myTable.Moves.Add(bigBlind);
                        HasRoundEnded();
                        SwitchTurns();
                    }
                    startGameStatus.SmallBlind = smallBlind;
                    startGameStatus.BigBlind = bigBlind;
                    myTable.LastBid = bigBlind.BidAmount;
                }
                myTable.TablePot = Convert.ToInt32(myTable.MinBet * 1.5);
                startGameStatus.SmallBlind = myTable.Moves[0];
                startGameStatus.BigBlind = myTable.Moves[1];
                startGameStatus.PlayerCard1 = myPlayer.PersonalCards[0];
                startGameStatus.PlayerCard2 = myPlayer.PersonalCards[1];
                startGameStatus.DealerId = myTable.DealerId;
                startGameStatus.PlayerId = myPlayer.PlayerId;
                foreach (PlayerBL player in myTable.Players)
                {
                    if (player.PlayerId == startGameStatus.SmallBlind.PlayerId)
                    {
                        player.CurrWallet -= myTable.MinBet / 2;
                        if (myPlayer.PlayerId == player.PlayerId)
                        {
                            myPlayer.ChipsOnTable = myTable.MinBet / 2;
                        }
                    }
                    if (player.PlayerId == startGameStatus.BigBlind.PlayerId)
                    {
                        player.CurrWallet -= myTable.MinBet;
                        if (myPlayer.PlayerId == player.PlayerId)
                        {
                            myPlayer.ChipsOnTable = myTable.MinBet;
                        }
                    }
                }
            }
            else
            {
                int PlayerIndex = -1;
                startGameStatus.SmallBlind = HistoryMoves[0];
                startGameStatus.BigBlind = HistoryMoves[1];
                startGameStatus.PlayerId = myPlayer.PlayerId;
                for (int i = 0; i < myTable.Players.Count; i++)
                {
                    if (myTable.Players[i].PlayerId == myUser.Id)
                    {
                        PlayerIndex = i;
                    }
                }
                startGameStatus.PlayerCard1 = myTable.Players[PlayerIndex].PersonalCards[0];
                startGameStatus.PlayerCard2 = myTable.Players[PlayerIndex].PersonalCards[1];
            }
            return startGameStatus;
        }

        public void SwitchTurns()
        {
            UserCheck();
            UpdateNumOfPlayers();
            if (myTable.NumOfPlayers == 1)
            {
                myTable.CurrRound = Round.Showdown;
                myTable.CurrRound = 0;
            }
            else if (myTable.CurrTurn == myTable.NumOfPlayers - 1)
            {
                myTable.CurrTurn = 0;
            }
            else
            {
                myTable.CurrTurn++;
            }
        }
        public void HasRoundEnded()
        {
            UserCheck();
            if (HistoryMoves == null)
            {
                if (myTable.FirstPlayerId == myPlayer.PlayerId)
                {
                    myTable.CurrRound++;
                    switch (myTable.CurrRound)
                    {
                        case Round.Flop:
                            myTable.TableStatus.TableCards = myTable.TableDeck.GetCards(3);
                            break;
                        case Round.Turn:
                            myTable.TableStatus.TableCards.Add(myTable.TableDeck.GetCard());
                            break;
                        case Round.River:
                            myTable.TableStatus.TableCards.Add(myTable.TableDeck.GetCard());
                            break;
                        case Round.Showdown:
                            EndGame();
                            break;
                    }
                    UpdateTableCardsInStaticList();
                }
            }
        }
        public void EndGame()
        {
            if (myTable.CurrRound == Round.Showdown || myTable.NumOfPlayers == 1)
            {
                myTable.TableStatus.HasGameFinished = true;
                if (myTable.NumOfPlayers == 1)
                {
                    IsAlone = true;

                }
                else if (HistoryMoves == null)
                {
                    DetermineWinner();
                }
            }
        }
        public void DetermineWinner()
        {
            UserCheck();
            if (myTable.TableStatus.WinnerIds.Count == 0)
            {
                HandEvaluator handEvaluator = new HandEvaluator()
                {
                    CardsOnTable = Deck.ToStrArrList(myTable.TableStatus.TableCards)
                };
                switch (myTable.Players.Count)
                {
                    case 1:
                        myTable.TableStatus.WinnerIds.Add(myPlayer.PlayerId);
                        break;
                    case 2:
                        handEvaluator.Chair0 = myTable.Players[0];
                        handEvaluator.Chair1 = myTable.Players[1];
                        handEvaluator.PotOfChair0 = myTable.Players[0].ChipsOnTable;
                        handEvaluator.PotOfChair1 = myTable.Players[1].ChipsOnTable;
                        break;
                    case 3:
                        handEvaluator.Chair0 = myTable.Players[0];
                        handEvaluator.Chair1 = myTable.Players[1];
                        handEvaluator.Chair2 = myTable.Players[2];
                        handEvaluator.PotOfChair0 = myTable.Players[0].ChipsOnTable;
                        handEvaluator.PotOfChair1 = myTable.Players[1].ChipsOnTable;
                        handEvaluator.PotOfChair2 = myTable.Players[2].ChipsOnTable;
                        break;
                    case 4:
                        handEvaluator.Chair0 = myTable.Players[0];
                        handEvaluator.Chair1 = myTable.Players[1];
                        handEvaluator.Chair2 = myTable.Players[2];
                        handEvaluator.Chair3 = myTable.Players[3];
                        handEvaluator.PotOfChair0 = myTable.Players[0].ChipsOnTable;
                        handEvaluator.PotOfChair1 = myTable.Players[1].ChipsOnTable;
                        handEvaluator.PotOfChair2 = myTable.Players[2].ChipsOnTable;
                        handEvaluator.PotOfChair3 = myTable.Players[3].ChipsOnTable;
                        break;
                    case 5:
                        handEvaluator.Chair0 = myTable.Players[0];
                        handEvaluator.Chair1 = myTable.Players[1];
                        handEvaluator.Chair2 = myTable.Players[2];
                        handEvaluator.Chair3 = myTable.Players[3];
                        handEvaluator.Chair4 = myTable.Players[4];
                        handEvaluator.PotOfChair0 = myTable.Players[0].ChipsOnTable;
                        handEvaluator.PotOfChair1 = myTable.Players[1].ChipsOnTable;
                        handEvaluator.PotOfChair2 = myTable.Players[2].ChipsOnTable;
                        handEvaluator.PotOfChair3 = myTable.Players[3].ChipsOnTable;
                        handEvaluator.PotOfChair4 = myTable.Players[4].ChipsOnTable;
                        break;
                }
                var result = handEvaluator.SpreadMoneyToWinners();
                List<SidePot> sidePots = result.Item1;
                List<List<HandRank>> totalWinners = result.Item2;
                if (sidePots[0].Winners.Count > 1)
                {
                    for (int i = 0; i < sidePots[0].Winners.Count - 1; i++)
                    {
                        myTable.TableStatus.WinnerIds.Add(totalWinners[i][0].User.PlayerId);
                    }
                }
                else
                {
                    if (totalWinners.Count > 0)
                    {
                        myTable.TableStatus.WinnerIds.Add(totalWinners[0][0].User.PlayerId);
                    }
                }
                UpdateUsersWalletInDB(totalWinners);
                UpdateHistoryInDB();
            }
        }
        public void UpdateNumOfPlayers()
        {
            int NumOfPlayers = 0;
            foreach (UserInfo user in LoggedInUsers)
            {
                for (int i = 0; i < myTable.Players.Count; i++)
                {
                    if (user.Id == myTable.Players[i].PlayerId)
                    {
                        NumOfPlayers++;
                    }
                }
            }
            myTable.NumOfPlayers = NumOfPlayers;
        }
        public bool HasGameStarted()
        {
            if (myTable == null)
            {
                return false; //Just finished a game.
            }
            return myTable.Players.Count == myTable.NumOfPlayers;
        }
        public void InitBlindsMoney()
        {
            if (myTable.CurrRound == Round.PreFlop)
            {
                foreach (PlayerBL player in myTable.Players)
                {
                    if (player.PlayerId == myUser.Id)
                    {
                        myPlayer.ChipsOnTable = player.ChipsOnTable;
                    }
                }
            }
        }
        private void UserCheck()
        {
            if (!LoggedInUsers.Contains(myUser))
            {
                throw new Exception("You are not logged in yet, Please restart the program");
            }
        }
        public void UpdateUsersWalletInDB(List<List<HandRank>> totalWinners)
        {
            for (int i = 0; i < totalWinners.Count; i++)
            {
                myUser.Wallet = totalWinners[i][0].User.CurrWallet;
                Database.UpdateUser(myUser);
            }
        }
        public void UpdateOneWinnerInDB()
        {
            if (IsAlone)
            {
                myUser.Wallet = myPlayer.CurrWallet;
                Database.UpdateUser(myUser);
            }
        }
        public void UpdateFoldingUserWalletInDB()
        {
            PokerTableBL tableBL = myTable;
            //Updating table from static List
            foreach (PokerTableBL pokerTable in Tables)
            {
                if (pokerTable.Id == myTable.Id)
                {
                    tableBL = pokerTable;
                }
            }
            PokerTable table = new PokerTable()
            {
                PokerTableName = tableBL.PokerTableName,
                TablePot = tableBL.TablePot,
                MinBet = tableBL.MinBet,
                DealerId = tableBL.DealerId
            };
            switch (tableBL.TableStatus.TableCards.Count)
            {
                case 1:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = "";
                    table.ThirdCard = "";
                    table.FourthCard = "";
                    table.FifthCard = "";
                    break;
                case 2:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = tableBL.TableStatus.TableCards[1].ToString();
                    table.ThirdCard = "";
                    table.FourthCard = "";
                    table.FifthCard = "";
                    break;
                case 3:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = tableBL.TableStatus.TableCards[1].ToString();
                    table.ThirdCard = tableBL.TableStatus.TableCards[2].ToString();
                    table.FourthCard = "";
                    table.FifthCard = "";
                    break;
                case 4:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = tableBL.TableStatus.TableCards[1].ToString();
                    table.ThirdCard = tableBL.TableStatus.TableCards[2].ToString();
                    table.FourthCard = tableBL.TableStatus.TableCards[3].ToString();
                    table.FifthCard = "";
                    break;
                case 5:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = tableBL.TableStatus.TableCards[1].ToString();
                    table.ThirdCard = tableBL.TableStatus.TableCards[2].ToString();
                    table.FourthCard = tableBL.TableStatus.TableCards[3].ToString();
                    table.FifthCard = tableBL.TableStatus.TableCards[4].ToString();
                    break;
            }
            myTable.Id = Database.InsertTable(table);
            myUser.Wallet -= myPlayer.ChipsOnTable;
            Database.UpdateUser(myUser);
            Player player = new Player()
            {
                PokerTableId = myTable.Id,
                PlayerId = myPlayer.PlayerId,
                PlayerName = myPlayer.PlayerName,
                ChipsOnTable = myPlayer.ChipsOnTable,
                FirstCard = myPlayer.PersonalCards[0].ToString(),
                SecondCard = myPlayer.PersonalCards[1].ToString()
            };
            PlayerIds.Add(Database.InsertPlayer(player));
        }
        public void UpdateHistoryInDB()
        {
            PokerTableBL tableBL = myTable;
            //Updating table from static List
            foreach (PokerTableBL pokerTable in Tables)
            {
                if (pokerTable.Id == myTable.Id)
                {
                    tableBL = pokerTable;
                }
            }
            PokerTable table = new PokerTable()
            {
                PokerTableName = tableBL.PokerTableName,
                TablePot = tableBL.TablePot,
                MinBet = tableBL.MinBet,
                DealerId = tableBL.DealerId
            };
            switch (tableBL.TableStatus.TableCards.Count)
            {
                case 1:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = "";
                    table.ThirdCard = "";
                    table.FourthCard = "";
                    table.FifthCard = "";
                    break;
                case 2:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = tableBL.TableStatus.TableCards[1].ToString();
                    table.ThirdCard = "";
                    table.FourthCard = "";
                    table.FifthCard = "";
                    break;
                case 3:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = tableBL.TableStatus.TableCards[1].ToString();
                    table.ThirdCard = tableBL.TableStatus.TableCards[2].ToString();
                    table.FourthCard = "";
                    table.FifthCard = "";
                    break;
                case 4:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = tableBL.TableStatus.TableCards[1].ToString();
                    table.ThirdCard = tableBL.TableStatus.TableCards[2].ToString();
                    table.FourthCard = tableBL.TableStatus.TableCards[3].ToString();
                    table.FifthCard = "";
                    break;
                case 5:
                    table.FirstCard = tableBL.TableStatus.TableCards[0].ToString();
                    table.SecondCard = tableBL.TableStatus.TableCards[1].ToString();
                    table.ThirdCard = tableBL.TableStatus.TableCards[2].ToString();
                    table.FourthCard = tableBL.TableStatus.TableCards[3].ToString();
                    table.FifthCard = tableBL.TableStatus.TableCards[4].ToString();
                    break;
            }
            myTable.Id = Database.InsertTable(table);
            foreach (PokerTableBL mytable in Tables)
            {
                if (mytable.Id == myTable.Id)
                {
                    for (int i = 0; i < mytable.Players.Count; i++)
                    {
                        Player player = new Player()
                        {
                            PokerTableId = mytable.Id,
                            PlayerId = mytable.Players[i].PlayerId,
                            PlayerName = mytable.Players[i].PlayerName,
                            ChipsOnTable = mytable.Players[i].ChipsOnTable,
                            FirstCard = mytable.Players[i].PersonalCards[0].ToString(),
                            SecondCard = mytable.Players[i].PersonalCards[1].ToString()
                        };
                        PlayerIds.Add(Database.InsertPlayer(player));
                    }
                }
            }
            switch (PlayerIds.Count)
            {
                case 1:
                    table.Player1Id = PlayerIds[0];
                    break;
                case 2:
                    table.Player1Id = PlayerIds[0];
                    table.Player2Id = PlayerIds[1];
                    break;
                case 3:
                    table.Player1Id = PlayerIds[0];
                    table.Player2Id = PlayerIds[1];
                    table.Player3Id = PlayerIds[2];
                    break;
                case 4:
                    table.Player1Id = PlayerIds[0];
                    table.Player2Id = PlayerIds[1];
                    table.Player3Id = PlayerIds[2];
                    table.Player4Id = PlayerIds[3];
                    break;
                case 5:
                    table.Player1Id = PlayerIds[0];
                    table.Player2Id = PlayerIds[1];
                    table.Player3Id = PlayerIds[2];
                    table.Player4Id = PlayerIds[3];
                    table.Player5Id = PlayerIds[4];
                    break;
            }
            Database.UpdateTable(table);
            foreach (Move move in tableBL.Moves)
            {
                move.PokerTableId = myTable.Id;
                Database.InsertMove(move);
            }
        }
        public void UpdatePersonalCardsInStaticList()
        {
            foreach (PokerTableBL table in Tables)
            {
                if (myTable.Id == table.Id)
                {
                    for (int i = 0; i < table.Players.Count; i++)
                    {
                        if (myPlayer.PlayerId == table.Players[i].PlayerId)
                        {
                            table.Players[i].PersonalCards = myTable.Players[i].PersonalCards;
                        }
                    }
                }
            }
        }
        public void UpdateTableCardsInStaticList()
        {
            foreach (PokerTableBL table in Tables)
            {
                if (myTable.Id == table.Id)
                {
                    table.TableStatus.TableCards = myTable.TableStatus.TableCards;
                }
            }
        }
        public bool InitHistoryMoves(int TableId)
        {
            HistoryMoves = Database.GetAllMoves(TableId);
            HistoryMovesCounter = 3;
            return true;
        }
        public bool ExecuteNextMoveByHistory()
        {
            if (HistoryMovesCounter == HistoryMoves.Count)
            {
                myTable.TableStatus.HasGameFinished = true;
            }
            if (HistoryMoves[HistoryMovesCounter - 1].PlayerId == myPlayer.PlayerId)
            {
                ExecuteMove(HistoryMoves[HistoryMovesCounter - 1].Operation, HistoryMoves[HistoryMovesCounter - 1].BidAmount);
                HistoryMovesCounter++;
            }
            else
            {
                myTable.TableStatus.LastMove = HistoryMoves[HistoryMovesCounter - 1];
                myTable.TableStatus.LastMove.PlayerId = HistoryMoves[HistoryMovesCounter - 1].PlayerId;
                HistoryMovesCounter++;
            }
            return true;
        }
        public PokerTableBL GetTableByHistory(PokerTable pokerTable)
        {
            myTable = new PokerTableBL(pokerTable.PokerTableName, 0, pokerTable.MinBet);
            if (pokerTable.Player1Id != -1)
            {
                myTable.Players.Add(new PlayerBL(Database.GetUserById(pokerTable.Player1Id), pokerTable.Id));
            }
            if (pokerTable.Player2Id != -1)
            {
                myTable.Players.Add(new PlayerBL(Database.GetUserById(pokerTable.Player2Id), pokerTable.Id));
            }
            if (pokerTable.Player3Id != -1)
            {
                myTable.Players.Add(new PlayerBL(Database.GetUserById(pokerTable.Player3Id), pokerTable.Id));
            }
            if (pokerTable.Player4Id != -1)
            {
                myTable.Players.Add(new PlayerBL(Database.GetUserById(pokerTable.Player4Id), pokerTable.Id));
            }
            if (pokerTable.Player5Id != -1)
            {
                myTable.Players.Add(new PlayerBL(Database.GetUserById(pokerTable.Player5Id), pokerTable.Id));
            }
            if (pokerTable.FirstCard != "")
            {
                myTable.TableStatus.TableCards.Add(new Card(pokerTable.FirstCard));
            }
            if (pokerTable.SecondCard != "")
            {
                myTable.TableStatus.TableCards.Add(new Card(pokerTable.SecondCard));
            }
            if (pokerTable.ThirdCard != "")
            {
                myTable.TableStatus.TableCards.Add(new Card(pokerTable.ThirdCard));
            }
            if (pokerTable.FourthCard != "")
            {
                myTable.TableStatus.TableCards.Add(new Card(pokerTable.FourthCard));
            }
            if (pokerTable.FifthCard != "")
            {
                myTable.TableStatus.TableCards.Add(new Card(pokerTable.FifthCard));
            }
            myTable.NumOfPlayers = myTable.Players.Count;
            myTable.DealerId = pokerTable.DealerId;
            for (int i = 0; i < myTable.Players.Count; i++)
            {
                if (myTable.DealerId == myTable.Players[i].PlayerId)
                {
                    myTable.DealerIndex = i;
                }
            }
            foreach (PlayerBL player in myTable.Players)
            {
                if (player.PlayerId == myUser.Id)
                {
                    myPlayer = player;
                }
            }
            return myTable;
        }
    }
}

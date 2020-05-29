using PokerBL.ORM;
using PokerDL.Models;
using PokerHandEvaluator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerHandEvaluator.HandEvaluator;

namespace PokerBL.Models
{
    public class ServiceHandler
    {
        public static List<UserInfo> LoggedInUsers;
        public static List<PokerTableBL> Tables;
        UserInfo myUser;
        PokerTableBL myTable;
        PlayerBL myPlayer;
        TableStatus tableStatus;
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

        }
        public int Login(string username, string password)
        {
            myUser = new UserInfo
            {
                Username = username,
                Password = password
            };
            myUser = Database.GetUserByCredentials(myUser);
            LoggedInUsers.Add(myUser);
            return myUser.Id;
        }

        public int Signup(string username, string password)
        {
            myUser = new UserInfo
            {
                Username = username,
                Password = password,
                Wallet = 1000000
            };
            Database.InsertUser(myUser);
            LoggedInUsers.Add(myUser);
            return myUser.Id;
        }

        public bool Logout()
        {
            UserCheck();
            LoggedInUsers.Remove(myUser);
            return true;
        }

        public bool CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            UserCheck();
            myTable = new PokerTableBL(PokerTableName, NumOfPlayers, MinBetAmount);
            myPlayer = new PlayerBL(myTable.Id, myUser);
            myTable.Players.Add(myPlayer);
            Tables.Add(myTable);
            return true;
        }

        public string GetCurrPlayerNames()
        {
            string PlayerNames = "";
            UserCheck();
            foreach (PlayerBL player in myTable.Players)
            {
                PlayerNames += player.PlayerName;
            }
            return PlayerNames;
        }

        public bool JoinTable(int TableId)
        {
            UserCheck();
            foreach (PokerTableBL table in Tables)
            {
                if (table.Id == TableId)
                {
                    myTable = table;
                    myPlayer = new PlayerBL(myTable.Id, myUser);
                    table.Players.Add(myPlayer);
                    return true;
                }
            }
            throw new Exception("An error occured while joining this table");
        }
        public bool LeaveTable()
        {
            UserCheck();
            myTable.Players.Remove(myPlayer);
            return true;
        }

        public TableStatus GetTableStatus()
        {
            bool IsMyTurn;
            Move lastMove;
            UserCheck();
            IsMyTurn = myTable.Players[myTable.CurrTurn].Id == myPlayer.Id;
            lastMove = myTable.Moves[myTable.Moves.Count - 1];
            if (tableStatus == null)
            {
                tableStatus = new TableStatus();
            }
            tableStatus.LastMove = lastMove;
            tableStatus.IsMyTurn = IsMyTurn;
            HasRoundEnded();
            return tableStatus;
        }

        public List<PokerTableBL> GetExistingTables()
        {
            UserCheck();
            return Tables;
        }
        public bool ExecuteMove(Operation op, int bidAmount)
        {
            bool HasFolded = false;
            UserCheck();
            if (myTable.LastBid == myPlayer.ChipsOnTable)
            {
                switch (op)
                {
                    case Operation.Raise:
                        myPlayer.ChipsOnTable += bidAmount;
                        myTable.FirstPlayerId = myPlayer.Id;
                        myTable.TablePot += bidAmount;
                        break;
                    case Operation.AllIn:
                        myPlayer.ChipsOnTable = myTable.MinBet * 40;
                        myTable.FirstPlayerId = myPlayer.Id;
                        myTable.TablePot += myTable.MinBet * 40;
                        break;
                }
            }
            else
            {
                switch (op)
                {
                    case Operation.Call:
                        myPlayer.ChipsOnTable += bidAmount;
                        myTable.TablePot += bidAmount;
                        break;
                    case Operation.Raise:
                        myPlayer.ChipsOnTable += bidAmount;
                        myTable.FirstPlayerId = myPlayer.Id;
                        myTable.TablePot += bidAmount;
                        break;
                    case Operation.Fold:
                        HasFolded = true;
                        break;
                }
            }
            if (!HasFolded)
            {
                myTable.Moves.Add(new Move(op, bidAmount, myUser.Id, myTable.Id));
                myTable.LastBid = myPlayer.ChipsOnTable;
                SwitchTurns();
            }
            else
            {
                myTable.Players.Remove(myPlayer);
                myTable.NumOfPlayers--;
            }
            return true;
        }

        public bool StartGame()
        {
            UserCheck();
            Move smallBlind, bigBlind;
            myPlayer = new PlayerBL(myTable.Id,myUser);
            myPlayer.GeneratePersonalCards(myTable);
            myTable.GenerateDealerIndex();
            if (myTable.DealerIndex == myTable.NumOfPlayers - 1)
            {
                smallBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[0].Id, myTable.Id);
                myTable.CurrTurn = 0;
                myTable.Moves.Add(smallBlind);
                SwitchTurns();
                bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[1].Id, myTable.Id);
                myTable.Moves.Add(bigBlind);
                SwitchTurns();
            }
            else if (myTable.DealerIndex == myTable.NumOfPlayers - 2)
            {
                smallBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[myTable.NumOfPlayers - 1].Id, myTable.Id);
                myTable.CurrTurn = myTable.NumOfPlayers - 1;
                myTable.Moves.Add(smallBlind);
                SwitchTurns();
                bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[0].Id, myTable.Id);
                myTable.Moves.Add(bigBlind);
                SwitchTurns();
            }
            else
            {
                smallBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[myTable.DealerIndex + 1].Id, myTable.Id);
                myTable.CurrTurn = myTable.DealerIndex + 1;
                myTable.Moves.Add(smallBlind);
                SwitchTurns();
                bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.Players[myTable.DealerIndex + 2].Id, myTable.Id);
                myTable.Moves.Add(bigBlind);
                SwitchTurns();
            }
            myTable.CurrRound++;
            return true;
        }

        public void SwitchTurns()
        {
            UserCheck();
            if (myTable.CurrTurn == myTable.NumOfPlayers - 1)
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
            if (myTable.FirstPlayerId == myPlayer.Id)
            {
                myTable.CurrRound++;
                switch (myTable.CurrRound)
                {
                    case Round.Flop:
                        tableStatus.TableCards = myTable.TableDeck.GetCards(3);
                        break;
                    case Round.Turn:
                        tableStatus.TableCards.Add(myTable.TableDeck.GetCard());
                        break;
                    case Round.River:
                        tableStatus.TableCards.Add(myTable.TableDeck.GetCard());
                        break;
                    case Round.Showdown:
                        tableStatus.WinnerId = DetermineWinner();
                        tableStatus.HasGameFinished = true;
                        break;
                }

            }
        }
        public int DetermineWinner()
        {
            UserCheck();
            HandEvaluator handEvaluator = new HandEvaluator
            {
                CardsOnTable = Deck.ToStrArrList(tableStatus.TableCards),
                Chair0 = myTable.Players[0],
                Chair1 = myTable.Players[1],
                Chair2 = myTable.Players[2],
                Chair3 = myTable.Players[3],
                Chair4 = myTable.Players[4],
                PotOfChair0 = myTable.Players[0].ChipsOnTable,
                PotOfChair1 = myTable.Players[1].ChipsOnTable,
                PotOfChair2 = myTable.Players[2].ChipsOnTable,
                PotOfChair3 = myTable.Players[3].ChipsOnTable,
                PotOfChair4 = myTable.Players[4].ChipsOnTable
            };
            List<SidePot> sidePots = handEvaluator.SpreadMoneyToWinners();
            return sidePots[0].Winners[0].Id;
        }
        private void UserCheck()
        {
            if(!LoggedInUsers.Contains(myUser))
            {
                throw new Exception("You are not logged in yet, Please restart the program");
            }
        }


    }
}

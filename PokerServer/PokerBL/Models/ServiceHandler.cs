using PokerBL.ORM;
using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class ServiceHandler
    {
        public static List<PokerTableBL> Tables;
        UserInfo myUser;
        PokerTableBL myTable;
        PlayerBL myPlayer;

        public ServiceHandler()
        {
            if (Tables == null)
            {
                Tables = new List<PokerTableBL>();
            }
            if (myUser == null)
            {
                myUser = new UserInfo();
            }
        }
        public int Login(string username, string password)
        {
            myUser.Username = username;
            myUser.Password = password;
            myUser.Id = Database.GetUserByCredentials(myUser);
            return myUser.Id;
        }

        public int Signup(string username, string password)
        {
            myUser.Username = username;
            myUser.Password = password;
            myUser.Wallet = 1000000;
            Database.InsertUser(myUser);
            return myUser.Id;
        }

        public bool CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            UserCheck();
            myTable = new PokerTableBL(PokerTableName, NumOfPlayers, MinBetAmount, myUser.Id)
            {
                LoggedInPlayers = 1
            };
            myTable.PlayerNames.Add(myUser.Username);
            Tables.Add(myTable);
            return true;


        }

        public List<string> GetCurrPlayerNames()
        {
            UserCheck();
            return myTable.PlayerNames;
        }

        public bool JoinTable(int TableId)
        {
            UserCheck();
            foreach (PokerTableBL table in Tables)
            {
                if (table.Id == TableId)
                {
                    myTable = table;
                    table.PlayerIds.Add(myUser.Id);
                    table.PlayerNames.Add(myUser.Username);
                    table.LoggedInPlayers++;
                    return true;
                }
            }
            throw new Exception("An error occured while joining this table");
        }
        public bool LeaveTable()
        {
            UserCheck();
            foreach (PokerTableBL table in Tables)
            {
                if (table.AdminId == myUser.Id)
                {
                    CloseTable();
                }
                else
                {
                    myTable.PlayerIds.Remove(myUser.Id);
                    myTable.PlayerNames.Remove(myUser.Username);
                }
                return true;
            }
            throw new Exception("An error occured while leaving this table");
        }

        public TableStatus GetTableStatus()
        {
            UserCheck();
            bool IsMyTurn = myTable.PlayerIds[myTable.CurrTurn] == myPlayer.Id;
            Move lastMove = myTable.Moves[myTable.Moves.Count - 1];
            TableStatus tableStatus = new TableStatus(lastMove, IsMyTurn);
            return tableStatus;
        }

        public bool CloseTable()
        {
            UserCheck();
            myTable.IsClosed = true;
            Tables.Remove(myTable);
            return true;
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
                switch(op)
                {
                    case Operation.Raise:
                        myPlayer.ChipsOnTable += bidAmount;
                        break;
                }
            }
            else
            {
                switch (op)
                {
                    case Operation.Call:
                        myPlayer.ChipsOnTable += bidAmount;
                        break;
                    case Operation.Raise:
                        myPlayer.ChipsOnTable += bidAmount;
                        break;
                    case Operation.Fold:
                        HasFolded = true;
                        break;
                }
            }
            if(!HasFolded)
            {
                myTable.Moves.Add(new Move(op, bidAmount, myUser.Id, myTable.Id));
                myTable.LastBid = myPlayer.ChipsOnTable;
                SwitchTurns();
            }
            else
            {
                myTable.PlayerIds.Remove(myPlayer.Id);
                myTable.PlayerNames.Remove(myUser.Username);
                myTable.NumOfPlayers--;
            }
            return true;
        }

        public bool StartGame()
        {
            Move smallBlind, bigBlind;
            myPlayer = new PlayerBL(myUser.Wallet, myTable.Id);
            myPlayer.GeneratePersonalCards(myTable);
            myTable.GenerateDealerIndex();
            if (myTable.DealerIndex == myTable.NumOfPlayers - 1)
            {
                smallBlind = new Move(Operation.Raise, myTable.MinBet, myTable.PlayerIds[0], myTable.Id);
                myTable.CurrTurn = 0;
                myTable.Moves.Add(smallBlind);
                SwitchTurns();
                bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.PlayerIds[1], myTable.Id);
                myTable.Moves.Add(bigBlind);
                SwitchTurns();
            }
            else if (myTable.DealerIndex == myTable.NumOfPlayers - 2)
            {
                smallBlind = new Move(Operation.Raise, myTable.MinBet, myTable.PlayerIds[myTable.NumOfPlayers - 1], myTable.Id);
                myTable.CurrTurn = myTable.NumOfPlayers - 1;
                myTable.Moves.Add(smallBlind);
                SwitchTurns();
                bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.PlayerIds[0], myTable.Id);
                myTable.Moves.Add(bigBlind);
                SwitchTurns();
            }
            else
            {
                smallBlind = new Move(Operation.Raise, myTable.MinBet, myTable.PlayerIds[myTable.DealerIndex + 1], myTable.Id);
                myTable.CurrTurn = myTable.DealerIndex + 1;
                myTable.Moves.Add(smallBlind);
                SwitchTurns();
                bigBlind = new Move(Operation.Raise, myTable.MinBet, myTable.PlayerIds[myTable.DealerIndex + 2], myTable.Id);
                myTable.Moves.Add(bigBlind);
                SwitchTurns();
            }

            return true;
        }

        public void SwitchTurns()
        {
            if (myTable.CurrTurn == myTable.NumOfPlayers - 1)
            {
                myTable.CurrTurn = 0;
            }
            else
            {
                myTable.CurrTurn++;
            }
        }

        private void UserCheck()
        {
            if (myUser.Id == -1)
            {
                throw new Exception("You are not logged in yet, Please restart the program");
            }
        }


    }
}

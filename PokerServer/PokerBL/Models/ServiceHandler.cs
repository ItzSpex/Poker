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
        int userId;
        string userName;
        PokerTableBL myTable;

        public ServiceHandler()
        {
            if (Tables == null)
            {
                Tables = new List<PokerTableBL>();
            }
        }
        public int Login(string username, string password)
        {
            UserInfo user = new UserInfo();
            user.Username = username;
            user.Password = password;
            userId = Database.GetUserByCredentials(user);
            userName = username;
            return userId;
        }

        public int Signup(string username, string password)
        {
            UserInfo user = new UserInfo();
            user.Username = username;
            user.Password = password;
            user.Wallet = 1000000;
            Database.InsertUser(user);
            userId = user.Id;
            userName = username;
            return userId;
        }

        public bool CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            UserCheck();
            myTable = new PokerTableBL(PokerTableName, NumOfPlayers, MinBetAmount, userId);
            myTable.LoggedInPlayers = 1;
            myTable.PlayerNames.Add(userName);
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
                    table.PlayerIds.Add(userId);
                    table.PlayerNames.Add(userName);
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
                if (table.AdminId == userId)
                {
                    CloseTable();
                }
                else
                {
                    myTable.PlayerIds.Remove(userId);
                    myTable.PlayerNames.Remove(userName);
                }
                return true;
            }
            throw new Exception("An error occured while leaving this table");
        }
        public bool CloseTable()
        {
            UserCheck();
            myTable.IsClosed = true;
            Tables.Remove(myTable);
            return true;
            throw new Exception("An error occured while closing this table");
        }
        public List<PokerTableBL> GetExistingTables()
        {
            UserCheck();
            return Tables;
        }
        public TableStatus GetTableStatus()
        {
            /*if (myTable.IsClosed)
            {
                myTable.loggedUsers--;
                if (myTable.loggedUsers == 0)
                {
                    Tables.Remove(myTable);
                }
            }*/
            throw new NotImplementedException();
        }
        private void UserCheck()
        {
            if (userId == -1)
            {
                throw new Exception("You are not logged in yet, Please restart the program");
            }
        }

    }
}

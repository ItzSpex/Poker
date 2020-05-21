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
            return userId;
        }

        public bool CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            UserCheck();
            myTable = new PokerTableBL(PokerTableName, NumOfPlayers, MinBetAmount, userId);
            myTable.LoggedInPlayers = 1;
            Tables.Add(myTable);
            return true;


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

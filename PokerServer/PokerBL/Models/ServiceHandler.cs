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
        PokerTableBL currTable;

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
            currTable = new PokerTableBL(PokerTableName, NumOfPlayers, MinBetAmount, userId);
            Tables.Add(currTable);
            return true;


        }
        public bool JoinTable(int TableId)
        {
            UserCheck();
            foreach (PokerTableBL table in Tables)
            {
                if (table.Id == TableId)
                    table.PlayerIds.Add(userId);
                    return true;
            }
            throw new Exception("An error occured while joining this table");
        }
        public bool LeaveTable(int TableId)
        {
            UserCheck();
            foreach (PokerTableBL table in Tables)
            {
                if (table.AdminId == userId && table.Id == TableId)
                    CloseTable(TableId);
                else if (table.Id == TableId)
                    table.PlayerIds.Remove(userId);
                return true;
            }
            throw new Exception("An error occured while leaving this table");
        }
        public bool CloseTable(int TableId)
        {
            UserCheck();
            foreach (PokerTableBL table in Tables)
            {
                if(table.AdminId == userId && table.Id == TableId)
                {
                    //TODO: Return all of the users to main menu.
                    Tables.Remove(table);
                    return true;
                }
            }
            throw new Exception("An error occured while closing this table");
        }
        public List<PokerTableBL> GetExistingTables()
        {
            UserCheck();
            return Tables;
        }

        private void UserCheck()
        {
            if (userId == -1)
                throw new Exception("You are not logged in yet, Please restart the program");
        }
    }
}

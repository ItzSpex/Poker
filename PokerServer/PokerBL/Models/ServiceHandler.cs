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
        bool IsAdmin;

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
            currTable = new PokerTableBL(PokerTableName, NumOfPlayers, MinBetAmount);
            Tables.Add(currTable);
            IsAdmin = true;
            return true;


        }
        public bool JoinTable()
        {
            UserCheck();
            return true;
        }
        public bool LeaveTable()
        {
            UserCheck();
            return true;
        }
        public bool CloseTable()
        {
            UserCheck();
            return true;
        }
        public List<PokerTableBL> GetExistingTables()
        {
            UserCheck();
            return Tables;
        }

        private void UserCheck()
        {
            if (userId == -1)
                throw new Exception("User not logged in");
        }
    }
}

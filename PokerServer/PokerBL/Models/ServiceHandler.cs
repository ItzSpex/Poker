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
        
        public ServiceHandler()
        {
            Tables = new List<PokerTableBL>();
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
            PokerTableBL pokerTableBL = new PokerTableBL(PokerTableName, NumOfPlayers, MinBetAmount);
            Tables.Add(pokerTableBL);
            return true;
        }
        
        public List<PokerTableBL> GetExistingTables ()
        {
            return Tables;
        }
    }
}

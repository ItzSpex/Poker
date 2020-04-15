using PokerBL.Models;
using PokerDL.Models;
using PokerDL.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.ORM
{
    public class Database
    {
        private static List<UserInfo> users = null;
        public static int GetUserByCredentials(UserInfo requstedUser)
        {
            UserInfo u = null;
            if (users == null)
            {
                UserInfoDB userInfoDB = new UserInfoDB();
                u = userInfoDB.GetByUsername(requstedUser.Username);
            }
            else
            {
                u = users.FirstOrDefault(item => item == requstedUser);
            }
            if (u == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (u.Password != requstedUser.Password)
            {
                throw new Exception("Wrong password");
            }
            return u.Id;
        }
        public static void InsertUser(UserInfo newUser)
        {
            UserInfoDB userInfoDB = new UserInfoDB();
            UserInfo u = userInfoDB.GetByUsername(newUser.Username);
            if (u != null)
            {
                throw new Exception("User exists");
            }
            userInfoDB.IdentityInsert(newUser);
        }
        public static void InsertTable(PokerTable newPokerTable)
        {
            PokerTableDB pokerTableDB = new PokerTableDB();
            PokerTable pT = pokerTableDB.GetByTableByName(newPokerTable.PokerTableName);
            if (pT != null)
            {
                throw new Exception("Table already exists");
            }
            pokerTableDB.IdentityInsert(newPokerTable);
        }
        public static List<PokerTable> GetAllTables()
        {
            PokerTableDB pokerTableDB = new PokerTableDB();
            List<PokerTable> pokerTables = pokerTableDB.GetTables();
            if (pokerTables == null)
            {
                throw new Exception("No tables exist");
            }
            return pokerTables;
        }
        public static void DeleteTable(PokerTable requestedTable)
        {
            PokerTableDB pokerTableDB = new PokerTableDB();
            pokerTableDB.Delete(requestedTable);
        }
    }
}

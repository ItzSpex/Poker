using PokerDL.Models;
using PokerDL.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.ORM
{
    public class Database
    {
        private static List<User> users = null;

        public static List<User> GetAllUsers()
        {
            if (users == null)
            {
                PokerDB pokerDB = new PokerDB();
                users = pokerDB.SelectAllUsers();
            }
            return users;
        }
        public static User GetUserById(int id)
        {
            if(users == null)
            {
                PokerDB pokerDB = new PokerDB();
                return pokerDB.SelectUserByID(id);
            }
            return (users.FirstOrDefault
                (item => item.UserId == id));

        }
    }
}

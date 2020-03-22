using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Utils
{
    public static class UserUtils
    {
        public static User ReadUser(SqlDataReader reader)
        {
            User user = new User();
            user.UserId = (int)reader["PId"];
            user.Username = reader["Username"].ToString();
            user.Password = reader["Password"].ToString();
            return user;
        }
    }
}

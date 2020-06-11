using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Mapping
{
    public class UserInfoDB : BaseDB<UserInfo>
    {
        public UserInfo GetByUsername(string Username)
        {
            command.CommandText = "SELECT * FROM USERINFO WHERE Username = '" + Username + "'";
            var l = Select();
            if (l.Count == 1)
                return l[0];
            return null;
        }
        public UserInfo GetById(int UserId)
        {
            command.CommandText = "SELECT * FROM USERINFO WHERE PlayerId = " + UserId;
            var l = Select();
            if (l.Count == 1)
                return l[0];
            return null;
        }

        protected override void GetModelColumns(UserInfo model)
        {
            model.Id = (int)reader["PlayerId"];
            model.Username = reader["Username"].ToString();
            model.Password = reader["Password"].ToString();
            model.Wallet = (int)reader["Wallet"];
        }

        protected override string GetSQLDeleteString(UserInfo model)
        {
            return "DELETE FROM USERINFO Where PlayerId = " + model.Id;
        }

        protected override string GetSQLInsertString()
        {
            return "INSERT INTO USERINFO " +
                "(Username, Password, Wallet) " +
                "Values(@Username, @Password, @Wallet)";
        }

        protected override string GetSQLUpdateString(UserInfo model)
        {
            return "UPDATE USERINFO " +
                   "SET Username = @Username, Password = @Password, Wallet = @Wallet " +
                   "WHERE PlayerId = " + model.Id;

        }
        protected override void SetSQLParameters(UserInfo model)
        {
            this.command.Parameters.Add("@PlayerId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@Username",
                System.Data.SqlDbType.NVarChar, 50);
            this.command.Parameters.Add("@Password",
                System.Data.SqlDbType.NVarChar, 50);
            this.command.Parameters.Add("@Wallet",
                System.Data.SqlDbType.Int);

            this.command.Parameters["@PlayerId"].Value = model.Id;
            this.command.Parameters["@Username"].Value = model.Username;
            this.command.Parameters["@Password"].Value = model.Password;
            this.command.Parameters["@Wallet"].Value = model.Wallet;
        }
    }
}

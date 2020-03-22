using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.ORM
{
    public class PokerDB
    {
        private string connectionString = @"Data Source=LENOVO-LR09LUJJ\user; 
                                          Initial Catalog=PokerDB; Integrated Secuirty=True;";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;

        public PokerDB()
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
        }
        public List<User> SelectAllUsers()
        {
            command.CommandText = "SELECT * FROM USERINFO";
            List<User> users = new List<User>();
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    User user = new User();
                    user.UserId = (int)reader["PId"];
                    user.Username = reader["Username"].ToString();
                    user.Password = reader["Password"].ToString();
                    users.Add(user);
                }
            }
            catch (Exception e) { throw new Exception(e.Message); }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return users;
        }
        public User SelectUserByID(int id)
        {
            command.CommandText = "SELECT * FROM USERINFO WHERE PId = " + id;
            List<User> users = new List<User>();
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    User user = new User();
                    user.UserId = (int)reader["PId"];
                    user.Username = reader["Username"].ToString();
                    user.Password = reader["Password"].ToString();
                    users.Add(user);
                }
            }
            catch (Exception e) { throw new Exception(e.Message); }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            if (users.Count == 1)
                return users[0];
            return null;
        }
    }
}


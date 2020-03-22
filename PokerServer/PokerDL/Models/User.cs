using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Models
{
    [DataContract]
    public class User
    {
        public User()
        {

        }
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }

        public static bool operator ==(User user1, User user2)
        {
            return (user1.Username == user2.Username && user1.Password == user2.Password);
        }
        public static bool operator !=(User user1, User user2)
        {
            return (user1.Username != user2.Username || user1.Password != user2.Password);
        }
    }
}

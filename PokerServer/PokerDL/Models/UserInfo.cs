using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Models
{
    [DataContract]
    public class UserInfo : BaseIdentityEntity
    {
        public UserInfo()
        {

        }
        public UserInfo(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }

    }
}

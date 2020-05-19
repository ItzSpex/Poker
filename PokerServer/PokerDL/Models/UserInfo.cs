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
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public long Wallet { get; set; }

    }
}

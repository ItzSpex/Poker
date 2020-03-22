using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PokerDL.Models
{
    public enum Operation { Spectate = 0, Call, Raise, Fold, Check };
    [DataContract]    
    public class Move
    {
        [DataMember]
        public int PlayerId { get; set; }
        [DataMember]
        public Operation playerOperation { get; set; }
    }
}

using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    [DataContract]
    public class TableStatus
    {
        [DataMember]
        public List<Move> Moves { get; set; }
    }
}

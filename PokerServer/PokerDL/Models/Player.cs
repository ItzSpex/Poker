using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class Player : BaseIdentityEntity
    {
        [DataMember]
        public int PokerTableId { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public bool IsSpectator { get; set; }
        [DataMember]
        public bool IsDealer { get; set; }
        [DataMember]
        public long ChipsOnTable { get; set; }
        [DataMember]
        public string FirstCard { get; set; }
        [DataMember]
        public string SecondCard { get; set; }
    }
}

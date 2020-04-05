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
    public class PokerTable : BaseIdentityEntity
    {
        [DataMember]
        public int NumOfPlayers { get; set; }
        [DataMember]
        public long TablePot { get; set; }
        [DataMember]
        public int CurrTurn { get; set; }
        [DataMember]
        public Card FirstCard { get; set; }
        [DataMember]
        public Card SecondCard { get; set; }
        [DataMember]
        public Card ThirdCard { get; set; }
        [DataMember]
        public Card FourthCard { get; set; }
        [DataMember]
        public Card FifthCard { get; set; }
    }
}

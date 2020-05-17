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
        public string PokerTableName { get; set; }
        [DataMember]
        public bool HasStarted { get; set; }
        [DataMember]
        public int NumOfPlayers { get; set; }
        [DataMember]
        public long TablePot { get; set; }
        [DataMember]
        public int CurrTurn { get; set; }
        [DataMember]
        public int MinBet { get; set; }
        [DataMember]
        public string FirstCard { get; set; }
        [DataMember]
        public string SecondCard { get; set; }
        [DataMember]
        public string ThirdCard { get; set; }
        [DataMember]
        public string FourthCard { get; set; }
        [DataMember]
        public string FifthCard { get; set; }
    }
}

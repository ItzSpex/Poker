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
    public class Player : BaseIdentityEntity
    {
        [DataMember]
        public string PlayerName { get; set; }
        [DataMember]
        public int PokerTableId { get; set; }
        [DataMember]
        public int ChipsOnTable { get; set; }
        [DataMember]
        public string FirstCard { get; set; }
        [DataMember]
        public string SecondCard { get; set; }
    }
}

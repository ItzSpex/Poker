using PokerDL.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PokerBL.Models
{
    [DataContract]
    public class TableStatus
    {
        [DataMember]
        public Move LastMove { get; set; }
        [DataMember]
        public bool IsMyTurn { get; set; }
        [DataMember]
        public List<Card> TableCards { get; set; }
        [DataMember]
        public bool HasGameFinished { get; set; }
        [DataMember]
        public int WinnerId { get; set; }
        public TableStatus()
        {
            this.TableCards = new List<Card>();
        }
    }
}
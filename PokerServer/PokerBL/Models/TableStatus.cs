using PokerDL.Models;
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

        public TableStatus(Move lastMove, bool isMyTurn)
        {
            this.LastMove = lastMove;
            this.IsMyTurn = isMyTurn;
        }
    }
}
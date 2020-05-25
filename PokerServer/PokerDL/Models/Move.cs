using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PokerDL.Models
{
    public enum Operation { Call = 0, Raise, Fold, Check };
    [DataContract]    
    public class Move : BaseIdentityEntity
    {
        [DataMember]
        public int PlayerId { get; set; }
        [DataMember]
        public int PokerTableId { get; set; }
        [DataMember]
        public int BidAmount { get; set; }
        [DataMember]
        public Operation Operation { get; set; }
        public Move()
        {

        }
        public Move(Operation op, int bidAmount, int playerId, int TableId)
        {
            PlayerId = playerId;
            PokerTableId = TableId;
            BidAmount = bidAmount;
            Operation = op;
        }

    }
}

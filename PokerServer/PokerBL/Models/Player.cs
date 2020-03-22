using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class Player
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSpectator { get; set; }
        public long ChipsOnTable { get; set; }
        public long PlayerWallet { get; set; }
        public Card FirstCard { get; set; }
        public Card SecondCard { get; set; }
    }
}

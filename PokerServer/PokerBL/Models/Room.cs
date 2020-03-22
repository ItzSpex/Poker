using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int NumOfPlayers { get; set; }
        public long TablePot { get; set; }
        public int CurrTurn { get; set; }
        public Card FirstCard { get; set; }
        public Card SecondCard { get; set; }
        public Card ThirdCard { get; set; }
        public Card FourthCard { get; set; }
        public Card FifthCard { get; set; }
    }
}

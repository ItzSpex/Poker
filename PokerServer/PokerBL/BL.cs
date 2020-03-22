using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL
{
    public class BL
    {
        public class TableStatus
        {
            List<Player> tablePlayers { get; set; }
            Room myRoom { get; set; }
        }
        public class Room
        {
            int PTId { get; set; }
            int numOfPlayers { get; set; }
            long tablePot { get; set; }
            int currentTurn { get; set; }
            Card Card1 { get; set; }
            Card Card2 { get; set; }
            Card Card3 { get; set; }
            Card Card4 { get; set; }
            Card Card5 { get; set; }
            TableStatus tableStatus;
            
        }
        public class Player
        {
            int PId;
            int PTId;
            bool isAdmin;
            bool isSpectator;
            long chipsOnTable;
            long wallet;
            Card Card1;
            Card Card2;
        }
        public class Card
        {
            public enum Suite : byte { Spades = 10, Hearts = 20, Clubs = 40, Diamonds = 128 };
            public enum Face :byte { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

            public Suite suite;
            public Face face;

            public Card(Suite s, Face f)
            {
                this.suite = s;
                this.face = f;
            }

            public void printCard()
            {
                Console.WriteLine("Suite: " + suite + "Face: " + face);
            }
            public int getFaceValue()
            {
                return ((int)suite);
            }
        }
    }
}

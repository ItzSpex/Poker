using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class Card
    {
        public enum Suite : byte { Spades = 10, Hearts = 20, Clubs = 40, Diamonds = 128 };
        public enum Face : byte { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

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
            return (int)face;
        }
        public int getCardValue()
        {
            return (int)suite + (int)face;
        }
    }
}

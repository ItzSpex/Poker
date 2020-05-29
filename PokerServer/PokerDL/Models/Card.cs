using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Models
{
    public class Card
    {
        public string Suit { get; set; } //Spades Hearts Clubs Diamonds
        public string Face { get; set; }

        public Card(string s)
        {
            string[] words = s.Split(',');
            this.Suit = words[0];
            if (words.Length == 2)
            {
                this.Face = words[1];
            }
            
            
        }
        public override string ToString()
        {
            return Suit + "," + Face.ToString();
        }

        public string[] ToStringArr()
        {
            string[] arr = new string[2];
            arr[0] = this.Face;
            arr[1] = this.Suit;
            return arr;
        }
    }
}

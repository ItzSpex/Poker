using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class Card
    {
        public string Suite { get; set; } //Spades Hearts Clubs Diamonds
        public int Face { get; set; }

        public Card(string s)
        {
            string[] words = s.Split(',');
            this.Suite = words[0];
            this.Face = Convert.ToInt32(words[1]);
        }
        public override string ToString()
        {
            return Suite + "," + Face.ToString();
        }
    }
}

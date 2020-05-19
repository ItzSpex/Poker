using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class PokerTableBL : PokerTable
    {
        public bool HasStarted { get; set; }
        public int NumOfPlayers { get; set; }
        public int CurrTurn { get; set; }
        public Deck TableDeck { get; set; }
        public List<int> PlayerIds { get; set; }

        public PokerTableBL(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            this.PokerTableName = PokerTableName;
            this.NumOfPlayers = NumOfPlayers;
            this.MinBet = MinBetAmount;
            this.TableDeck = new Deck();
            this.PlayerIds = new List<int>();
        }
    }
}

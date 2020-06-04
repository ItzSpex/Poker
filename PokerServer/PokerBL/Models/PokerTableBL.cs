using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    [DataContract]
    public class PokerTableBL : PokerTable
    {
        [DataMember]
        public bool HasStarted { get; set; }
        [DataMember]
        public int NumOfPlayers { get; set; }
        [DataMember]
        public int CurrTurn { get; set; }
        [DataMember]
        public int PokerTableId { get; set; }
        [DataMember]
        public List<Move> Moves { get; set; }
        [DataMember]
        public List<PlayerBL> Players { get; set; }
        public Deck TableDeck { get; set; }
        public int LastBid { get; set; }
        public int DealerIndex { get; set; }
        public int FirstPlayerId { get; set; }
        public Round CurrRound { get; set; }

        public PokerTableBL(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            this.PokerTableId = Id;
            this.PokerTableName = PokerTableName;
            this.NumOfPlayers = NumOfPlayers;
            this.MinBet = MinBetAmount;
            this.TableDeck = new Deck();
            this.TableDeck.ShuffleCards();
            this.Players = new List<PlayerBL>();
            this.Moves = new List<Move>();
            this.CurrRound = Round.Deal;
        }
        public void GenerateDealerIndex()
        {
            if (DealerId == -1)
            {
                Random r = new Random();
                DealerIndex = r.Next(0, NumOfPlayers);
                DealerId = Players[DealerIndex].Id;
                FirstPlayerId = DealerIndex;
            }
           
        }
        

    }
}

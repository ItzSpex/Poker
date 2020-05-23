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
        public bool IsClosed { get; set; } = false;
        public int LoggedInPlayers { get; set; }
        public Deck TableDeck { get; set; }
        public List<int> PlayerIds { get; set; }
        public List<string> PlayerNames { get; set; }
        public int AdminId { get; set; }

        public PokerTableBL(string PokerTableName, int NumOfPlayers, int MinBetAmount, int UserId)
        {
            this.PokerTableId = Id;
            this.PokerTableName = PokerTableName;
            this.NumOfPlayers = NumOfPlayers;
            this.MinBet = MinBetAmount;
            this.TableDeck = new Deck();
            this.PlayerIds = new List<int>();
            this.Moves = new List<Move>();
            this.AdminId = UserId;
        }
        public void GenerateDealerId()
        {
            int DealerIndex = 0;
            Random r = new Random();
            DealerIndex = r.Next(0, NumOfPlayers);
            DealerId = PlayerIds[DealerIndex];
        }
    }
}

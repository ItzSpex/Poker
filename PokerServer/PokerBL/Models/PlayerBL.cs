using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class PlayerBL : Player
    {
        public long CurrWallet { get; set; }
        public List<Card> PersonalCards { get; set; }
        public bool IsPlayingThisGame { get; set; }
        
        public PlayerBL(int TableId, UserInfo user)
        {
            Id = user.Id;
            PlayerName = user.Username;
            CurrWallet = user.Wallet;
            PokerTableId = TableId;
            PersonalCards = new List<Card>();
        }
        public void GeneratePersonalCards(PokerTableBL myTable)
        {
            PersonalCards = myTable.TableDeck.GetCards(2);
            FirstCard = PersonalCards[0].ToString();
            SecondCard = PersonalCards[1].ToString();
        }
    }
}

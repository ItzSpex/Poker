using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    class PlayerBL : Player
    {
        int CurrWallet { get; set; }
        List<Card> personalCards { get; set; }
        
        public PlayerBL(int Wallet, int TableId)
        {
            CurrWallet = Wallet;
            PokerTableId = TableId;
            personalCards = new List<Card>();
        }
        public void GeneratePersonalCards(PokerTableBL myTable)
        {
            personalCards = myTable.TableDeck.GetCards(2);
            FirstCard = personalCards[0].ToString();
            SecondCard = personalCards[1].ToString();
        }
    }
}

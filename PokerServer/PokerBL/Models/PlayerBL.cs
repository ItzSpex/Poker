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
    public class PlayerBL : Player
    {
        public int CurrWallet { get; set; }
        public List<Card> PersonalCards { get; set; }
        public bool IsPlayingThisGame { get; set; }
        [DataMember]
        public int PlayerId { get; set; }   
        public PlayerBL(int TableId, UserInfo user)
        {
            Id = user.Id;
            PlayerId = user.Id;
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

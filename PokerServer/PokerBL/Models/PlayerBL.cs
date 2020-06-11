using PokerBL.ORM;
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
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class PlayerBL : Player
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public int CurrWallet { get; set; }
        public List<Card> PersonalCards { get; set; }
        public bool IsPlayingThisGame { get; set; }
        public PlayerBL(UserInfo user)
        {
            PlayerId = user.Id;
            PlayerName = user.Username;
            CurrWallet = user.Wallet;
            PersonalCards = new List<Card>();
        }
        public PlayerBL(UserInfo user, int TableId)
        {
            PlayerId = user.Id;
            PlayerName = user.Username;
            CurrWallet = user.Wallet;
            PersonalCards = new List<Card>();
            PersonalCards.Add(new Card(Database.GetPlayerById(user.Id, TableId).FirstCard));
            PersonalCards.Add(new Card(Database.GetPlayerById(user.Id, TableId).SecondCard));
            PokerTableId = TableId;
        }
        public PokerTableBL GeneratePersonalCards(PokerTableBL myTable)
        {
            PersonalCards = myTable.TableDeck.GetCards(2);
            FirstCard = PersonalCards[0].ToString();
            SecondCard = PersonalCards[1].ToString();
            for (int i = 0; i < myTable.Players.Count; i++)
            {
                if(myTable.Players[i].PlayerId == PlayerId)
                {
                    myTable.Players[i].PersonalCards = PersonalCards;
                }
            }
            return myTable;
        }
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                PlayerBL p = (PlayerBL)obj;
                return (PlayerId == p.PlayerId);
            }
        }
    }
}

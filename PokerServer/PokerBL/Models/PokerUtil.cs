using PokerBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public class PokerUtil
    {
        public static Deck Deck;
    }
    public class PreDeal
    {
        public int DealerId { get; set; }
        public List<Card> PersonalCards { get; set; }
        public PreDeal(int NumOfPlayers, List<int> PlayerIds)
        {
            Random Rand = new Random();
            DealerId = PlayerIds[Rand.Next(0, NumOfPlayers)];
            PokerUtil.Deck = new Deck();
            PersonalCards = PokerUtil.Deck.GetCards(2); 
        }
    }
    public class Deck
    {
        public Queue<Card> cards = new Queue<Card>(52);
        string[] numbers = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" };
        public Deck()
        {
            foreach (string s in numbers)
            {
                cards.Enqueue(new Card("Clubs," + s));

            }
            foreach (string s in numbers)
            {
                cards.Enqueue(new Card("Spades," + s));

            }
            foreach (string s in numbers)
            {
                cards.Enqueue(new Card("Hearts," + s));

            }
            foreach (string s in numbers)
            {
                cards.Enqueue(new Card("Diamonds," + s));

            }
        }
        public void ShuffleCards()
        {
            Random r = new Random();
            List<Card> CardList = new List<Card>();
            for (int i = 0; i < cards.Count; i++)
            {
                CardList.Add(cards.Dequeue());
            }
            //Shuffling Cards:
            for (int i = CardList.Count - 1; i > 0; --i)
            {
                int j = r.Next(i + 1);
                Card temp = CardList[i];
                CardList[i] = CardList[j];
                CardList[j] = temp;
            }
            foreach (Card card in CardList)
            {
                cards.Enqueue(card);
            }
        }
        public Card GetCard()
        {
            ShuffleCards(); //Todo: Decide if the shuffle should be here or in ctor.
            return cards.Dequeue();
        }
        public List<Card> GetCards(int amount)
        {
            List<Card> cardsRequested = new List<Card>(amount);
            for (int i = 0; i < amount; i++)
            {
                cardsRequested.Add(GetCard());
            }
            return cardsRequested;
        }


    }
}

using PokerBL.Models;
using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.Models
{
    public enum Rank { RoyalFlush, StraightFlush, FourOfAKind, FullHouse, Flush, Straight, ThreeOfAKind, TwoPair, OnePair, HighCard }
    public enum Round { Deal, PreFlop, Flop, Turn, River, Showdown }
    public class Deck
    {
        const int DeckSize = 52;
        public Queue<Card> cards = new Queue<Card>(DeckSize);
        readonly string[] numbers = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" };
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
            List<Card> CardList = new List<Card>();
            for (int i = 0; i < DeckSize; i++)
            {
                CardList.Add(cards.Dequeue());
            }
            //Shuffling Cards:
            Shuffle(CardList);
            foreach (Card card in CardList)
            {
                cards.Enqueue(card);
            }
        }
        private void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public Card GetCard()
        {
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
        public static List<string[]> ToStrArrList(List<Card> cards)
        {
            List<string[]> RequestedCardList = new List<string[]>();
            foreach (Card card in cards)
            {
                RequestedCardList.Add(card.ToStringArr());
            }
            return RequestedCardList;
        }
    }
    [DataContract]
    public class StartGameStatus
    {
        [DataMember]
        public Card PlayerCard1 { get; set; }
        [DataMember]
        public Card PlayerCard2 { get; set; }
        [DataMember]
        public Move SmallBlind { get; set; }
        [DataMember]
        public Move BigBlind { get; set; }
        [DataMember]
        public int PlayerId { get; set; }
        [DataMember]
        public int DealerId { get; set; }
    }
}

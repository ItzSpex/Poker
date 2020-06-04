using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Card = PokerClient.PokerServiceRef.Card;

namespace PokerClient
{
    /// <summary>
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        public Card FirstCard { get; set; }
        public Card SecondCard { get; set; }
        public Card ThirdCard { get; set; }
        public Card FourthCard { get; set; }
        public Card FifthCard { get; set; }
        public TableControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void SetCards(List<Card> cards)
        {
            if (cards.Count == 0)
            {
                FirstCard.Suit = "Card";
                FirstCard.Face = "Back";
                SecondCard.Suit = "Card";
                SecondCard.Face = "Back";
                ThirdCard.Suit = "Card";
                ThirdCard.Face = "Back";
                FourthCard.Suit = "Card";
                FourthCard.Face = "Back";
                FifthCard.Suit = "Card";
                FifthCard.Face = "Back";
            }
            FirstCard = cards[0];
            SecondCard = cards[1];
            ThirdCard = cards[2];
            FourthCard = cards[3];
            FifthCard = cards[4];
        }
    }
}

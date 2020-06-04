using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class OpponentControl : PlayerControl
    {
        public OpponentControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public string Angle { get; set; } = "0";
    }
    public class PlayerControl : UserControl, INotifyPropertyChanged
    {
        public PlayerControl()
        {
            this.DataContext = this;
        }
        public string PlayerName { get; set; }
        public int BetAmount { get; set; }
        public string BetAmountStr
        {
            get
            {
                return "Bet: " + BetAmount.ToString();
            }
            set
            {
            }
        }
        public Card FirstCard
        {
            set
            {
                firstCard = value;
                OnPropertyChanged("FirstCardBitmap");
            }
            get
            {
                return firstCard;
            }
        }
        private Card firstCard;

        public Card SecondCard { get; set; }
        public BitmapImage FirstCardBitmap
        {
            get
            {

                if (FirstCard != null)
                {
                    string Path = @"Resources\" + FirstCard.Suit + "," + FirstCard.Face + ".png";
                    return new BitmapImage(new Uri(Path,UriKind.RelativeOrAbsolute));
                }
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

    }
}

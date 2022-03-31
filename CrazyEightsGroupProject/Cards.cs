using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CrazyEightsGroupProject
{
    public class Card
    {
        public enum Suit
        {
            Club = 1,
            Diamond = 2,
            Heart = 3,
            Spades = 4,
        }

        public enum Number
        {
            Ace = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
        }

        public Suit CardSuit { get; set; }
        public Number CardNumber { get; set; }
        public Boolean IsFlipped { get; set; }

        public BitmapImage CardImage { get; set; }

        public Card (Suit suit, Number cardNumber, Boolean isFlipped)
        {
            this.CardSuit = suit;
            this.CardNumber = cardNumber;
            this.IsFlipped = isFlipped;
            this.CardImage = GetCardImagePath(suit, cardNumber, isFlipped);
        }

        public Card(Suit suit, Number cardNumber)
        {
            this.CardSuit = suit;
            this.CardNumber = cardNumber;
            this.IsFlipped = true;
            this.CardImage = GetCardImagePath(suit, cardNumber, IsFlipped);
        }

        public Card()
        {

        }

        public BitmapImage GetCardImagePath(Suit cardSuit, Number cardNumber, Boolean isFlipped)
        {
            string suit = cardSuit.ToString();
            string suiturl = "";

            // if the card is flipped, figure out what card image needs to be played
            if (!isFlipped)
            {

                if (suit == "Club")
                {
                    suiturl = "C";
                }
                else if (suit == "Diamond")
                {
                    suiturl = "D";
                }
                else if (suit == "Heart")
                {
                    suiturl = "H";
                }
                else if (suit == "Spades")
                {
                    suiturl = "S";
                }

                BitmapImage img = new BitmapImage();

                img.BeginInit();
                img.UriSource = new Uri("Classic/" + suiturl + (int)cardNumber + ".png", UriKind.RelativeOrAbsolute);
                img.EndInit();

                return img;
            }
            // if it is not flipped, that means the back just needs to show.
            else
            {
                BitmapImage img = new BitmapImage();

                img.BeginInit();
                img.UriSource = new Uri("Classic/Back.png", UriKind.RelativeOrAbsolute);
                img.EndInit();

                return img;
            }
        }


        public override string ToString()
        {
            return CardNumber.ToString() + " of " + CardSuit.ToString();
        }

        public void Flip()
        {
            if (IsFlipped == true)
            {
                IsFlipped = false;
            }
            else
            {
                IsFlipped = true;
            }

        }
    }

    public class Deck
    {
        public Deck()
        {
            Reset();
        }

        public List<Card> Cards { get; set; }

        public void Reset()
        {
            Cards = Enumerable.Range(1, 4)
                .SelectMany(s => Enumerable.Range(1, 13)
                                    .Select(c => new Card()
                                    {
                                        CardSuit = (Card.Suit)s,
                                        CardNumber = (Card.Number)c


                                    }
                                            )
                            )
                   .ToList();
        }

        public void Shuffle()
        {
            Cards = Cards.OrderBy(c => Guid.NewGuid())
                         .ToList();
        }

        public Card TakeCard()
        {
            var card = Cards.FirstOrDefault();
            Cards.Remove(card);

            return card;
        }

        public List<Card> TakeCards(int numberOfCards)
        {
            var cards = Cards.Take(numberOfCards);

            //var takeCards = cards as Card[] ?? cards.ToArray();
            var takeCards = cards as List<Card> ?? cards.ToList();
            Cards.RemoveAll(takeCards.Contains);

            return takeCards;
        }

        public List<Card> Sort(List<Card> listOfCards)
        {
            List<Card> sorted = listOfCards.GroupBy(s => s.CardSuit).
                OrderByDescending(c => c.Count()).SelectMany(g => g.OrderByDescending(c => c.CardNumber)).ToList();

            return sorted;

        }
    }


}

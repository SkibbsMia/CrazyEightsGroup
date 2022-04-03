using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrazyEightsGroupProject
{
    /// <summary>
    /// Interaction logic for PlayScreen.xaml
    /// </summary>
    public partial class PlayScreen : Window
    {
        public Game game = null;
        public List<Player> players = null;
        public List<List<Image>> DecksImages = null;
        public PlayScreen(int numberOfPlayers)
        {
            InitializeComponent();
            // start the game
            game = new Game(numberOfPlayers);

            // grab a list of players
            players = game.GetPlayers();

            // create a list of each players cards images.
            List<Image> userImages = new List<Image>()
            {
                Card1, Card2, Card3, Card4, Card5, Card6, Card7, Card8, Card9, Card10, 
                Card11, Card12, Card13, Card14, Card15, Card16, Card17, Card18, Card19, Card20
            };

            List<Image> CPUOneImages = new List<Image>()
            {
                CPUOneCard1, CPUOneCard2, CPUOneCard3, CPUOneCard4, CPUOneCard5, CPUOneCard6,
                CPUOneCard7, CPUOneCard8, CPUOneCard9, CPUOneCard10, CPUOneCard11, CPUOneCard12, CPUOneCard13
            };

            List<Image> CPUTwoImages = new List<Image>()
            {
                CPUTwoCard1, CPUTwoCard2, CPUTwoCard3, CPUTwoCard4, CPUTwoCard5, CPUTwoCard6, CPUTwoCard7,
                CPUTwoCard8, CPUTwoCard9, CPUTwoCard10, CPUTwoCard11, CPUTwoCard12, CPUTwoCard13
            };

            List<Image> CPUThreeImages = new List<Image>()
            {
                CPUThreeCard1, CPUThreeCard2, CPUThreeCard3, CPUThreeCard4, CPUThreeCard5, CPUThreeCard6, CPUThreeCard7,
                CPUThreeCard8, CPUThreeCard9, CPUThreeCard10, CPUThreeCard11, CPUThreeCard12, CPUThreeCard13
            };

            // make a list of the lists
            List<List<Image>> ListofListImages = new List<List<Image>>()
            {
                userImages, CPUOneImages, CPUTwoImages, CPUThreeImages
            };

            DecksImages = ListofListImages;

            // Display all of the cards.
            LoadCards();
        }

        public void LoadCards()
        {
            // Grab the top card on the discard pile and display it.
            List<Card> discardPile = game.GetDiscardPile();
            Card topCard = discardPile[^1];

            // check to see if there is a need of a re-shuffle.
            if (game.GetDeck().Cards.Count == 5)
            {
                // grab a new deck
                game.SetDeck();

                // go through everyones hand and remove all of the cards specifically in the new deck
                foreach (Player player in players)
                {
                    foreach (Card card in player.GetHand())
                    {
                        game.GetDeck().Cards.Remove(card);
                    }
                }
                // then remove the discard top card.
                game.GetDeck().Cards.Remove(topCard);

                // Clear discard pile then add that top card again.
                discardPile.Clear();
                discardPile.Add(topCard);
            }

            //display top card in the discard pile.
            //DiscardPile.Source = topCard.CardImage;
            DiscardPile.Source = topCard.GetCardImagePath(topCard.CardSuit, topCard.CardNumber, topCard.IsFlipped);

            foreach (List<Image> list in DecksImages)
            {
                foreach (Image image in list)
                {
                    image.Visibility = Visibility.Hidden;
                }
            }

            // go through each player playings deck and display the cards.
            int i = 0;
            do
            {
                List<Image> thisHandsImages = DecksImages[i];
                int j = 0;
                foreach (Card card in players[i].GetHand())
                {
                    thisHandsImages[j].Visibility = Visibility.Visible;
                    //thisHandsImages[j].Source = card.CardImage;
                    thisHandsImages[j].Source = card.GetCardImagePath(card.CardSuit, card.CardNumber, card.IsFlipped);
                    j++;
                }
                i++;
            }
            while (i < players.Count);
        }

        private void DiscardCard(object sender, MouseButtonEventArgs e)
        {

            // When the user clicks a card in their deck, discard the card.
            Image clickedImage = sender as Image;

            // make sure that the cards are not clickable if they are in a middle of playing a eight.
            if (Club.Visibility == Visibility.Hidden)
            {
                // make sure that they are clicking on a usable card in their hand.
                if (clickedImage.Source != null || clickedImage.Visibility != Visibility.Hidden)
                {
                    // grab the card ontop of the discard pile
                    List<Card> discard = game.GetDiscardPile();
                    Card topCard = discard[^1];

                    // grab source name, figure out which card needs to be discarded.
                    string source = clickedImage.Source.ToString();
                    string[] subs = source.Split('/');
                    string name = subs[^2] + "/" + subs[^1];

                    // go through the players hand and make sure that they grab the right card.
                    Card discardCard = null;
                    foreach (Card card in players[0].GetHand())
                    {
                        //BitmapImage cardSource = card.CardImage;
                        BitmapImage cardSource = card.GetCardImagePath(card.CardSuit, card.CardNumber, card.IsFlipped);
                        if (cardSource.ToString() == name)
                        {
                            discardCard = card;
                        }
                    } 

                    // go ahead and discard the card.
                    int isDiscarded = game.DiscardCard(0, topCard, discardCard);

                    // if a eight was played, allow the player to choose the suit of their choice.
                    if (isDiscarded == 2)
                    {
                        // a eight was discarded.
                        MessageBox.Show("A Eight was Played! Please Choose a Suit.", "Played Crazy Eight!", MessageBoxButton.OK);
                        Club.Visibility = Visibility.Visible;
                        Diamond.Visibility = Visibility.Visible;
                        Spades.Visibility = Visibility.Visible;
                        Heart.Visibility = Visibility.Visible;
                        LoadCards();
                    }
                    if (isDiscarded == 1)
                    {
                        Thread.Sleep(1000);

                        // this means that it is the CPU's turn now.
                            for (int i = 1; i < players.Count; i++)
                        {
                        Card theTopCard = discard[^1];
                            int winCheck = game.CPUTurn(i, theTopCard);
                            if (winCheck == 4)
                            {
                                Loser();
                            }
                            LoadCards();
                            Thread.Sleep(1000);
                        }
                    }

                    if (isDiscarded == 3)
                    {
                        Winner();
                    }
                }
            }
        }

        private void EightPlayedClick(object sender, RoutedEventArgs e)
        {
            // grab which suit they clicked.
            Button buttonClicked = sender as Button;
            string suitName = buttonClicked.Name;

            // grab discard Pile.
            List<Card> discardPile = game.GetDiscardPile();

            // switch case to change the top cards suit.
            switch (suitName.ToLower())
            {
                case "club":
                    discardPile[^1].CardSuit = Card.Suit.Club;
                    break;
                case "diamond":
                    discardPile[^1].CardSuit = Card.Suit.Diamond;
                    break;
                case "heart":
                    discardPile[^1].CardSuit = Card.Suit.Heart;
                    break;
                case "spades":
                    discardPile[^1].CardSuit = Card.Suit.Spades;
                    break;
            }

            // get rid of the buttons.
            Club.Visibility = Visibility.Hidden;
            Diamond.Visibility = Visibility.Hidden;
            Spades.Visibility = Visibility.Hidden;
            Heart.Visibility = Visibility.Hidden;

            List<Card> discard = game.GetDiscardPile();

            Thread.Sleep(1000);

            // this means that it is the CPU's turn now.
            for (int i = 1; i < players.Count; i++)
            {
                Card theTopCard = discard[^1];
                int help = game.CPUTurn(i, theTopCard);
                if (help == 4)
                {
                    Loser();
                }
                LoadCards();
                Thread.Sleep(1000);
            }
        }

        private void PickUpCard(object sender, MouseButtonEventArgs e)
        {
            // make sure that the cards are not clickable if they are in a middle of playing a eight.
            if (Club.Visibility == Visibility.Hidden)
            {
                // grab the card ontop of the discard pile
                List<Card> discard = game.GetDiscardPile();
                Card topCard = discard[^1];

                // pick a card.
                int pickedACard = game.PickCard(0, topCard);

                if (pickedACard == 1)
                {
                    
                    // this means that it is the CPU's turn now.
                    for (int i = 1; i < players.Count; i++)
                    {
                    Card theTopCard = discard[^1];
                    int help = game.CPUTurn(i, theTopCard);
                        if (help == 4) {
                            Loser();
                        }
                        LoadCards();
                    Thread.Sleep(1000);
                    }
                }
                
            }
        }
        private void Winner()
        {
            MessageBox.Show("Congrats! You've won!", "Winner", MessageBoxButton.OK);
            this.Close();

        }

        private void Loser()
        {
            MessageBox.Show("You lost... Try again", "Loser", MessageBoxButton.OK);
            this.Close();

        }
    }
}
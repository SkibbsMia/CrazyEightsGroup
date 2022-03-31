using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyEightsGroupProject
{
    public class Game
    {
        private Deck deck;
        private readonly int numberOfPlayers = 2;
        private Player playerOne;
        private Player playerTwo;
        private Player playerThree = null;
        private Player playerFour = null;
        private List<Card> discardPile;
        private List<Card> tempList;
        private readonly List<Player> playerList = new List<Player>();

        public Game()
        {
            SetDeck();
            SetPlayers(numberOfPlayers);
            SetDiscardPile();
        }
        public Game(int numberOfPlayers)
        {
            SetDeck();
            SetPlayers(numberOfPlayers);
            SetDiscardPile();
        }

        public void SetDiscardPile()
        {
            this.discardPile = new List<Card>();
            Card firstCard = deck.TakeCard();
            discardPile.Add(firstCard);
        }

        public List<Card> GetDiscardPile()
        {
            return discardPile;
        }

        public void SetPlayers(int numberOfPlayers)
        {
            // In a game there is a minimum of two, so alway create two players.
            this.playerOne = new Player("You", deck.Sort(deck.TakeCards(8)), false);
            this.playerTwo = new Player("CPU-1", deck.Sort(deck.TakeCards(8)), true);
            playerList.Add(playerOne);
            playerList.Add(playerTwo);

            // If there is 3 players, add one more.
            if (numberOfPlayers >= 3)
            {
                this.playerThree = new Player("CPU-2", deck.Sort(deck.TakeCards(8)), true);
                playerList.Add(playerThree);
            }
            // If there is 4 players, add a last one.
            if (numberOfPlayers >= 4)
            {
                this.playerFour = new Player("CPU-3", deck.Sort(deck.TakeCards(8)), true);
                playerList.Add(playerFour);
            }
        }

        public List<Player> GetPlayers()
        {
            return playerList;
        }

        public void SetDeck()
        {
            this.deck = new Deck();
            this.deck.Shuffle();
        }

        public Deck GetDeck()
        {
            return this.deck;
        }

        public bool PlayableCard(int playerNumber, Card topCard)
        {
            bool playeable = false;
            // go through all cards in the hand.
            foreach (Card card in playerList[playerNumber].GetHand())
            {
                // if there is a card that is playable, return true
                if (card.CardSuit == topCard.CardSuit || card.CardNumber == topCard.CardNumber || card.CardNumber == Card.Number.Eight)
                {
                    playeable = true;
                    return playeable;
                }
            }
            // if not return false.
            return playeable;
        }

        public int PickCard(int playerNumber, Card topCard)
        {
            int pickedUpCard = 0;
            // If there is not a playable card, pick up a card.
            if (PlayableCard(playerNumber, topCard) == false)
            {
                // grab a card from the deck.
                Card grabbedcard = deck.TakeCard();

                //flip the card if the cpu picked it up
                if (playerList[playerNumber].GetIsCPU() == true)
                {
                    grabbedcard.Flip();
                }
                // add it to the current players hand
                playerList[playerNumber].GetHand().Add(grabbedcard);

                pickedUpCard = 1;
            }
            return pickedUpCard;
        }


        public bool CardPlayable(Card topCard, Card card)
        {
            bool playable = false;

            // if the card is playable return true
            if (card.CardSuit == topCard.CardSuit || card.CardNumber == topCard.CardNumber || card.CardNumber == Card.Number.Eight)
            {
                playable = true;
            }

            return playable;
        }

        public int DiscardCard(int playerNumber, Card topCard, Card discardCard)
        {
            int discarded = 0;
            if (CardPlayable(topCard, discardCard))
            {
                // discard the card.
                // if the player is a cpu, then flip the card around.
                if (playerList[playerNumber].GetIsCPU() == true)
                {
                    discardCard.Flip();
                }
                discardPile.Add(discardCard);
                playerList[playerNumber].GetHand().Remove(discardCard);
                



                discarded = 1;



                if (discardCard.CardNumber == Card.Number.Eight)
                {
                    // When a player hits the eight, change suit.
                    discarded = 2;

                    // if the cpu is the one who played the eight, do algorithom.
                    if (playerList[playerNumber].GetIsCPU() == true)
                    {
                        //now we need to build the algorithom for when a cpu plays a eight.
                        int heart = 0;
                        int club = 0;
                        int diamond = 0;
                        int spades = 0;

                        // count how many of each suit it has.
                        foreach (Card cardCount in playerList[playerNumber].GetHand())
                        {
                            if (cardCount.CardSuit == Card.Suit.Club)
                            {
                                club++;
                            }
                            else if (cardCount.CardSuit == Card.Suit.Diamond)
                            {
                                diamond++;
                            }
                            else if (cardCount.CardSuit == Card.Suit.Heart)
                            {
                                heart++;
                            }
                            else if (cardCount.CardSuit == Card.Suit.Spades)
                            {
                                spades++;
                            }
                        }

                        if (club > diamond && club > heart && club > spades)
                        {
                            // they have more clubs.
                            discardPile[^1].CardSuit = Card.Suit.Club;

                        }
                        else if (diamond > club && diamond > heart && diamond > spades)
                        {
                            // they have more diamonds
                            discardPile[^1].CardSuit = Card.Suit.Diamond;
                        }
                        else if (heart > club && heart > diamond && heart > spades)
                        {
                            // they have more hearts
                            discardPile[^1].CardSuit = Card.Suit.Heart;
                        }
                        else if (spades > club && spades > diamond && spades > heart)
                        {
                            // they have more spades.
                            discardPile[^1].CardSuit = Card.Suit.Spades;
                        }
                    }
                }
                else if (discardCard.CardNumber == Card.Number.Two)
                {
                    // Grab two cards from the deck.
                    Card grabbedcard1 = deck.TakeCard();
                    Card grabbedcard2 = deck.TakeCard();

                    int playerpicking = playerNumber;
                    // If the player is at the end of the list, send cards to the first player in the list.
                    if (playerpicking == playerList.Count - 1)
                    {
                        playerpicking = 0;
                    }
                    else
                    {
                        playerpicking++;
                    }

                    // Check if the player who is getting the card is a CPU or not
                    if (playerList[playerpicking].GetIsCPU() == true)
                    {
                        grabbedcard2.Flip();
                        grabbedcard1.Flip();
                    }
                    playerList[playerpicking].GetHand().Add(grabbedcard1);
                    playerList[playerpicking].GetHand().Add(grabbedcard2);

                }
            }
            tempList = playerList[playerNumber].GetHand();
            if (tempList.Count == 0)
            {
                discarded = 3;
            }

            if (tempList.Count == 0 && playerList[playerNumber].GetIsCPU() == true)
            {
                discarded = 4;
            }
            return discarded;
        }

        public int CPUTurn(int playerNumber, Card topCard)
        {
            int CPUwin = 0;
            bool hasPlayableCard = false;
            // check to see if the cpu has a playable card.
            foreach (Card card in playerList[playerNumber].GetHand())
            {

                hasPlayableCard = PlayableCard(playerNumber, topCard);

                if (hasPlayableCard)
                {
                    break;
                }
            }

            // If they have a playable card, then discard the card with algorithom.
            if (hasPlayableCard)
            {
                foreach (Card card in playerList[playerNumber].GetHand())
                {
                    // look at only cards that are valid.
                    if (card.CardNumber == topCard.CardNumber || card.CardSuit == topCard.CardSuit)
                    {
                        // go for the normal cards first.
                        if (card.CardNumber != Card.Number.Eight && card.CardNumber != Card.Number.Two)
                        {
                            CPUwin = DiscardCard(playerNumber, topCard, card);
                            
                            break;
                        }
                        // go and play the pick up two card next
                        else if (card.CardNumber == Card.Number.Two)
                        {
                            CPUwin = DiscardCard(playerNumber, topCard, card);
                            break;
                        }
                        // then play a eight if nothing else can be played.
                        else if (card.CardNumber == Card.Number.Eight)
                        {
                            CPUwin = DiscardCard(playerNumber, topCard, card);

                            break;
                        }
                    }
                }

            }
            // this means that there is no playable cards in the CPU's deck, meaning they need to pick up a card.
            else
            {
                PickCard(playerNumber, topCard);
            }
            return CPUwin;
        }

    }
}

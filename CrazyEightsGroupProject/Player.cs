using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyEightsGroupProject
{
    public class Player
    {
        private string name;
        private List<Card> playersCards;
        private bool isCPU;

        public Player()
        {
            SetName("Player");
            SetHand(this.playersCards);
        }

        public Player(string name, List<Card> hand, bool cpu)
        {
            SetCPU(cpu);
            SetName(name);
            SetHand(hand);
        }

        public void SetCPU(bool cpu)
        {
            this.isCPU = cpu;
        }

        public bool GetIsCPU()
        {
            return isCPU;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetHand(List<Card> playersCards)
        {
            if (isCPU == true)
            {
                foreach (Card card in playersCards)
                {
                    card.Flip();
                }
            }
            this.playersCards = playersCards;
        }

        public string GetName()
        {
            return name;
        }

        public List<Card> GetHand()
        {
            return playersCards;
        }
    }
}

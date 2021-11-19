using System;
using System.Collections.Generic;
using System.Linq;
using Urho;

namespace CardGameExample
{
    public class CardStackManager
    {
        /*
         * Manger of a card stack.
         */
        public string Tag { get; set; }                       //tag value of stack entity
        public CardStack StackComp { get; set; }
        public Node LastCardonStack { get; set; }             //last card in stack
        public List<Node> CardsInStack { get; set; }          //bring cards up one level
        public int FannedDirection { get; set; }                //fanning direction
        public Vector2 FanOutDistannce { get; set; }            //distance of cards from each other
        public int TotalCards { get; set; }
        public CardStackManager(Node _cardStack)
        {
            //
            // Stack entity holding cards
            //
            Tag = _cardStack.Name;
            StackComp = new CardStack();
            LastCardonStack = new Node();

            StackComp = _cardStack.GetComponent<CardStack>();
            if (StackComp == null)
                return;

            if (StackComp.CardsInStack.Count == 0)
                return;
            else
                TotalCards = StackComp.CardsInStack.Count;

            LastCardonStack = StackComp.CardsInStack.LastOrDefault();
            CardsInStack = StackComp.CardsInStack;
            FannedDirection = StackComp.FannedDirection;
            switch (FannedDirection)
            {
                case 0:
                    FanOutDistannce = Vector2.Zero;
                    break;
                case 1:
                    FanOutDistannce = new Vector2(0.35f, 0);
                    break;
                case 2:
                    FanOutDistannce = new Vector2(-0.35f, 0);
                    break;
                case 3:
                    FanOutDistannce = new Vector2(0, 0.35f);
                    break;
                case 4:
                    FanOutDistannce = new Vector2(0, -0.35f);
                    break;

            }
        }
        public Node GetCard(int _no)
        {
            if ((StackComp.CardsInStack.Count < _no) || (_no > StackComp.CardsInStack.Count))
                return null;

            return StackComp.CardsInStack[_no];
        }
    }
}

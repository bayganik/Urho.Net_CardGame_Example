using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Urho2D;

namespace CardGameExample
{
    public class CardStack : Component
    {
        public int StackId;
        public Vector3 Position;
        public List<Node> CardsInStack;
        //
        // are cards in this stack fanned out?
        //
        public int FannedDirection = 4;     // 0=stack on top eachother, 1=right, 2=left, 3=up, 4=down
        public float FannedOffset = 0.35f;  // how far to separate the cards from eachother
        public CardStack()
        {
            CardsInStack = new List<Node>();
        }
        public Node GetFirstCard()
        {
            if (CardsInStack.Count <= 0)
                return null;

            return CardsInStack[0];
        }
        public Node GetCard(int _cardSeq)
        {
            if (_cardSeq > CardsInStack.Count - 1)
                return null;

            return CardsInStack[_cardSeq];
        }
    }
}

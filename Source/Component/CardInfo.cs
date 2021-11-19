using System;
using System.Collections.Generic;
using System.Text;

using Urho;
using Urho.Urho2D;

namespace CardGameExample
{
    /*
     * CardInfo component holds all data about a card
     */
    public class CardInfo : Component
    {
        public CardStack HoldingStack;      // Card stack that holds this card
        public int Index = 0;               // 0 - 51 e.g. cardfaces[faceIndex];
        public int FaceNumber = 0;           // 2 two,.. 9 ten, 11 jack, 12 queen, 13 king, 14 Ace
        public int Suit = 0;                // 0 heart, 1 dimond, 2 clubs, 3 spade
        public bool IsFaceUp = true;        // card face showing?
        public bool IsRed = true;           // could be used in game like Solitair
        public int Rank = 2;                // 2 two, 10 ten, 10 jack, 10 queen, 10 king, 11 Ace 
        public int RankExtra = 0;           // 1 Ace can also be ONE in blackjack
        public StaticSprite2D FaceImage;    // sprite image of a card
        public StaticSprite2D BackImage;    // sprite image of back of a card
        public CardInfo()
        {
            //Node = node;
        }
    }
}

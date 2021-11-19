using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Urho2D;
using Urho.Actions;
using Urho.Shapes;

namespace CardGameExample
{
    /*
     *  CardDeckManager class to manage a deck of cards
     *      creation of visual cards
     *      creation of nodes for each card (when dealing out)
     *      shuffle
     */

    public class CardDeckManager
    {
        public int[] CardIndx;                      // shuffeled card numbers
        //public StaticSprite2D[] CardFace;           // image of each card face
        public StaticSprite2D[] CardBack;           // image of each card back
        public CardInfo[] CardInfo;                 // information about the card

        public int CurrentCardNumber = 0;           // sequential pointer to cardDeck;
        public int CurrentCardBack = 6;             // red back

        public StaticSprite2D CardX;                // image of X card
        public StaticSprite2D CardO;                // image of O card
        public StaticSprite2D CardJ;                // image of Jocker card

        public SpriteSheet2D SpriteSheet;
        public int cardWidth = 72;          //width of each card
        public int cardHeight = 100;        //height of each card
        public CardDeckManager()
        {
            InitAllCards();
            Shuffle();
        }
        public void InitAllCards()
        {
            // Get sprite sheet
            SpriteSheet = Global.ResourceCache.GetSpriteSheet2D("Cards/CardDeck.xml");
            if (SpriteSheet == null)
                return;

            //CardFace = new StaticSprite2D[52];
            CardBack = new StaticSprite2D[52];
            CardInfo = new CardInfo[52];
            CardIndx = new int[52];

            CurrentCardNumber = 0;      //card counter
            //
            // hearts
            //
            for (int cardInd = 2; cardInd < 15; cardInd++)
            {
                //CardFace[CurrentCardNumber] = GetSprite("H" + cardInd.ToString());
                CardIndx[CurrentCardNumber] = CurrentCardNumber;

                CardInfo ci = new CardInfo();
                ci.Index = CurrentCardNumber;
                if (cardInd == 14)
                    ci.FaceNumber = 1;      //ace is last card of the suit
                else
                    ci.FaceNumber = cardInd;
                ci.Suit = 0;                //hearts
                ci.IsRed = true;
                ci.IsFaceUp = true;
                if (cardInd > 9)
                    ci.Rank = 10;
                else
                    ci.Rank = cardInd;
                ci.FaceImage = GetSprite("H" + cardInd.ToString());
                CardInfo[CurrentCardNumber] = ci;

                CurrentCardNumber += 1;
            }
            //
            // diamonds
            //
            for (int cardInd = 2; cardInd < 15; cardInd++)
            {
                //CardFace[CurrentCardNumber] = GetSprite("D" + cardInd.ToString());
                CardIndx[CurrentCardNumber] = CurrentCardNumber;

                CardInfo ci = new CardInfo();
                ci.Index = CurrentCardNumber;
                if (cardInd == 14)
                    ci.FaceNumber = 1;      //ace is last card of the suit
                else
                    ci.FaceNumber = cardInd;
                ci.Suit = 1;                //diamond
                ci.IsRed = true;
                ci.IsFaceUp = true;
                if (cardInd > 9)
                    ci.Rank = 10;
                else
                    ci.Rank = cardInd;
                ci.FaceImage = GetSprite("D" + cardInd.ToString());
                CardInfo[CurrentCardNumber] = ci;

                CurrentCardNumber += 1;
            }
            //
            // clubs
            //
            for (int cardInd = 2; cardInd < 15; cardInd++)
            {
                //CardFace[CurrentCardNumber] = GetSprite("C" + cardInd.ToString());
                CardIndx[CurrentCardNumber] = CurrentCardNumber;

                CardInfo ci = new CardInfo();
                ci.Index = CurrentCardNumber;
                if (cardInd == 14)
                    ci.FaceNumber = 1;      //ace is last card of the suit
                else
                    ci.FaceNumber = cardInd;
                ci.Suit = 2;                //clubs
                ci.IsRed = false;
                ci.IsFaceUp = true;
                if (cardInd > 9)
                    ci.Rank = 10;
                else
                    ci.Rank = cardInd;
                ci.FaceImage = GetSprite("C" + cardInd.ToString());
                CardInfo[CurrentCardNumber] = ci;

                CurrentCardNumber += 1;
            }
            //
            // spades
            //
            for (int cardInd = 2; cardInd < 15; cardInd++)
            {
                //CardFace[CurrentCardNumber] = GetSprite("S" + cardInd.ToString());
                CardIndx[CurrentCardNumber] = CurrentCardNumber;

                CardInfo ci = new CardInfo();
                ci.Index = CurrentCardNumber;
                if (cardInd == 14)
                    ci.FaceNumber = 1;      //ace is last card of the suit
                else
                    ci.FaceNumber = cardInd;
                ci.Suit = 3;                //spades
                ci.IsRed = false;
                ci.IsFaceUp = true;
                if (cardInd > 9)
                    ci.Rank = 10;
                else
                    ci.Rank = cardInd;
                ci.FaceImage = GetSprite("S" + cardInd.ToString());
                CardInfo[CurrentCardNumber] = ci;

                CurrentCardNumber += 1;
            }

            CardX = GetSprite("X");
            CardO = GetSprite("O");
            CardJ = GetSprite("J");
            //
            // backs
            //
            CurrentCardNumber = 0;
            for (int cardInd = 1; cardInd < 11; cardInd++)
            {
                CardBack[CurrentCardNumber] = GetSprite("B" + cardInd.ToString());
                CurrentCardNumber += 1;
            }
            //
            // Reset to point to first card
            //
            CurrentCardNumber = 0;
            CurrentCardBack = 3;
        }
        public void Shuffle()
        {
            //
            // cardDeckPoint is shuffled and first card number = 0
            //
            int count = 51;
            int temp;
            for (int j = count; j > 1; j--)
            {
                temp = CardIndx[j];
                int Number = Global.NextRandom(0, j + 1);

                CardIndx[j] = CardIndx[Number];
                CardIndx[Number] = temp;
            }
            //Array.Sort(CardIndx);
            CurrentCardNumber = 0;
        }
        public StaticSprite2D GetSprite(string _cardImage)
        {
            StaticSprite2D staticSprite = new StaticSprite2D();
            Sprite2D sprite = SpriteSheet.GetSprite(_cardImage);
            if (sprite == null)
                throw new InvalidOperationException("sprite not found");

            staticSprite.BlendMode = BlendMode.Alpha;
            staticSprite.Sprite = sprite;
            staticSprite.Layer = 0;

            return staticSprite;
        }
        public StaticSprite2D GetCardBack()
        {
            StaticSprite2D card;
            card = CardBack[CurrentCardBack];

            return card;
        }
        public CardInfo DealOtherCard(string _type)
        {
            CardInfo card = new CardInfo();
            switch (_type)
            {
                case "O":
                    card.FaceImage = CardO;
                    break;
                case "X":
                    card.FaceImage = CardX;
                    break;
                case "J":
                    card.FaceImage = CardJ;
                    break;
            }

            return card;
        }
        public CardInfo DealCard()
        {
            CardInfo card;
            if (CurrentCardNumber > 51)
                return null;

            int cnum = CardIndx[CurrentCardNumber];
            card = CardInfo[cnum];
            card.BackImage = GetCardBack();
            
            CurrentCardNumber += 1;
            return card;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Urho2D;


namespace CardGameExample
{
    /*
     * PlayStack system to:
     *  Nodes with CardStack component system to display what is on their location (Fanned out, in place, etc.)
     */
    public class DispPlayStackSystem : NodeProcessingSystem
    {
        const float moveSpeed = 4f;
        Urho2DCards MyScene;
        //
        // how to fanout the cards (downward, upward, on top of eachother)
        //
        Vector2 fanOutDistannce;
        public DispPlayStackSystem(SceneComponent _scene, string _tag, bool _recursive = false) : base(_scene, _tag, _recursive)
        {
            MyScene = (Urho2DCards)_scene;
        }
        public override void Process()
        {
            //
            // Tag to get is "playstack" should get 7 of them
            //
            base.Process();
            Nodes = MyScene.MyScene.GetChildrenWithTag(Tag, Recursive);
            foreach(Node playPile in Nodes)
            {
                CardStack sc = playPile.GetComponent<CardStack>();
                switch (sc.FannedDirection)
                {
                    case 0:
                        fanOutDistannce = Vector2.Zero;
                        break;
                    case 1:
                        fanOutDistannce = new Vector2(0.35f, 0);
                        break;
                    case 2:
                        fanOutDistannce = new Vector2(-0.35f, 0);
                        break;
                    case 3:
                        fanOutDistannce = new Vector2(0, 0.35f);
                        break;
                    case 4:
                        fanOutDistannce = new Vector2(0, -0.35f);
                        break;

                }
                //
                // All cards are Nodes in this stack
                //
                int ind = 0;                            //cards number in stack

                for (int i = 0; i < sc.CardsInStack.Count; i++)
                {
                    Node cardEntity = sc.CardsInStack[i];
                    cardEntity.Enabled = true;
                    Vector2 fo = fanOutDistannce * new Vector2(ind, ind);
                    Vector2 position = new Vector2(playPile.Position.X + fo.X, playPile.Position.Y + fo.Y);

                    cardEntity.SetPosition2D(position);
                    StaticSprite2D sprite = cardEntity.GetComponent<StaticSprite2D>();
                    //
                    // bigger number layers drawn last (top)
                    //
                    sprite.Layer = ind * 1;
                    //
                    // is card face up or not
                    //
                    CardInfo ci = cardEntity.GetComponent<CardInfo>();
                    if (ci.IsFaceUp)
                        sprite.Sprite = ci.FaceImage.Sprite;
                    else
                        sprite.Sprite = ci.BackImage.Sprite;
                    
                    ind += 1;
                }

            }
        }
    }
}

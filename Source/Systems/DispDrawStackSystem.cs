using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Urho2D;


namespace CardGameExample
{
    /*
     * 2D camera move system using Translate
     * No need for search of Nodes with a tag, since we assume only one camera
     * in the scene
     */
    public class DispDrawStackSystem : NodeProcessingSystem
    {
        const float moveSpeed = 4f;
        Urho2DCards MyScene;
        //
        // Nodes with CardStack component system to display what is on their location (Fanned out, in place, etc.)
        //
        Vector2 fanOutDistannce;
        public DispDrawStackSystem(SceneComponent _scene, string _tag, bool _recursive = false) : base(_scene, _tag, _recursive)
        {
            MyScene = (Urho2DCards)_scene;
        }
        public override void Process()
        {
            //
            // Tag to get is "dealstack" should get ONE
            //
            base.Process();
            Nodes = MyScene.MyScene.GetChildrenWithTag(Tag, Recursive);
            if (Nodes.Length != 1)
                return;

            Node activePile = Nodes[0];

            CardStack sc = activePile.GetComponent<CardStack>();

            fanOutDistannce = Vector2.Zero;                // no fanning
            int ind = 0;                                   //cards layer number

            for (int i = 0; i < sc.CardsInStack.Count; i++)
            {
                Node cardEntity = sc.CardsInStack[i];
                cardEntity.Enabled = true;
                Vector2 fo = fanOutDistannce * new Vector2(ind, ind);
                var position = new Vector2(activePile.Position.X + fo.X, activePile.Position.Y + fo.Y);

                cardEntity.SetPosition2D(position);
                StaticSprite2D sprite = cardEntity.GetComponent<StaticSprite2D>();
                //
                // bigger number layers drawn last (top)
                //
                sprite.Layer = ind * 1;
                //
                // all cards face up
                //
                CardInfo ci = cardEntity.GetComponent<CardInfo>();
                sprite.Sprite = ci.FaceImage.Sprite;

                ind += 1;
            }
        }
    }
}

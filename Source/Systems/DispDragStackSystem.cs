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
    public class DispDragStackSystem : NodeProcessingSystem
    {
        const float moveSpeed = 4f;
        Urho2DCards MyScene;
        //
        // Nodes with CardStack component system to display what is on their location (Fanned out, in place, etc.)
        //
        Vector2 fanOutDistannce;
        public DispDragStackSystem(SceneComponent _scene, string _tag, bool _recursive = false) : base(_scene, _tag, _recursive)
        {
            MyScene = (Urho2DCards)_scene;
        }
        public override void Process()
        {
            //
            // Tag to get is "dragstack" should get ONE
            //
            base.Process();
            Nodes = MyScene.MyScene.GetChildrenWithTag(Tag, Recursive);
            if (Nodes.Length != 1)
                return;

            Node activePile = Nodes[0];
            CardStack sc = activePile.GetComponent<CardStack>();
            //
            // if drag pile is empty, no dragging needed
            //
            if (sc.CardsInStack.Count == 0)
                return;

            //
            // we have cards in the drag pile, so move them
            //
            Vector2 pos = MyScene.GetMousePositionXY(MyScene.Input.MousePosition.X, MyScene.Input.MousePosition.Y);
            activePile.Position = new Vector3(pos.X, pos.Y, 0);


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
                var position = new Vector2(activePile.Position.X + fo.X, activePile.Position.Y + fo.Y);

                cardEntity.SetPosition2D(position);
                StaticSprite2D sprite = cardEntity.GetComponent<StaticSprite2D>();
                //
                // bigger number layers drawn last (top)
                //
                sprite.Layer = (ind * 1) + 1000;
                //
                // cards are face up
                //
                CardInfo ci = cardEntity.GetComponent<CardInfo>();
                sprite.Sprite = ci.FaceImage.Sprite;

                ind += 1;
            }

        }
    }
}


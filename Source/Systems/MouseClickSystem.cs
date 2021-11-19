using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Urho2D;


namespace CardGameExample
{
    /*
     * This is the heart of the card game.  Where is the mouse and what did it click on?
     * Is the mouse down/up
     * Is it dragging cards
     * 
     * This system makes all the decisions on methods to call
     */
    public class MouseClickSystem : NodeProcessingSystem
    {
        const float moveSpeed = 4f;
        Urho2DCards MyScene;
        Node pickedNode;

        public MouseClickSystem(SceneComponent _scene, string _tag, bool _recursive = false) : base(_scene, _tag, _recursive)
        {
            MyScene = (Urho2DCards)_scene;
        }
        public override void Process()
        {
            base.Process();
            GameState gs = MyScene.GameState_Check();
            if (gs == GameState.GS_END)
                return;


            MyScene.Input.MouseButtonDown += MouseButtonDown;
            MyScene.Input.MouseButtonUp += MouseButtonUp;
        }
        private void MouseButtonDown(MouseButtonDownEventArgs args)
        {
            float mouseX, mouseY;
            if (args.Button != (int)MouseButton.Left)
                return;
          
            mouseX = MyScene.Input.MousePosition.X;
            mouseY = MyScene.Input.MousePosition.Y;
            RigidBody2D rigidBody = MyScene.Physics2D.GetRigidBody((int)mouseX, (int)mouseY); // Raycast for RigidBody2Ds to pick
            if (rigidBody == null)
                return;

            pickedNode = rigidBody.Node;
            switch (pickedNode.Name)
            {
                case "1":       //play stacks
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                    Node clickedOn = MyScene.GetMousePositionNode(mouseX, mouseY);
                    if (clickedOn == null)      //no card Node was clicked on
                        break;  
                    //
                    // Find all face up cards that can be dragged
                    //
                    MyScene.TakePlayStackCards2Drag(clickedOn);
                    break;
                case "30":      //ace
                case "40":      //ace
                case "50":      //ace
                case "60":      //ace
                case "80":      //draw from stack
                    MyScene.DealOneCard2Drag(pickedNode);
                    break;
                case "90":      //deal from stack
                    MyScene.DealOneCard2DrawStack();
                    break;
            }
            //
            // Stop executing every frame
            //
            MyScene.Input.MouseButtonDown -= MouseButtonDown;
        }
        private void MouseButtonUp(MouseButtonUpEventArgs args)
        {
            if (args.Button == (int)MouseButton.Right)
                return;

            RigidBody2D rigidBody = MyScene.Physics2D.GetRigidBody(MyScene.Input.MousePosition.X, MyScene.Input.MousePosition.Y); // Raycast for RigidBody2Ds to pick
            if (rigidBody == null)
            {
                //
                // if we didn't drop on any stacks, return cards
                //
                MyScene.ReturnCardFromDrag2Stack();
                return;
            }

            pickedNode = rigidBody.Node;
            switch (pickedNode.Name)
            {
                case "1":       //play stacks
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                    MyScene.DropCardFromDrag2PlayStack(pickedNode);

                    break;
                case "30":      //ace
                case "40":      //ace
                case "50":      //ace
                case "60":      //ace
                case "80":      //drawstack
                    MyScene.DropCardFromDrag2AceStack(pickedNode);

                    break;
                case "90":
                    break;
                default:
                    //
                    // we didn't drop on any stacks, return cards to original stacks
                    //
                    MyScene.ReturnCardFromDrag2Stack();
                    break;
            }
            //
            // Stop executing every frame
            //
            MyScene.Input.MouseButtonUp -= MouseButtonUp;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Actions;
using Urho.Shapes;

namespace CardGameExample
{
    /*
     * 3D camera move system using FiniteTimeAction
     * No need for search of Nodes with a tag, since we assume only one camera
     * in the scene
     */
    public class Camera3DMoveSystem : NodeProcessingSystem
    {
        const float duration = 1f;                  //2 second
        public Camera3DMoveSystem(SceneComponent _scene, string _tag, bool _recursive = false) : base(_scene, _tag, _recursive)
        {
            // node with camera tag for 3D movements using a timer
        }
        public override void Process()
        {
            base.Process();

            FiniteTimeAction action = null;

            if (Scene.Input.GetKeyPress(Key.W))
                action = new MoveBy(duration, new Vector3(0, 0, 5));

            if (Scene.Input.GetKeyPress(Key.S))
                action = new MoveBy(duration, new Vector3(0, 0, -5));

            if (Scene.Input.GetKeyPress(Key.A))
                action = new MoveBy(duration, new Vector3(-5, 0, 0));

            if (Scene.Input.GetKeyPress(Key.D))
                action = new MoveBy(duration, new Vector3(5, 0, 0));

            if (action != null)
            {
                //
                // move the camera using timer
                //
                Scene.CameraNode.RunActionsAsync(action);
            }
        }
    }
}

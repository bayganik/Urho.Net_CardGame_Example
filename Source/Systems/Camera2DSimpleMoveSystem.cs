using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Actions;
using Urho.Shapes;


namespace CardGameExample
{
    /*
     * 2D camera move system using Translate
     * No need for search of Nodes with a tag, since we assume only one camera
     * in the scene
     */
    class Camera2DSimpleMoveSystem : NodeProcessingSystem
    {
        const float moveSpeed = 4f;
        public Camera2DSimpleMoveSystem(SceneComponent _scene, string _tag, bool _recursive = false) : base(_scene, _tag, _recursive)
        {
            // node with camera tag for 2D movements
        }
        public override void Process()
        {
            base.Process();

            //
            // Read WASD keys and move the camera scene node to the corresponding direction
            //
            if (Scene.Input.GetKeyDown(Key.W))
                Scene.CameraNode.Translate(Vector3.UnitY * moveSpeed * Global.DeltaTime);
            if (Scene.Input.GetKeyDown(Key.S))
                Scene.CameraNode.Translate(-Vector3.UnitY * moveSpeed * Global.DeltaTime);
            if (Scene.Input.GetKeyDown(Key.A))
                Scene.CameraNode.Translate(-Vector3.UnitX * moveSpeed * Global.DeltaTime);
            if (Scene.Input.GetKeyDown(Key.D))
                Scene.CameraNode.Translate(Vector3.UnitX * moveSpeed * Global.DeltaTime);

            if (Scene.Input.GetKeyDown(Key.PageUp))
                Scene.Camera.Zoom = Scene.Camera.Zoom * 1.01f;          //zoom in
            if (Scene.Input.GetKeyDown(Key.PageDown))
                Scene.Camera.Zoom = Scene.Camera.Zoom * 0.99f;          //zoom out
        }
    }
}

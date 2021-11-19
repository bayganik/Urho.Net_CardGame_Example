using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Actions;
using Urho.Shapes;


namespace CardGameExample
{
    /* 
     * 3D camera move system using Translate
     * No need for search of Nodes with a tag, since we assume only one camera
     * in the scene
     * Also FPS using the mouse movement 
     */
    class Camera3DSimpleMoveSystem : NodeProcessingSystem
    {
        const float moveSpeed = 10f;
        const float mouseSensitivity = .1f;

        public Camera3DSimpleMoveSystem(SceneComponent _scene, string _tag, bool _recursive = false) : base(_scene, _tag, _recursive)
        {
            // node with camera tag for 3D movements
        }
        public override void Process()
        {
            base.Process();

            IntVector2 mouseMove = Scene.Input.MouseMove;
            Scene.Yaw += mouseSensitivity * mouseMove.X;
            Scene.Pitch += mouseSensitivity * mouseMove.Y;
            Scene.Pitch = MathHelper.Clamp(Scene.Pitch, -90, 90);

            Scene.CameraNode.Rotation = new Quaternion(Scene.Pitch, Scene.Yaw, 0);
            //
            // Read WASD keys and move the camera scene node to the corresponding direction
            //
            if (Scene.Input.GetKeyDown(Key.W))
                Scene.CameraNode.Translate(Vector3.UnitZ * moveSpeed * Global.DeltaTime);
            if (Scene.Input.GetKeyDown(Key.S))
                Scene.CameraNode.Translate(-Vector3.UnitZ * moveSpeed * Global.DeltaTime);
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

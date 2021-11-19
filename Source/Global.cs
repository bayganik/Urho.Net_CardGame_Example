
using System;
using System.Diagnostics;
using System.Globalization;
using Urho.Resources;
using Urho.Gui;
using Urho;
using Urho.Urho2D;

namespace CardGameExample
{
    public class NodeProcessingSystem
    {
        /*
         * The system part of ECS framework. Executed on every update frame
         * Allows for processing of Nodes outside of Scene component for separation of concerns
         * 
         * You can use the Tag property to group Nodes together or use Component to group them
         * See the Process method
         */
        public SceneComponent Scene;
        public Node[] Nodes;
        public string Tag;
        public bool Recursive;
        public NodeProcessingSystem(SceneComponent _scene, string _tag, bool _recursive = false)
        {
            Tag = _tag;
            Scene = _scene;
            Recursive = _recursive;
        }
        public virtual void Process()
        {
            //Nodes = Scene.MyScene.GetChildrenWithComponent<CardInfo>();
            //Nodes = Scene.MyScene.GetChildrenWithTag(Tag, Recursive);
            //foreach(Node nd in Nodes)
            //{

            //}
        }
    }
    public class mmWindow : Window
    {
        /*
         * A window element with title bar and close button
         */
        public Button ButtonClose;
        public Text WindowTitle;
        public UIElement TitleBar;
        public mmWindow(IntVector2 _pos, IntVector2 _size, string _title = "window")
        {
            base.SetMinSize(_size.X, _size.Y);
            base.Position = _pos;
            base.Movable = true;

            base.SetLayout(LayoutMode.Vertical, 0, new IntRect(10, 10, 10, 10));
            base.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            base.Name = "mmWindow";

            TitleBar = new UIElement();
            TitleBar.SetMinSize(0, 24);
            TitleBar.VerticalAlignment = VerticalAlignment.Top;
            TitleBar.LayoutMode = LayoutMode.Horizontal;
            //
            // Must create title this way, so it can get updated
            //
            WindowTitle = base.CreateChild<Text>(new StringHash("Text"));
            WindowTitle.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            WindowTitle.Value = _title;
            WindowTitle.SetFont(Global.ResourceCache.GetFont("Fonts/BlueHighway.ttf"), 10);

            ButtonClose = new Button();
            ButtonClose.Name = "CloseButton";

            TitleBar.AddChild(WindowTitle);
            TitleBar.AddChild(ButtonClose);
            base.AddChild(TitleBar);

            base.SetStyleAuto(null);
            WindowTitle.SetStyleAuto(null);

        }
        public void SetTitle(string _text)
        {
            WindowTitle.Value = _text;
        }
    }
    public class mmCheckBox : CheckBox
    {
        /*
         * A checkbox element with text description
         */
        public string Text { get; set; }
        Text cbTxt;
        public mmCheckBox(string _text = " ", string _name = "mmCheckbox")
        {
            base.Name = _name;
            Text = _text;

            //
            // Text display next to checkbox
            //
            cbTxt = this.CreateChild<Text>(new StringHash("Text"));
            cbTxt.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Top);
            SetText(_text);
            cbTxt.SetFont(Global.ResourceCache.GetFont("Fonts/BlueHighway.ttf"), 10);
        }
        public void SetText(string _text)
        {
            Text = _text;
            cbTxt.Value = "     " + Text;
        }
        public void SetFontSize(int _size)
        {
            cbTxt.SetFont(Global.ResourceCache.GetFont("Fonts/BlueHighway.ttf"), _size);
        }
    }
    public class mmButton : Button
    {
        /*
         * A button element with text alligned in the center
         */
        public string Text { get; set; }
        Text cbTxt;
        public mmButton(string _text = " ", string _name = "mmButton")
        {
            base.Name = _name;
            base.MinHeight = 30;
            Text = _text;

            //
            // Text display next to checkbox
            //
            cbTxt = this.CreateChild<Text>(new StringHash("Text"));
            cbTxt.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            SetText(_text);
            cbTxt.SetFont(Global.ResourceCache.GetFont("Fonts/BlueHighway.ttf"), 10);
        }
        public void SetText(string _text)
        {
            Text = _text;
            cbTxt.Value = Text;
        }
        public void SetFontSize(int _size)
        {
            cbTxt.SetFont(Global.ResourceCache.GetFont("Fonts/BlueHighway.ttf"), _size);
        }
        public void SetMinHeight(int _size)
        {
            base.MinHeight = _size;
        }
    }
    public class mmLineEdit : LineEdit
    {
        /*
         * A line edit element loaded with a value
         */
        public mmLineEdit(string _text = " ", string _name = "mmLineEdit")
        {
            base.Name = _name;
            base.MinHeight = 30;
            SetText(_text);
            base.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Center);
            base.TextSelectable = true;
        }
        public void SetText(string _text)
        {

            base.Text = _text;
        }
        public void SetMinHeight(int _size)
        {
            base.MinHeight = _size;
        }
    }
    public enum GameState
    {
        GS_INTRO = 0,
        GS_PLAY,
        GS_END,
        GS_DEAD
    };
    /*
     * Global values used EVERY where in the game, this will avoid passing data thru parameters
     */
    public static class Global
    {
        public static GameState GameState = GameState.GS_INTRO;
        public static ResourceCache ResourceCache;
        public static PhysicsWorld2D PhysicsWorld;

        public const float CameraMinDist = 1.0f;
        public const float CameraInitialDist = 5.0f;
        public const float CameraMaxDist = 20.0f;

        public const float GyroscopeThreshold = 0.1f;

        public const int CtrlUp = 1;
        public const int CtrlDown = 2;
        public const int CtrlLeft = 4;
        public const int CtrlRight = 8;
        public const int CtrlHit = 16;

        public const float MoveForce = 0.8f;
        public const float InairMoveForce = 0.02f;
        public const float BrakeForce = 0.2f;
        public const float JumpForce = 7.0f;
        public const float YawSensitivity = 0.1f;
        public const float InairThresholdTime = 0.1f;

        public static float DeltaTime;
        //
        // Generate a random number
        //
        static readonly Random random = new Random();
        /// Return a random float between 0.0 (inclusive) and 1.0 (exclusive.)
        public static float NextRandom() 
        { 
            return (float)random.NextDouble(); 
        }
        /// Return a random float between 0.0 and range, inclusive from both ends.
        public static float NextRandom(float range) 
        { 
            return (float)random.NextDouble() * range; 
        }
        /// Return a random float between min and max, inclusive from both ends.
        public static float NextRandom(float min, float max) 
        { 
            return (float)((random.NextDouble() * (max - min)) + min); 
        }
        /// Return a random integer between min and max - 1.
        public static int NextRandom(int min, int max) 
        { 
            return random.Next(min, max); 
        }
    }
}

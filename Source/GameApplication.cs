
using System;
using System.Diagnostics;
using System.Globalization;
using Urho.Resources;
using Urho.Gui;
using Urho;

namespace CardGameExample
{
    /*
     * Actual game application used to start everything
     */
    public class GameApplication : Application
    {
        UrhoConsole console;
        DebugHud debugHud;

        Sprite logoSprite;
        UI ui;

        public const float TouchSensitivity = 2;
        public bool TouchEnabled { get; set; }
        public MonoDebugHud MonoDebugHud { get; set; }

        [Preserve]
        public GameApplication(ApplicationOptions options = null) : base(options) { }

        static GameApplication()
        {
            Urho.Application.UnhandledException += Application_UnhandledException1;
        }

        static void Application_UnhandledException1(object sender, Urho.UnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached && !e.Exception.Message.Contains("BlueHighway.ttf"))
                Debugger.Break();
            e.Handled = true;
        }

        protected bool IsLogoVisible
        {
            get { return logoSprite.Visible; }
            set { logoSprite.Visible = value; }
        }
        /// <summary>
        /// Joystick XML layout for mobile platforms
        /// </summary>
        protected virtual string JoystickLayoutPatch => string.Empty;

        protected override void Start()
        {
            base.Start();

            Input.Enabled = true;
            MonoDebugHud = new MonoDebugHud(this);
            MonoDebugHud.Show();

            CreateLogo();
            SetWindowAndTitleIcon();
            CreateConsoleAndDebugHud();
            //
            // setup the Global values
            //
            Global.ResourceCache = this.ResourceCache;

        }

        void CreateLogo()
        {
            float scale = 0.25f;

            var logoTexture = ResourceCache.GetTexture2D("Textures/LogoLarge.png");

            if (logoTexture == null)
                return;

            ui = UI;
            logoSprite = ui.Root.CreateSprite();
            logoSprite.Texture = logoTexture;
            int w = logoTexture.Width;
            int h = logoTexture.Height;
            logoSprite.SetScale(scale);
            logoSprite.SetSize(w, h);
            logoSprite.SetHotSpot(0, h);
            logoSprite.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Bottom);
            logoSprite.Opacity = 0.75f;
            logoSprite.Priority = -100;
        }

        void SetWindowAndTitleIcon()
        {
            var icon = ResourceCache.GetImage("Textures/UrhoIcon.png");
            Graphics.SetWindowIcon(icon);
            Graphics.WindowTitle = "mmGame3D";
        }

        void CreateConsoleAndDebugHud()
        {
            var xml = ResourceCache.GetXmlFile("UI/DefaultStyle.xml");
            console = Engine.CreateConsole();
            console.DefaultStyle = xml;
            console.Background.Opacity = 0.8f;

            debugHud = Engine.CreateDebugHud();
            debugHud.DefaultStyle = xml;
        }

        void InitTouchInput()
        {
            TouchEnabled = true;
            var layout = ResourceCache.GetXmlFile("UI/ScreenJoystick_Samples.xml");
            if (!string.IsNullOrEmpty(JoystickLayoutPatch))
            {
                XmlFile patchXmlFile = new XmlFile();
                patchXmlFile.FromString(JoystickLayoutPatch);
                layout.Patch(patchXmlFile);
            }
            var screenJoystickIndex = Input.AddScreenJoystick(layout, ResourceCache.GetXmlFile("UI/DefaultStyle.xml"));
            Input.SetScreenJoystickVisible(screenJoystickIndex, true);
        }
    }
}

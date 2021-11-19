using Urho;
using Urho.Gui;
using Urho.Resources;
using Urho.IO;
using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

using System.Collections;

namespace CardGameExample
{
    /// <summary>
    /// Actual game application to play a particular SceneComponent
    /// </summary>
	public class CardGameExample : GameApplication
    {
        bool isMobile = false;
        protected SceneComponent currentSample = null;
       public CardGameExample(ApplicationOptions options) : base(options) { }
        /// <summary>
        /// Initialize all engine objects here
        /// </summary>
        protected override void Start()
        {
            base.Start();

            Input.KeyDown += HandleKeyDown;

            isMobile = (Platform == Platforms.iOS || Platform == Platforms.Android);
            //
            // play a particular scene
            //
            currentSample = Activator.CreateInstance<Urho2DCards>();
            currentSample.Run();
        }
        void HandleKeyDown(KeyDownEventArgs e)
        {
            Exit();
        }
    }
}


using Urho;
using System.Collections.Generic;
using Urho.Gui;

namespace CardGameExample
{
    /*
     * Information about UIElements in the scene
     */
    public class ElementInfo
    {
        public UIElement Element { get; set; }
        public IntVector2 Start { get; set; }
        public IntVector2 Delta { get; set; }
        public int Buttons { get; set; }

        public ElementInfo(UIElement element, IntVector2 start, IntVector2 delta, int buttons)
        {
            Element = element;
            Start = start;
            Delta = delta;
            Buttons = buttons;
        }
    }
}

using Urho;
using Urho.Gui;
using System.Diagnostics;
using System.Text;

namespace Urho
{
    public static class UIElements
    {
		public static bool ContainsScreenPos(this UIElement e, IntVector2 screenPos)
		{
			IntVector2 pos = e.ScreenToElement(screenPos);
			IntVector2 size = e.Size;
			return pos.X >= 0 && pos.X < size.X && pos.Y >= 0 && pos.Y < size.Y;
		}
		
        public static string ToStringUI(this UIElement e)
        {
            var p = e.Position;
            var s = e.Size;
            var sp = e.ScreenPosition;
            return $"{e.GetType().Name} @ {p.X},{p.Y} size: {s.X}x{s.Y} screenPos: {sp.X},{sp.Y}";
        }

        public static void DumpThisUI(this UIElement e, int indent = 0)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < indent; ++i)
                builder.Append("  ");
            builder.Append(e.ToStringUI());
            Debug.WriteLine(builder.ToString());
        }

        public static void DumpUI(this UIElement e, int indent = 0)
        {
            e.DumpThisUI(indent);
            foreach (var c in e.Children)
                c.DumpUI(indent + 1);
        }
    }
}

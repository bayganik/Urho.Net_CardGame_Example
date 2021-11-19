using Urho;
using Urho.IO;
using Urho.Resources;

namespace Urho
{
	public abstract class MoreUrho
	{
		protected Log Log => Application.Log;
		protected ResourceCache Cache => Application.Current.ResourceCache;
		protected Graphics Graphics => Application.Current.Graphics;
	}
}

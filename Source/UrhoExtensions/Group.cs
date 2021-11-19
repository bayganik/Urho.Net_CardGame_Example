using System.Collections.Generic;
using Urho;

namespace Urho
{
	public class Group<T> : MoreNode where T : Node
	{
		public Group(Node parent, Args args = null)
			: base(parent, args)
		{
		}

		public new IEnumerable<T> Children
		{
			get
			{
				foreach (var t in base.Children)
					yield return (T)t;
			}
		}
	}
}

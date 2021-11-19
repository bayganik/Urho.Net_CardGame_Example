using Urho;

namespace Urho
{
	public class MoreNode : Node
	{
		public class Args
		{
			public Vector3 Position;
		}

		public MoreNode(Node parent, Args args = null)
		{
			if (args == null) args = new Args();

			parent.AddChild(this);
			Position = new Vector3(args.Position);
		}
	}
}

using Urho;

namespace Urho
{
	public static class Vectors
	{
		// C# has extension methods but not extension operators?? Weird!
		public static Vector3 Multiply(this Vector3 a, Vector3 b)
		{
			return Vector3.Multiply(a, b);
		}

		public static Vector3 WorldToLocal(this Vector3 vector, Node node)
		{
			return node.WorldToLocal(vector);
		}

		public static Vector3 LocalToWorld(this Vector3 vector, Node node)
		{
			return node.LocalToWorld(vector);
		}

		public static Vector3 ConvertFromTo(this Vector3 vector, Node src, Node dest)
		{
			return vector.LocalToWorld(src).WorldToLocal(dest);
		}

		public static Vector3 Norm(this Vector3 vector)
		{
			return Vector3.Normalize(vector);
		}

		public static IntVector2 Int(this Vector2 vector)
		{
			return new IntVector2((int)vector.X, (int)vector.Y);
		}

		public static Vector2 Float(this IntVector2 ivec)
		{
			return new Vector2(ivec.X, ivec.Y);
		}

		public static Vector2 Clamp(this Vector2 vec, Vector2 min, Vector2 max)
		{
			return Vector2.Clamp(vec, min, max);
		}

		public static Vector3 Clamp(this Vector3 vec, Vector3 min, Vector3 max)
		{
			return Vector3.Clamp(vec, min, max);
		}

		public static Vector2 WithX(this Vector2 v, float x)
		{
			Vector2 result = v;
			result.X = x;
			return result;
		}

		public static Vector2 WithY(this Vector2 v, float y)
		{
			Vector2 result = v;
			result.Y = y;
			return result;
		}

		public static Vector3 WithX(this Vector3 v, float x)
		{
			Vector3 result = v;
			result.X = x;
			return result;
		}

		public static Vector3 WithY(this Vector3 v, float y)
		{
			Vector3 result = v;
			result.Y = y;
			return result;
		}

		public static Vector3 WithZ(this Vector3 v, float z)
		{
			Vector3 result = v;
			result.Z = z;
			return result;
		}

		public static Vector3 WithZ(this Vector2 v, float z)
		{
			return new Vector3(v.X, v.Y, z);
		}

		public static IntVector2 WithX(this IntVector2 v, int x)
		{
			IntVector2 result = v;
			result.X = x;
			return result;
		}

		public static IntVector2 WithY(this IntVector2 v, int y)
		{
			IntVector2 result = v;
			result.Y = y;
			return result;
		}

		public static Color ToUrhoColor(this Vector3 v)
		{
			return new Color(v.X, v.Y, v.Z);
		}
	}
}

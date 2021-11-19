using Urho;

namespace Urho
{
    public static class Matrices
    {
        /// Multiply a Vector3 which is assumed to represent position.
        public static Vector3 Transform(this Matrix3x4 m, ref Vector3 rhs)
        {
            return new Vector3(
                (m.m00 * rhs.X + m.m01 * rhs.Y + m.m02 * rhs.Z + m.m03),
                (m.m10 * rhs.X + m.m11 * rhs.Y + m.m12 * rhs.Z + m.m13),
                (m.m20 * rhs.X + m.m21 * rhs.Y + m.m22 * rhs.Z + m.m23)
            );
        }

        public static Vector3 Transform(this Matrix3x4 m, ref Vector4 rhs)
        {
            return new Vector3(
                (m.m00 * rhs.X + m.m01 * rhs.Y + m.m02 * rhs.Z + m.m03 * rhs.W),
                (m.m10 * rhs.X + m.m11 * rhs.Y + m.m12 * rhs.Z + m.m13 * rhs.W),
                (m.m20 * rhs.X + m.m21 * rhs.Y + m.m22 * rhs.Z + m.m23 * rhs.W)
            );
        }
    }
}

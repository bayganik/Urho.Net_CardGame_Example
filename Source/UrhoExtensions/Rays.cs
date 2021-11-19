using System;
using Urho;

namespace Urho
{
    public static class Rays
    {
        public static Ray WorldToLocal(this Ray ray, Node node)
        {
            return node.WorldToLocal(ray);
        }

        public static Ray LocalToWorld(this Ray ray, Node node)
        {
            return node.LocalToWorld(ray);
        }

        public static Ray ConvertFromTo(this Ray ray, Node src, Node dest)
        {
            return ray.LocalToWorld(src).WorldToLocal(dest);
        }

        public static Ray OffsetBy(this Ray ray, Vector3 offset)
        {
            return new Ray(ray.Origin + offset, ray.Direction);
        }

        public static Ray ScaledBy(this Ray ray, float scale)
        {
            return new Ray(ray.Origin * scale, ray.Direction);
        }

        public static Vector3 NearestPointOnSphereAt(this Ray ray, Vector3 center, float radius)
        {
            return ray.OffsetBy(-center).NearestPointOnSphere(radius) + center;
        }

        public static Vector3 NearestPointOnSphere(this Ray ray, float radius)
        {
            if (radius <= 0)
            {
                return Vector3.Zero;
            }
            return ray.ScaledBy(1 / radius).NearestPointOnUnitSphere() * radius;
        }

        public static Vector3 NearestPointOnUnitSphere(this Ray ray)
        {
            Vector3 proj = ray.Project(Vector3.Zero);
            if (proj.LengthSquared <= 1)
            {
                // Closest approach is inside the sphere.
                // Move back along the ray until we touch the surface.
                float distance = (float)Math.Sqrt(1 - proj.LengthSquared);
                return proj - distance * ray.Direction;
            }
            else
            {
                // Closest approach is outside the sphere.
                // All tangent rays from the current ray define a circle.
                // The result we want is a point on that circle.
                Vector3 center = ray.Origin / ray.Origin.LengthSquared;
                float radius = (float)Math.Sqrt(1 - center.LengthSquared);

                // Get the plane containing the circle, and find where our ray hits it.
                Plane plane = new Plane(normal: ray.Origin, point: center);
                Vector3 hit = ray.Origin + ray.Direction * ray.HitDistance(plane);

                // Move 'radius' units from center -> hit
                Vector3 result = center + Vector3.Normalize(hit - center) * radius;
                return result;
            }
        }
    }
}

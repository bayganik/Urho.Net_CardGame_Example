using Urho;

namespace Urho
{
    public static class Nodes
    {
        public static T CreateChildComponent<T>(this Node parent) where T : Component
        {
            string name = typeof(T).Name;
            Node child = parent.CreateChild(name);
            return child.CreateComponent<T>();
        }

        public static void RotateX(this Node node, float angle, TransformSpace transformSpace = TransformSpace.Parent)
        {
            node.Pitch(angle, transformSpace);
        }

        public static void RotateY(this Node node, float angle, TransformSpace transformSpace = TransformSpace.Parent)
        {
            node.Yaw(angle, transformSpace);
        }

        public static void RotateZ(this Node node, float angle, TransformSpace transformSpace = TransformSpace.Parent)
        {
            node.Roll(angle, transformSpace);
        }

        public static Ray WorldToLocal(this Node node, Ray ray)
        {
            return new Ray(
                node.WorldToLocal(ray.Origin),
                node.WorldToLocal(ray.Direction).Norm());
        }

        public static Ray LocalToWorld(this Node node, Ray ray)
        {
            return new Ray(
                node.LocalToWorld(ray.Origin),
                node.LocalToWorld(ray.Direction).Norm());
        }
    }
}

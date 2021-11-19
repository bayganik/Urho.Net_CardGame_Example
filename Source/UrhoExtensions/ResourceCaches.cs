using System;
using Urho;
using Urho.Resources;

namespace Urho
{
    public static class ResourceCaches
    {
        public static RenderPath GetRenderPath(this ResourceCache cache, string name)
        {
            var xml = cache.GetXmlFile(name);
            var rp = new RenderPath();
            if (!rp.Load(xml))
            {
                throw new Exception("Failed to load render path: " + name);
            }
            return rp;
        }

        public static string GetText(this ResourceCache cache, string name)
        {
            return cache.GetFile(name).ReadText();
        }

        public static string GetText(this ResourceCache cache, string name, System.Text.Encoding encoding)
        {
            return cache.GetFile(name).ReadText(encoding);
        }

        public static byte[] GetBinary(this ResourceCache cache, string name)
        {
            return cache.GetFile(name).ReadBinary();
        }
    }
}
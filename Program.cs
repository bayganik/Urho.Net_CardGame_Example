using Urho;

namespace CardGameExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new ApplicationOptions()
            {
                ResourcePaths = new[] { "Data;CoreData" },
                Width = 1200,
                Height = 800,
                ResizableWindow = true
            };
            try
            {
                new CardGameExample(options).Run();
            }
            catch
            { }
        }
    }
}


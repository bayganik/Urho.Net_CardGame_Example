using Urho;
using Urho.IO;

namespace Urho
{
    public static class Logs
    {
        public static void Info(this Log log, object message)
        {
            Log.Write(LogLevel.Info, message.ToString());
        }

        public static void Warn(this Log log, object message)
        {
            Log.Write(LogLevel.Warning, message.ToString());
        }

        public static void Error(this Log log, object message)
        {
            Log.Write(LogLevel.Error, message.ToString());
        }
    }
}

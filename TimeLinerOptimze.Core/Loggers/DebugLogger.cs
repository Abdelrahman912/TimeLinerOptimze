using System.Diagnostics;

namespace TimeLinerOptimze.Core.Loggers
{
    public class DebugLogger : ILogger
    {
        public void Log<T>(T t) where T : class
        {
            Debug.WriteLine(t.ToString());
        }
    }
}
